using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Camera.Android.Enums;
using Java.Lang;
using Math = System.Math;

namespace AoLibs.Camera.Android
{
    public class CameraAdapter
    {
        private readonly CameraController5000 _controller;


        private bool _initialized;

        public bool SupportsManual { get; private set; }
        public bool SupportsFps { get; private set; }
        public bool SupportsCameraChange { get; set; }

        public event EventHandler<bool> RecordingStateChanged;

        public int MinIso { get; set; }
        public int MaxIso { get; set; }
        public int MinBias { get; set; }
        public int MaxBias { get; set; }
        public double MinExposureDuration { get; set; }
        public double MaxExposureDuration { get; set; }
        public int MinFps { get; set; }
        public int MaxFps { get; set; }

        public bool ManualFocusEngaged { get; set; }

        public bool IsFlashOn { get; set; }


        public CameraAdapter(CameraController5000 controller)
        {
            _controller = controller;
        }

        public void Init()
        {
            if (_initialized)
                return;

            _initialized = true;

            UpdateMaxAndMinValues();
        }

        #region Interface


        private async void UpdateMaxAndMinValues()
        {
            var characteristics = _controller.CurrentCameraCharacteristics;

            var level = (int) (Integer) characteristics.Get(CameraCharacteristics.InfoSupportedHardwareLevel);
            SupportsManual = characteristics.Get(CameraCharacteristics.SensorInfoSensitivityRange) != null &&
                             characteristics.Get(CameraCharacteristics.ControlAeCompensationRange) != null &&
                             characteristics.Get(CameraCharacteristics.SensorInfoExposureTimeRange) != null &&
                             level != (int) InfoSupportedHardwareLevel.Legacy;

            SupportsFps =
                false; //TODO characteristics.Get(SCameraCharacteristics.RequestAvailableCapabilitiesConstrainedHighSpeedVideo)

            if (!SupportsManual)
            {
                return;
            }

            var range = (Range) characteristics.Get(CameraCharacteristics.SensorInfoSensitivityRange);
            MinIso = (int) (Integer) range.Lower;
            MaxIso = (int) (Integer) range.Upper;

            range = (Range) characteristics.Get(CameraCharacteristics.ControlAeCompensationRange);
            MinBias = (int) (Integer) range.Lower;
            MaxBias = (int) (Integer) range.Upper;

            range = (Range) characteristics.Get(CameraCharacteristics.SensorInfoExposureTimeRange);
            MinExposureDuration = (long) (Long) range.Lower;
            MaxExposureDuration = (long) (Long) range.Upper;
        }


        public void TurnFlashlightOn()
        {
            _controller.PreviewBuilder.Set(CaptureRequest.FlashMode, (int) FlashMode.Torch);
            IsFlashOn = true;
            //Parent._sCameraManager.SetTorchMode(Parent._cameraId, true);
        }

        public void TurnFlashlightOff()
        {
            _controller.PreviewBuilder.Set(CaptureRequest.FlashMode, (int) FlashMode.Off);
            IsFlashOn = false;
        }


        #endregion


        public void ConfigureExposureBias(double? exposureBias = null)
        {
            if (exposureBias != null)
            {
                _controller.PreviewBuilder.Set(CaptureRequest.ControlAeExposureCompensation,
                    (Integer) (int) exposureBias.Value);
            }
            else
            {
                _controller.PreviewBuilder.Set(CaptureRequest.ControlAeExposureCompensation, null);
            }

            _controller.StartPreview();
        }


        public void ConfigureFocus(double? focusPosition = null)
        {
            var characteristic = _controller.CurrentCameraCharacteristics;
            var facing = (int) (Integer) characteristic.Get(CameraCharacteristics.LensFacing);

            //front camera doesn't support setting focus
            if (facing == (int) LensFacing.Front)
                return;

            if (focusPosition == null)
            {
                _controller.PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int) ControlAFMode.Auto);
            }
            else
            {
                _controller.PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, null);
                _controller.PreviewBuilder.Set(CaptureRequest.LensFocusDistance, (Float) (float) focusPosition);
            }
        }

        public void SetFocus(View view, MotionEvent e)
        {
            if (_controller.CurrentState != CameraState.Preview)
                return;

            var actionMasked = e.ActionMasked;
            if (actionMasked != MotionEventActions.Down)
                return;

            if (ManualFocusEngaged)
                return;


            var characteristics = _controller.CurrentCameraCharacteristics;

            var sensorArraySize = (Rect) characteristics.Get(CameraCharacteristics.SensorInfoActiveArraySize);

            var y = (int) ((e.GetX() / view.Width) * sensorArraySize.Height());
            var x = (int) ((e.GetY() / view.Height) * sensorArraySize.Width());
            var halfTouchWidth = 150; //(int)e.getTouchMajor();
            var halfTouchHeight = 150; //(int)e.getTouchMinor();
            var focusAreaTouch = new MeteringRectangle(Math.Max(x - halfTouchWidth, 0),
                Math.Max(y - halfTouchHeight, 0),
                halfTouchWidth * 2,
                halfTouchHeight * 2,
                MeteringRectangle.MeteringWeightMax - 1);

            _controller.CameraSession.StopRepeating();
            //first stop the existing repeating request

            //cancel any existing AF trigger (repeated touches, etc.)
            _controller.PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int) ControlAFTrigger.Cancel);
            _controller.PreviewBuilder.Set(CaptureRequest.ControlAfMode, (int) ControlAFMode.Off);
            _controller.CameraSession.Capture(_controller.PreviewBuilder.Build(), _controller.SessionCaptureCallback,
                _controller.BackgroundHandler);

            //Now add a new AF trigger with focus region
            if (((int) (Integer) characteristics.Get(CameraCharacteristics.ControlMaxRegionsAf)) >= 1)
            {
                _controller.PreviewBuilder.Set(CaptureRequest.ControlAfRegions,
                    new MeteringRectangle[] {focusAreaTouch});
            }

            _controller.PreviewBuilder.Set(CaptureRequest.ControlMode, (int) ControlMode.Auto);
            _controller.PreviewBuilder.Set(CaptureRequest.ControlAfMode, (int) ControlAFMode.Auto);
            _controller.PreviewBuilder.Set(CaptureRequest.ControlAfTrigger, (int) ControlAFTrigger.Start);
            _controller.PreviewBuilder.SetTag(CameraController5000
                .FocusRequestTag); //we'll capture this later for resuming the preview

            //then we ask for a single request (not repeating!)
            _controller.CameraSession.Capture(_controller.PreviewBuilder.Build(), _controller.SessionCaptureCallback,
                _controller.BackgroundHandler);
            ManualFocusEngaged = true;
        }


        public void ConfigureWhiteBalance(double? whiteBalanceTemperature, double? whiteBalanceTintFocus,
            bool isWBTempDifferenceFromAuto)
        {
            //if (whiteBalanceTemperature != null || whiteBalanceTintFocus != null)
            //{
            //    _controller.PreviewBuilder.Set(CaptureRequest.ControlAwbMode, SCameraMetadata.ControlAwbModeOff);
            //    _controller.PreviewBuilder.Set(CaptureRequest.ColorCorrectionMode, SCameraMetadata.ColorCorrectionModeTransformMatrix);
            //    _controller.PreviewBuilder.Set(CaptureRequest.ColorCorrectionGains, ColorTemperature((int)whiteBalanceTemperature.Value));
            //}
            //else
            //{
            //    _controller.PreviewBuilder.Set(CaptureRequest.ControlAwbMode, SCameraMetadata.ControlAwbModeAuto);
            //    _controller.PreviewBuilder.Set(CaptureRequest.ColorCorrectionMode, SCameraMetadata.ColorCorrectionModeHighQuality);

            //}

            //Parent.StartPreview();
        }

        private RggbChannelVector ColorTemperature(float whiteBalance)
        {
            whiteBalance = (System.Math.Min(whiteBalance, 38000) * 100f) / 50000f;
            var temperature = 100 - whiteBalance;
            float red;
            float green;
            float blue;

            //Calculate red
            if (temperature <= 66)
                red = 255;
            else
            {
                red = temperature - 60;
                red = (float) (329.698727446 * (Java.Lang.Math.Pow((double) red, -0.1332047592)));
                if (red < 0)
                    red = 0;
                if (red > 255)
                    red = 255;
            }


            //Calculate green
            if (temperature <= 66)
            {
                green = temperature;
                green = (float) (99.4708025861 * Java.Lang.Math.Log(green) - 161.1195681661);
                if (green < 0)
                    green = 0;
                if (green > 255)
                    green = 255;
            }
            else
            {
                green = temperature - 60;
                green = (float) (288.1221695283 * (Java.Lang.Math.Pow((double) green, -0.0755148492)));
                if (green < 0)
                    green = 0;
                if (green > 255)
                    green = 255;
            }

            //calculate blue
            if (temperature >= 66)
                blue = 255;
            else if (temperature <= 19)
                blue = 0;
            else
            {
                blue = temperature - 10;
                blue = (float) (138.5177312231 * Java.Lang.Math.Log(blue) - 305.0447927307);
                if (blue < 0)
                    blue = 0;
                if (blue > 255)
                    blue = 255;
            }

            return new RggbChannelVector((red / 255) * 2, (green / 255), (green / 255), (blue / 255) * 2);
        }

        public void ConfigureExposureAndISO(double? newDuration, double? minISO, double? maxISO)
        {
            float currentISO = (int) (Integer) _controller.PreviewBuilder.Get(CaptureRequest.SensorSensitivity);
            var currentExposureInSeconds =
                (long) (Long) _controller.PreviewBuilder.Get(CaptureRequest.SensorExposureTime) / 10_000_000_00;

            if (currentExposureInSeconds == 0)
                currentExposureInSeconds = 30;

            if (newDuration == null && minISO != null)
                newDuration = 1 / 30;


            if (maxISO != null && currentISO > maxISO)
            {
                currentISO = (float) maxISO;

                if (newDuration == null && currentExposureInSeconds != 0)
                    newDuration = 1 / currentExposureInSeconds;
            }

            if (minISO != null && currentISO < minISO)
            {
                currentISO = (float) minISO;

                if (newDuration == null && currentExposureInSeconds != 0)
                    newDuration = 1 / currentExposureInSeconds;
            }

            currentISO = currentISO < MinIso ? MinIso + 1 : currentISO;
            currentISO = currentISO > MaxIso ? MaxIso - 1 : currentISO;

            if (newDuration == null && maxISO == null && minISO == null)
            {
                _controller.PreviewBuilder.Set(CaptureRequest.ControlAeMode, (int) ControlAEMode.On);
            }
            else
            {
                newDuration = 1.0d / newDuration;
                newDuration = newDuration.Value * 10_000_000_00;

                newDuration = newDuration < MinExposureDuration ? MinExposureDuration : newDuration;
                newDuration = newDuration > MaxExposureDuration ? MaxExposureDuration : newDuration;

                _controller.PreviewBuilder.Set(CaptureRequest.ControlAeMode, (int) ControlAEMode.Off);
                _controller.PreviewBuilder.Set(CaptureRequest.SensorExposureTime, (Long) (long) newDuration.Value);
                _controller.PreviewBuilder.Set(CaptureRequest.SensorSensitivity, (Integer) (int) currentISO);


            }

            _controller.StartPreview();
        }

        [Preserve]
        private void IncludeArr()
        {
            var arr = new JavaArray<Range>(IntPtr.Zero, JniHandleOwnership.DoNotRegister);
        }
    }
}