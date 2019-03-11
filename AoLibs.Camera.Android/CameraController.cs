using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Camera.Android.Enums;
using AoLibs.Camera.Android.Views;
using AoLibs.Utilities.Android.Listeners;
using Java.IO;
using Java.Lang;
using Java.Util.Concurrent;
using Double = Java.Lang.Double;
using Exception = Java.Lang.Exception;
using Math = Java.Lang.Math;
using Object = Java.Lang.Object;
using Orientation = Android.Media.Orientation;
using Path = System.IO.Path;
using Semaphore = Java.Util.Concurrent.Semaphore;

namespace AoLibs.Camera.Android
{
    public partial class CameraController5000
    {
        private readonly Activity _activity;
        private readonly AutoFitTextureView _autoFitTextureView;
        private readonly FaceRectView _faceRectView;
        public const string FocusRequestTag = "FocusRequestTag";
        public event EventHandler<Face[]> OnNewFacesDetected;


        private const string Tag = nameof(CameraController5000);
        private const int MaxPreviewFps = 24;

        private static readonly Dictionary<int, int> DngOrientation = new Dictionary<int, int>
        {
            {0, (int) Orientation.Normal},
            {90, (int) Orientation.Rotate90},
            {180, (int) Orientation.Rotate180},
            {270, (int) Orientation.Rotate270}
        };

        private readonly Semaphore _cameraOpenCloseLock = new Semaphore(1);

        private readonly ImageReader.IOnImageAvailableListener _imageCallback;

        private ImageSaver _imageSaver;
        private MediaRecorder mMediaRecorder;
        public CameraCaptureSession.CaptureCallback _sessionCaptureCallback { get; private set; }
        public Handler _backgroundHandler { get; private set; }
        private HandlerThread _backgroundHandlerThread;
        private DateTime _recordingStartTime;
        private SemaphoreSlim _textureReadySemaphore;
        private bool _firstRun = true;
        private SemaphoreSlim _photoTakingSemaphore = new SemaphoreSlim(0);

        private string _cameraId;

        private Queue<CaptureResult> _captureResultQueue;

        private CameraCharacteristics _characteristics;

        private int _dsiHeight;
        private int _dsiWidth;

        private ImageFormatType _imageFormat;
        private List<ImageFormatType> _imageFormatList;

        private Handler _imageSavingHandler;
        private HandlerThread _imageSavingHandlerThread;

        private bool _isAeTriggered;
        private bool _isAfTriggered;

        private ImageReader _jpegReader;
        private int _lastOrientation;
        private int _lensFacing;
        private List<int> _lensFacingList;
        private OrientationEventListener _orientationListener;

        private Size _pictureSize;
        private Size _videoSize;
        private Size _previewSize;

        public CameraState CurrentState { get; private set; } = CameraState.Idle;
        public CameraCharacteristics CurrentCameraCharacteristics => _characteristics;

        public CaptureRequest.Builder PreviewBuilder { get; private set; }
        private CaptureRequest.Builder _captureBuilder;

        private ImageReader _rawReader;
        private CameraDevice _CameraDevice;
        private CameraManager _CameraManager;
        public CameraCaptureSession _CameraSession { get; private set; }
        public CameraAdapter CameraAdapter { get; set; }

        private List<(Size videoSize, List<Range> fpsRanges)> _videoConfigurations = new List<(Size videoSize, List<Range> fpsRanges)>();
        private string _currentTemporaryVideoFilePath;
        private string _targetVideoPath;

        public CameraController5000(
            Activity activity, 
            AutoFitTextureView autoFitTextureView, 
            FaceRectView faceRectView = null)
        {
            _activity = activity;
            _autoFitTextureView = autoFitTextureView;
            _faceRectView = faceRectView;
            CameraAdapter = new CameraAdapter(this);
        }


        #region Lifecycle



        public void OnPause()
        {
            CurrentState = CameraState.Closing;

            SetOrientationListener(false);

            CloseCamera();
            StopBackgroundThread();


            _CameraDevice = null;
        }

        public void OnResume()
        {
            var displayMetrics = new DisplayMetrics();
            _activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            _dsiHeight = displayMetrics.HeightPixels;
            _dsiWidth = displayMetrics.WidthPixels;

            var realMetrics = new DisplayMetrics();
            _activity.WindowManager.DefaultDisplay.GetRealMetrics(realMetrics);


            CurrentState = CameraState.Idle;

            StartBackgroundThread();

            _captureResultQueue = new Queue<CaptureResult>();

            InitCamera();
            SetOrientationListener(true);
            CheckRequiredFeatures();
            OpenCamera(_lensFacing);
        }

        private void CloseCamera()
        {
            try
            {
                _cameraOpenCloseLock.Acquire();

                if (_CameraSession != null)
                {
                    _CameraSession.Close();
                    _CameraSession = null;
                }

                if (_CameraDevice != null)
                {
                    _CameraDevice.Close();
                    _CameraDevice = null;
                }

                if (_jpegReader != null)
                {
                    _jpegReader.Close();
                    _jpegReader = null;
                }

                if (_rawReader != null)
                {
                    _rawReader.Close();
                    _rawReader = null;
                }

                if (mMediaRecorder != null)
                {
                    mMediaRecorder.Release();
                    mMediaRecorder = null;
                }

                _CameraManager = null;
            }
            catch (InterruptedException e)
            {
                Log.Error(Tag, "Interrupted while trying to lock camera closing.", e);
            }
            finally
            {
                _cameraOpenCloseLock.Release();
            }
        }

        private void OpenCamera(int facing)
        {
            try
            {
                _lensFacing = facing;

                if (!_cameraOpenCloseLock.TryAcquire(3000, TimeUnit.Microseconds))
                {
                    return;
                }


                _cameraId = null;

                // Find camera device that facing to given facing parameter.
                foreach (var id in _CameraManager.GetCameraIdList())
                {
                    var cameraCharacteristics = _CameraManager.GetCameraCharacteristics(id);
                    if ((int)cameraCharacteristics.Get(CameraCharacteristics.LensFacing) == facing)
                    {
                        _cameraId = id;
                        break;
                    }
                }

                if (_cameraId == null)
                {
                    return;
                }

                // acquires camera characteristics
                _characteristics = _CameraManager.GetCameraCharacteristics(_cameraId);

                var streamConfigurationMap =
                    (StreamConfigurationMap)_characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

                // Acquires supported preview size list that supports SurfaceTexture
                _previewSize =
                    GetOptimalPreviewSize(
                        streamConfigurationMap.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))),
                        (double)_pictureSize.Width / _pictureSize.Height);

                _autoFitTextureView.PreviewSize = _previewSize;
                SetAspectRatioTextureView(_previewSize.Width, _previewSize.Height);
                _videoSize = _previewSize;

                foreach (var videoSize in streamConfigurationMap.GetHighSpeedVideoSizes())
                {
                    var speeds = new List<Range>();
                    foreach (var fpsRange in streamConfigurationMap.GetHighSpeedVideoFpsRangesFor(videoSize))
                    {
                        //we will record constant fps video with highest fps for given size
                        if (fpsRange.Lower.Equals(fpsRange.Upper))
                        {
                            speeds.Add(fpsRange);
                        }
                    }
                    _videoConfigurations.Add((videoSize, speeds));
                }

                Log.Debug(Tag, "Picture Size: " + _pictureSize + " Preview Size: " + _previewSize);

                // Configures an ImageReader
                _jpegReader = ImageReader.NewInstance(_pictureSize.Width, _pictureSize.Height, ImageFormatType.Jpeg, 1);
                _jpegReader.SetOnImageAvailableListener(_imageCallback, _imageSavingHandler);

                if (((int[])_characteristics.Get(CameraCharacteristics.RequestAvailableCapabilities)).Contains(
                     (int)RequestAvailableCapabilities.Raw))
                {
                    var rawSizeList = new List<Size>();

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M &&
                        streamConfigurationMap.GetHighResolutionOutputSizes((int)ImageFormatType.RawSensor) != null)
                        rawSizeList.AddRange(
                            streamConfigurationMap.GetHighResolutionOutputSizes((int)ImageFormatType.RawSensor));
                    rawSizeList.AddRange(streamConfigurationMap.GetOutputSizes((int)ImageFormatType.RawSensor));

                    var rawSize = rawSizeList[0];

                    _rawReader = ImageReader.NewInstance(rawSize.Width, rawSize.Height, ImageFormatType.RawSensor, 1);
                    _rawReader.SetOnImageAvailableListener(_imageCallback, _imageSavingHandler);

                    _imageFormatList = new List<ImageFormatType>
                    {
                        ImageFormatType.Jpeg,
                        ImageFormatType.RawSensor
                    };
                }
                else
                {
                    if (_rawReader != null)
                    {
                        _rawReader.Close();
                        _rawReader = null;
                    }

                    _imageFormatList = new List<ImageFormatType>
                    {
                        ImageFormatType.Jpeg
                    };
                }

                _imageFormat = ImageFormatType.Jpeg;

                // Set the aspect ratio to TextureView
                var orientation = _activity.Resources.Configuration.Orientation;
                if (orientation == global::Android.Content.Res.Orientation.Landscape)
                {
                    _autoFitTextureView.SetAspectRatio(_previewSize.Width, _previewSize.Height);
                    _faceRectView?.SetAspectRatio(_previewSize.Width, _previewSize.Height);
                }
                else
                {
                    _autoFitTextureView.SetAspectRatio(_previewSize.Height, _previewSize.Width);
                    _faceRectView?.SetAspectRatio(_previewSize.Height, _previewSize.Width);
                }

                // calculate transform matrix for face rect view
                if (_faceRectView != null)
                {
                    ConfigureFaceRectTransform();
                }                

                // Opening the camera device here
                _CameraManager.OpenCamera(_cameraId, new CameraDeviceStateCallback(this), _backgroundHandler);
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot open the camera. ({nameof(OpenCamera)})", e);
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera opening.", e);
            }
        }

        #endregion

        #region Initialization

        private void CheckRequiredFeatures()
        {
            try
            {
                // Find available lens facing value for this device
                var lensFacings = new HashSet<int>();
                foreach (var id in _CameraManager.GetCameraIdList())
                {
                    var cameraCharacteristics = _CameraManager.GetCameraCharacteristics(id);
                    lensFacings.Add((int)cameraCharacteristics.Get(CameraCharacteristics.LensFacing));
                }

                _lensFacingList = new List<int>(lensFacings);
                _lensFacing = _lensFacingList[0];

                SetDefaultJpegSize(_CameraManager, _lensFacing);
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(CheckRequiredFeatures)})", e);
            }
        }

        private void SetOrientationListener(bool isEnable)
        {
            if (_orientationListener == null)
            {
                _orientationListener = new OrientationEventListenerCustom(this);

                if (isEnable)
                    _orientationListener.Enable();
                else
                    _orientationListener.Disable();
            }
        }

        private void InitCamera()
        {
            _CameraManager = (CameraManager)_activity.GetSystemService(Context.CameraService);
            _autoFitTextureView.SurfaceTextureListener = new SurfaceTextureListener(this);
        }

        public void RegisterViewForSetFocus(View v)
        {
            v.SetOnTouchListener(new OnTouchListener(e =>
            {
                CameraAdapter.SetFocus(v, e);
            }));
        }

        //private async void RecreateTexture()
        //{
        //    if (_firstRun)
        //    {
        //        _firstRun = false;
        //        return;
        //    }

        //    _textureReadySemaphore = new SemaphoreSlim(0);

        //    var oldTextureView = AutoFitTexture;
        //    var txView = new AutoFitTextureView(Activity)
        //    {
        //        LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
        //            ViewGroup.LayoutParams.MatchParent)
        //        { Gravity = GravityFlags.Center, },
        //        Id = Resource.Id.AutoFitTexture
        //    };
        //    _autoFitTexture = txView;
        //    CameraSection.AddView(txView, 1);

        //    RootView.RequestLayout();

        //    await _textureReadySemaphore.WaitAsync();

        //    CameraSection.RemoveView(oldTextureView);
        //    oldTextureView.Dispose();
        //}
      
        #endregion

        #region Preview

        private async void CreatePreviewSession()
        {
            if (null == _CameraDevice
                || null == _CameraDevice
                || null == _CameraManager
                || null == _previewSize
                || !_autoFitTextureView.IsAvailable)
                return;

            try
            {
                var texture = _autoFitTextureView.SurfaceTexture;

                // Set default buffer size to camera preview size.
                texture.SetDefaultBufferSize(_previewSize.Width, _previewSize.Height);
                var retries = 3;
                while (retries >= 0)
                {
                    try
                    {
                        PrepareMediaRecorder();
                        break;
                    }
                    catch (Exception e)
                    {
                        await Task.Delay(1000);
                    }

                    retries--;
                }


                var surface = new Surface(texture);
                var recSurface = mMediaRecorder.Surface;

                //mProcessor.SetOutputSurface(surface);

                // Creates CaptureRequest.Builder for preview with output target.
                PreviewBuilder = _CameraDevice.CreateCaptureRequest(CameraTemplate.Record);

                //_previewBuilder.Set(CaptureRequest.ControlAeTargetFpsRange, Range.Create(MaxPreviewFps,MaxPreviewFps));
                //_previewBuilder.Set(CaptureRequest.ControlAfMode, CameraMetadata.ControlAfModeContinuousPicture);
                PreviewBuilder.AddTarget(surface);
                PreviewBuilder.AddTarget(recSurface);


                // Creates CaptureRequest.Builder for still capture with output target.
                _captureBuilder = _CameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);

                if (_faceRectView != null)
                {
                    PreviewBuilder.Set(CaptureRequest.StatisticsFaceDetectMode, (int)StatisticsFaceDetectMode.Simple);
                    _captureBuilder.Set(CaptureRequest.StatisticsFaceDetectMode, (int)StatisticsFaceDetectMode.Simple);
                }


                // Creates a CameraCaptureSession here.
                var outputSurfaces = new List<Surface> { surface, _jpegReader.Surface, recSurface };
                if (_rawReader != null) outputSurfaces.Add(_rawReader.Surface);

                _CameraDevice.CreateCaptureSession(outputSurfaces, new CaptureSessionStateCallback(this),
                    _backgroundHandler);
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(CreatePreviewSession)})", e);
            }
        }

        private void SetAspectRatioTextureView(int resolutionWidth, int resolutionHeight)
        {
            if (resolutionWidth > resolutionHeight)
            {
                var newWidth = _dsiWidth;
                var newHeight = _dsiWidth * resolutionWidth / resolutionHeight;
                UpdateTextureViewSize(newWidth, newHeight);
            }
            else
            {
                var newWidth = _dsiWidth;
                var newHeight = _dsiWidth * resolutionHeight / resolutionWidth;
                UpdateTextureViewSize(newWidth, newHeight);
            }
        }

        private Size GetOptimalPreviewSize(Size[] sizes, double targetRatio)
        {
            Size optimalSize = null;
            var minDiff = Double.MaxValue;

            var display = _activity.WindowManager.DefaultDisplay;
            var displaySize = new Point();
            display.GetSize(displaySize);

            // Try to find an size match aspect ratio and size
            foreach (var size in sizes)
            {
                //var wDiff = Math.Abs(displaySize.X - size.Width);
                var hDiff = Math.Abs(displaySize.Y - size.Height);

                var diff = /*wDiff*/ +hDiff;
                if (diff < minDiff)
                {
                    minDiff = diff;
                    optimalSize = size;
                }
            }

            return optimalSize;
        }

        public void StartPreview()
        {
            if (_CameraSession == null) return;

            try
            {
                _CameraSession.StopRepeating();
                // Starts displaying the preview.
                _CameraSession.SetRepeatingRequest(PreviewBuilder.Build(), _sessionCaptureCallback,
                    _backgroundHandler);
                CurrentState = CameraState.Preview;
                CameraAdapter.Init();
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(StartPreview)})", e);
            }
        }

        private void UpdateTextureViewSize(int viewWidth, int viewHeight)
        {
            _autoFitTextureView.LayoutParameters = new FrameLayout.LayoutParams(viewWidth, viewHeight);
        }

        #endregion

        #region Postprocessing

        private async void ConfigureTransform(int viewWidth, int viewHeight, bool asPost = false)
        {
            if (null == _autoFitTextureView || null == _previewSize) return;

            var rotation = _activity.WindowManager.DefaultDisplay.Rotation;
            var matrix = new Matrix();
            var viewRect = new RectF(0, 0, viewWidth, viewHeight);
            var bufferRect = new RectF(0, 0, _previewSize.Height, _previewSize.Width);
            var centerX = viewRect.CenterX();
            var centerY = viewRect.CenterY();

            await Task.Delay(100);

            if (rotation == SurfaceOrientation.Rotation90 || SurfaceOrientation.Rotation270 == rotation)
            {
                bufferRect.Offset(centerX - bufferRect.CenterX(), centerY - bufferRect.CenterY());
                matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
                var scale = Math.Max(
                    (float)viewHeight / _previewSize.Height,
                    (float)viewWidth / _previewSize.Width);
                matrix.PostScale(scale, scale, centerX, centerY);
                matrix.PostRotate(90 * ((int)rotation - 2), centerX, centerY);
            }
            else
            {
                matrix.PostRotate(90 * (int)rotation, centerX, centerY);
            }


            _autoFitTextureView?.SetTransform(matrix);
            _autoFitTextureView?.SurfaceTexture?.SetDefaultBufferSize(_previewSize.Width, _previewSize.Height);
        }


        private void ConfigureFaceRectTransform()
        {
            var orientation = (int)_activity.Resources.Configuration.Orientation;
            var degrees = (int)_activity.WindowManager.DefaultDisplay.Rotation * 90;

            int result;
            if ((int)_characteristics.Get(CameraCharacteristics.LensFacing) == (int)LensFacing.Front)
            {
                result = ((int)_characteristics.Get(CameraCharacteristics.SensorOrientation) + degrees) % 360;
                result = (360 - result) % 360; // compensate the mirror
            }
            else
            {
                result = ((int)_characteristics.Get(CameraCharacteristics.SensorOrientation) - degrees + 360) % 360;
            }

            _faceRectView?.SetTransform(_previewSize,
                (int)_characteristics.Get(CameraCharacteristics.LensFacing),
                result, orientation);
        }

        private void ProcessFace(Face[] faces, Rect zoomRect)
        {
            _activity.RunOnUiThread(() =>
            {
                _faceRectView.SetFaceRect(faces, zoomRect);
                _faceRectView.Invalidate();
                OnNewFacesDetected?.Invoke(this, faces);
            });
        }

        #endregion

        #region Picture

        private int GetJpegOrientation()
        {
            var degrees = _lastOrientation;

            if ((int)_characteristics.Get(CameraCharacteristics.LensFacing) == (int)LensFacing.Front)
                degrees = -degrees;

            return ((int)_characteristics.Get(CameraCharacteristics.SensorOrientation) + degrees + 360) % 360;
        }

        private void SetDefaultJpegSize(CameraManager manager, int facing)
        {
            try
            {
                foreach (var id in manager.GetCameraIdList())
                {
                    var cameraCharacteristics = manager.GetCameraCharacteristics(id);
                    if ((int)cameraCharacteristics.Get(CameraCharacteristics.LensFacing) == facing)
                    {
                        var jpegSizeList = new List<Size>();

                        if (Build.VERSION.SdkInt >= BuildVersionCodes.M &&
                            ((StreamConfigurationMap)cameraCharacteristics.Get(CameraCharacteristics
                                .ScalerStreamConfigurationMap))
                            .GetHighResolutionOutputSizes((int)ImageFormatType.Jpeg) !=
                            null)
                            jpegSizeList.AddRange(((StreamConfigurationMap)cameraCharacteristics.Get(
                                    CameraCharacteristics
                                        .ScalerStreamConfigurationMap))
                                .GetHighResolutionOutputSizes((int)ImageFormatType.Jpeg));
                        jpegSizeList.AddRange(((StreamConfigurationMap)cameraCharacteristics.Get(CameraCharacteristics
                            .ScalerStreamConfigurationMap)).GetOutputSizes((int)ImageFormatType.Jpeg));
                        _pictureSize = jpegSizeList
                            .Where(size => size.Width <= 1920)
                            .OrderByDescending(size => size.Width)
                            .First();
                    }
                }
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(SetDefaultJpegSize)})", e);
            }
        }

        private void LockAf()
        {
            try
            {
                CurrentState = CameraState.WaitAf;
                _isAfTriggered = false;

                // Set AF trigger to CaptureRequest.Builder
                PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);

                // App should send AF triggered request for only a single capture.
                _CameraSession.Capture(PreviewBuilder.Build(), new CaptureSessionCallback(this), _backgroundHandler);
                PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Idle);
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(LockAf)})", e);
            }
        }

        private void TakePicture()
        {
            if (CurrentState == CameraState.Closing)
                return;

            try
            {
                // Sets orientation
                _captureBuilder.Set(CaptureRequest.JpegOrientation, GetJpegOrientation());

                _captureBuilder.AddTarget(_imageFormat == ImageFormatType.Jpeg
                    ? _jpegReader.Surface
                    : _rawReader.Surface);

                _CameraSession.Capture(_captureBuilder.Build(),
                    new CaptureSessionCaptureCallback(this),
                    _backgroundHandler);

                _captureBuilder.RemoveTarget(_imageFormat == ImageFormatType.Jpeg
                    ? _jpegReader.Surface
                    : _rawReader.Surface);

                CurrentState = CameraState.TakePicture;
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(TakePicture)})", e);
            }
        }

        private void TriggerAe()
        {
            try
            {
                CurrentState = CameraState.WaitAe;
                _isAeTriggered = false;

                PreviewBuilder.Set(CaptureRequest.ControlAePrecaptureTrigger, (int)ControlAEPrecaptureTrigger.Start);

                // App should send AE triggered request for only a single capture.
                _CameraSession.Capture(PreviewBuilder.Build(), new CameraCaptureSessionCaptureCallback(this),
                    _backgroundHandler);
                PreviewBuilder.Set(CaptureRequest.ControlAePrecaptureTrigger, (int) ControlAEPrecaptureTrigger.Idle);
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(TriggerAe)})", e);
            }
        }

        private void UnlockAf()
        {
            // If we send TRIGGER_CANCEL. Lens move to its default position. This results in bad user experience.
            if ((int)PreviewBuilder.Get(CaptureRequest.ControlAfMode) == (int)ControlAFMode.Auto ||
                (int)PreviewBuilder.Get(CaptureRequest.ControlAfMode) == (int)ControlAFMode.Macro)
            {
                CurrentState = CameraState.Preview;
                return;
            }

            // Triggers CONTROL_AF_TRIGGER_CANCEL to return to initial AF state.
            try
            {
                PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Cancel);
                _CameraSession.Capture(PreviewBuilder.Build(), new CameraCaptureSessionCaptureCallbackForUnlockAf(this), 
                    _backgroundHandler);
                PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Idle);
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(UnlockAf)})", e);
            }
        }

        #endregion

        #region BackgroundThreading

        private void StartBackgroundThread()
        {
            _backgroundHandlerThread = new HandlerThread("Background Thread");
            _backgroundHandlerThread.Start();
            _backgroundHandler = new Handler(_backgroundHandlerThread.Looper);

            _imageSavingHandlerThread = new HandlerThread("Saving Thread");
            _imageSavingHandlerThread.Start();
            _imageSavingHandler = new Handler(_imageSavingHandlerThread.Looper);
        }

        private void StopBackgroundThread()
        {
            if (_backgroundHandlerThread != null)
            {
                _backgroundHandlerThread.QuitSafely();
                try
                {
                    _backgroundHandlerThread.Join();
                    _backgroundHandlerThread = null;
                    _backgroundHandler = null;
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }
            }

            if (_imageSavingHandlerThread != null)
            {
                _imageSavingHandlerThread.QuitSafely();
                try
                {
                    _imageSavingHandlerThread.Join();
                    _imageSavingHandlerThread = null;
                    _imageSavingHandler = null;
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        #endregion

        #region Callbacks

        private class CaptureCallbackWrapper : CameraCaptureSession.CaptureCallback
        {
            private readonly CameraController5000 _parent;

            public CaptureCallbackWrapper(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnCaptureFailed(CameraCaptureSession p0, CaptureRequest p1, CaptureFailure p2)
            {
                base.OnCaptureFailed(p0, p1, p2);
                _parent.CameraAdapter.ManualFocusEngaged = false;
            }

            public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request,
                TotalCaptureResult result)
            {
                _parent.CameraAdapter.ManualFocusEngaged = false;

                if (request.Tag?.ToString() == FocusRequestTag)
                {
                    _parent.PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, null);
                    _parent.StartPreview();
                    return;
                }

                switch (_parent.CurrentState)
                {
                    case CameraState.Idle:
                    case CameraState.TakePicture:
                    case CameraState.Closing:
                        // do nothing
                        break;
                    case CameraState.Preview:
                        if (result.Get(CaptureResult.StatisticsFaces) != null)
                        {
                            var f = result.Get(CaptureResult.StatisticsFaces).ToArray<Face>();
                            var r = (Rect)result.Get(CaptureResult.ScalerCropRegion);
                            _parent.ProcessFace(f, r);
                        }

                        if (_parent._textureReadySemaphore != null)
                        {
                            _parent._textureReadySemaphore.Release();
                            _parent._textureReadySemaphore = null;
                        }
                        break;
                    // If AF is triggered and AF_STATE indicates AF process is finished, app will trigger AE pre-capture.
                    case CameraState.WaitAf:
                        {
                            if (_parent._isAfTriggered)
                            {
                                var afState = (ControlAFState)(int)result.Get(CaptureResult.ControlAfState);
                                // Check if AF is finished.
                                if (ControlAFState.FocusedLocked == afState ||
                                    ControlAFState.NotFocusedLocked == afState)
                                {
                                    // If AE mode is off or device is legacy device then skip AE pre-capture.
                                    if ((int)result.Get(CaptureResult.ControlAeMode) !=
                                        (int) ControlAEMode.Off &&
                                        (int)_parent._characteristics.Get(
                                         CameraCharacteristics.InfoSupportedHardwareLevel) !=
                                        (int) InfoSupportedHardwareLevel.Legacy)
                                        _parent.TriggerAe();
                                    else
                                        _parent.TakePicture();
                                    _parent._isAfTriggered = false;
                                }
                            }

                            break;
                        }

                    // If AE is triggered and AE_STATE indicates AE pre-capture process is finished, app will take a picture.
                    case CameraState.WaitAe:
                        {
                            if (_parent._isAeTriggered)
                            {
                                var state = result.Get(CaptureResult.ControlAeState);
                                if (state != null)
                                {
                                    var aeState = (int)state;
                                    if (
                                        (int)ControlAEState.Converged == aeState ||
                                        (int)ControlAEState.FlashRequired == aeState ||
                                        (int)ControlAEState.Locked == aeState)
                                    {
                                        _parent.TakePicture();
                                        _parent._isAeTriggered = false;
                                    }
                                }
                            }

                            break;
                        }
                }
            }
        }

        private class OnImageAvailableListener : Object, ImageReader.IOnImageAvailableListener
        {
            private readonly CameraController5000 _parent;

            public OnImageAvailableListener(CameraController5000 parent)
            {
                _parent = parent;
            }

            public void OnImageAvailable(ImageReader reader)
            {
                if (_parent._imageFormat == ImageFormatType.Jpeg)
                    _parent._imageSaver.Save(reader.AcquireNextImage());
                else _parent._imageSaver.Save(reader.AcquireNextImage());
            }
        }


        public class CaptureSessionStateCallback : CameraCaptureSession.StateCallback
        {
            private readonly CameraController5000 _parent;

            public CaptureSessionStateCallback(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnConfigureFailed(CameraCaptureSession p0)
            {
                if (_parent.CurrentState == CameraState.Closing)
                    return;
                _parent.CurrentState = CameraState.Idle;
            }

            public override void OnConfigured(CameraCaptureSession p0)
            {
                if (_parent.CurrentState == CameraState.Closing)
                    return;
                _parent._CameraSession = p0;
                _parent.StartPreview();
            }
        }

        private class SurfaceTextureListener : Object, TextureView.ISurfaceTextureListener
        {
            private readonly CameraController5000 _parent;

            public SurfaceTextureListener(CameraController5000 parent)
            {
                _parent = parent;
            }

            public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
            {
                _parent.ConfigureTransform(width, height);
                _parent.CreatePreviewSession();
            }

            public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
            {
                return false;
            }

            public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
            {
                // SurfaceTexture size changed, we need to configure transform for TextureView, again.
                _parent.ConfigureTransform(width, height);
            }

            public void OnSurfaceTextureUpdated(SurfaceTexture surface)
            {
            }
        }

        private class CaptureSessionCallback : CameraCaptureSession.CaptureCallback
        {
            private readonly CameraController5000 _parent;

            public CaptureSessionCallback(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnCaptureCompleted(CameraCaptureSession p0, CaptureRequest p1,
                TotalCaptureResult p2)
            {
                _parent._isAfTriggered = true;
            }
        }


        private class CameraDeviceStateCallback : CameraDevice.StateCallback
        {
            private readonly CameraController5000 _parent;

            public CameraDeviceStateCallback(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnDisconnected(CameraDevice p0)
            {
                _parent._cameraOpenCloseLock.Release();
                if (_parent.CurrentState == CameraState.Closing)
                    return;
            }

            public override void OnError(CameraDevice camera, CameraError error)
            {
                _parent._cameraOpenCloseLock.Release();
                if (_parent.CurrentState == CameraState.Closing)
                    return;
            }

            public override void OnOpened(CameraDevice p0)
            {
                _parent._cameraOpenCloseLock.Release();
                if (_parent.CurrentState == CameraState.Closing)
                    return;
                _parent._CameraDevice = p0;
                _parent.CreatePreviewSession();

            }
        }

        private class OrientationEventListenerCustom : OrientationEventListener
        {
            private readonly CameraController5000 _parent;

            public OrientationEventListenerCustom(CameraController5000 parent) : base(parent._activity)
            {
                _parent = parent;
            }

            public override void OnOrientationChanged(int orientation)
            {
                if (orientation == OrientationUnknown) return;
                _parent._lastOrientation = (orientation + 45) / 90 * 90;
            }
        }


        private class CaptureSessionCaptureCallback : CameraCaptureSession.CaptureCallback
        {
            private readonly CameraController5000 _parent;

            public CaptureSessionCaptureCallback(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnCaptureCompleted(CameraCaptureSession p0, CaptureRequest p1,
                TotalCaptureResult p2)
            {
                try
                {
                    _parent._captureResultQueue.Enqueue(p2);
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }

                if (_parent.CurrentState == CameraState.Closing)
                    return;
                _parent.UnlockAf();
            }

            public override void OnCaptureFailed(CameraCaptureSession p0, CaptureRequest p1, CaptureFailure p2)
            {
                if (_parent.CurrentState == CameraState.Closing)
                    return;
                _parent.UnlockAf();
            }
        }

        private class CameraCaptureSessionCaptureCallback : CameraCaptureSession.CaptureCallback
        {
            private readonly CameraController5000 _parent;

            public CameraCaptureSessionCaptureCallback(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnCaptureCompleted(CameraCaptureSession p0, CaptureRequest p1,
                TotalCaptureResult p2)
            {
                _parent._isAeTriggered = true;
            }
        }

        private class CameraCaptureSessionCaptureCallbackForUnlockAf : CameraCaptureSession.CaptureCallback
        {
            private readonly CameraController5000 _parent;

            public CameraCaptureSessionCaptureCallbackForUnlockAf(CameraController5000 parent)
            {
                _parent = parent;
            }

            public override void OnCaptureCompleted(CameraCaptureSession p0, CaptureRequest p1,
                TotalCaptureResult p2)
            {
                if (_parent.CurrentState == CameraState.Closing)
                    return;
                _parent.CurrentState = CameraState.Preview;
            }
        }

        #endregion

        #region Video

        private void StartVideoRecording(string targetPath)
        {
            _targetVideoPath = targetPath;
            CurrentState = CameraState.RecordVideo;
            mMediaRecorder.Start();
        }

        private void PrepareMediaRecorder()
        {
            if (mMediaRecorder != null)
            {
                mMediaRecorder.Release();
                mMediaRecorder = null;
            }

            mMediaRecorder = new MediaRecorder();
            //mMediaRecorder.SetAudioSource(AudioSource.Mic);
            mMediaRecorder.SetVideoSource(VideoSource.Surface);
            mMediaRecorder.SetOutputFormat(OutputFormat.Mpeg4);

            if (!string.IsNullOrEmpty(_currentTemporaryVideoFilePath) &&
                System.IO.File.Exists(_currentTemporaryVideoFilePath))
            {
                System.IO.File.Delete(_currentTemporaryVideoFilePath);
            }

            _currentTemporaryVideoFilePath = Path.GetTempFileName();
            mMediaRecorder.SetOutputFile(_currentTemporaryVideoFilePath);

            int bitRate = 384000;
            if (_videoSize.Width * _videoSize.Height >= 1920 * 1080)
            {
                bitRate = 14000000;
            }
            else if (_videoSize.Width * _videoSize.Height >= 1280 * 720)
            {
                bitRate = 9730000;
            }
            else if (_videoSize.Width * _videoSize.Height >= 640 * 480)
            {
                bitRate = 2500000;
            }
            else if (_videoSize.Width * _videoSize.Height >= 320 * 240)
            {
                bitRate = 622000;
            }

            mMediaRecorder.SetVideoEncodingBitRate(bitRate);
            mMediaRecorder.SetVideoFrameRate(MaxPreviewFps);
            mMediaRecorder.SetVideoSize(_previewSize.Width, _previewSize.Height);
            mMediaRecorder.SetVideoEncoder(VideoEncoder.H264);
           // mMediaRecorder.SetAudioEncoder(AudioEncoder.Aac);
            mMediaRecorder.SetOrientationHint(GetJpegOrientation());
            mMediaRecorder.Prepare();
        }

        private void StopRecordingVideo()
        {
            // prevents terminated during that the operation to start.
            if ((DateTime.UtcNow - _recordingStartTime).TotalMilliseconds < 1000)
                return;

            try
            {
                _CameraSession.StopRepeating();
                _CameraSession.AbortCaptures();
            }
            catch (CameraAccessException e)
            {
                Log.Error(Tag, $"Cannot access the camera. ({nameof(StopRecordingVideo)})", e);
            }


            // Stop recording
            mMediaRecorder.Stop();
            System.IO.File.Move(_currentTemporaryVideoFilePath, _targetVideoPath);
            mMediaRecorder.Reset();
            CurrentState = CameraState.Preview;
            CreatePreviewSession();
        }

        #endregion

        private class ImageSaver
        {
            private readonly CameraController5000 _parent;

            public ImageSaver(CameraController5000 parent)
            {
                _parent = parent;
            }

            public void Save(Image image)
            {
                var buffer = image.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Remaining()];
                buffer.Get(bytes);
                try
                {

                }
                finally
                {
                    image.Close();
                    _parent._photoTakingSemaphore.Release();
                }
            }
        }
    }
}
