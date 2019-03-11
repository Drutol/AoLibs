using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Util;
using Android.Views;
using AoLibs.Camera.Android.Utils;
using Java.Lang;

namespace AoLibs.Camera.Android.Views
{
    public class FaceRectView : View
    {
        private RectF _actualRect;
        private Matrix _aspectRatio;
        private RectF _boundRect;
        private Face[] _mFaces;
        private int _mFacing;

        private Matrix _matrix;
        private int _mOrientation;

        private Paint _paint;
        private Size _mPreviewSize;
        private int _mRatioHeight;
        private int _mRatioWidth;

        private RectF _revisionZoomRect;

        private int _mRotation;
        private Rect _mZoomRect;

        public Orientation RealOrientation => SimpleOrientationListener.Instance.CurrentOrientation;

        public FaceRectView(Context context) : base(context)
        {
            Init();
        }

        public FaceRectView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            Init();
        }

        public FaceRectView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init();
        }

        void Init()
        {
            _paint = new Paint();
            _paint.SetStyle(Paint.Style.Stroke);

            _matrix = new Matrix();
            _aspectRatio = new Matrix();
            _revisionZoomRect = new RectF();
            _actualRect = new RectF();
            _boundRect = new RectF();

        }

        public void SetAspectRatio(int width, int height)
        {
            if (width < 0 || height < 0) throw new IllegalArgumentException("Size cannot be negative.");
            _mRatioWidth = width;
            _mRatioHeight = height;
            RequestLayout();
        }

        public void SetFaceRect(Face[] faces, Rect zoomRect)
        {
            _mFaces = faces;
            _mZoomRect = zoomRect;
        }

        public void SetTransform(Size previewSize, int facing, int rotation, int orientation)
        {
            _mFacing = facing;
            _mPreviewSize = previewSize;
            _mRotation = rotation;
            _mOrientation = orientation;
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (_mFaces != null && _mZoomRect != null)
            {
                // Prepare matrix
                _matrix.Reset();
                _aspectRatio.Reset();
                _actualRect.Set(0, 0, _mPreviewSize.Width, _mPreviewSize.Height);

                // First apply zoom (crop) rect.
                // Unlike the documentation, many device does not report final crop region.
                // So, here we calculate final crop region which takes account aspect ratio between crop region and preview size.
                {
                    _revisionZoomRect.Set(_mZoomRect);
                    var left = _revisionZoomRect.Left;
                    var top = _revisionZoomRect.Top;

                    _revisionZoomRect.OffsetTo(0, 0);

                    _aspectRatio.SetRectToRect(_actualRect, _revisionZoomRect, Matrix.ScaleToFit.Center);

                    _aspectRatio.MapRect(_actualRect);
                    _actualRect.Offset(left, top);
                }

                _matrix.PostTranslate(-_actualRect.CenterX(), -_actualRect.CenterY());

                // compensate mirror
                _matrix.PostScale(_mFacing == (int)LensFacing.Front ? -1 : 1, 1);

                // Then rotate and scale to UI size
                _matrix.PostRotate(_mRotation);
                if (_mOrientation == (int)Orientation.Landscape)
                    _matrix.PostScale(Width / _actualRect.Width(), Height / _actualRect.Height());
                else
                    _matrix.PostScale(Height / _actualRect.Width(), Width / _actualRect.Height());
                _matrix.PostTranslate((float)Width / 2, (float)Height / 2);

                foreach (var face in _mFaces)
                {
                    _boundRect.Set(face.Bounds);
                    _matrix.MapRect(_boundRect);

                    _paint.Color = Color.Blue;
                    _paint.StrokeWidth = 3;
                    canvas.DrawRect(_boundRect, _paint);
                }

                var f = _mFaces.FirstOrDefault()?.Bounds;

                if (f != null)
                {
                    var mappedFace = new RectF();
                    mappedFace.Set(f);
                    _matrix.MapRect(mappedFace);
                }       
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            var height = MeasureSpec.GetSize(heightMeasureSpec);
            if (0 == _mRatioWidth || 0 == _mRatioHeight)
            {
                SetMeasuredDimension(width, height);
            }
            else
            {
                if (width < height * _mRatioWidth / _mRatioHeight)
                    SetMeasuredDimension(width, width * _mRatioHeight / _mRatioWidth);
                else
                    SetMeasuredDimension(height * _mRatioWidth / _mRatioHeight, height);
            }
        }
    }
}
