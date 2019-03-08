using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AoLibs.Camera.Android.Utils;

namespace AoLibs.Camera.Android.Views
{
    public class AutoFitTextureView : TextureView
    {
        public enum AutoFitMethod
        {
            AdjustViewHeight,
            AdjustViewHeightOrWidth,
            PreserveDimensionsAndApplyMatrix
        }

        private Size _previewSize;
        private int _ratioWidth;
        private int _ratioHeight;


        public Size PreviewSize
        {
            get => _previewSize;
            set
            {
                _previewSize = value;
                RequestLayout();
            }
        }

        public AutoFitMethod FitMethod { get; set; }
      
        protected AutoFitTextureView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public AutoFitTextureView(Context context) : base(context)
        {
            Init();
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        private void Init()
        {

        }

        public void SetAspectRatio(int width, int height)
        {
            _ratioWidth = width;
            _ratioHeight = height;
            RequestLayout();
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            if (FitMethod == AutoFitMethod.PreserveDimensionsAndApplyMatrix)
            {
                int width = MeasureSpec.GetSize(widthMeasureSpec);
                if (PreviewSize != null)
                {
                    float ratio;
                    if (PreviewSize.Height >= PreviewSize.Width)
                        ratio = (float) PreviewSize.Height / PreviewSize.Width;
                    else
                        ratio = (float) PreviewSize.Width / PreviewSize.Height;

                    var newHeight = (int) (width * ratio);
                    SetMeasuredDimension(width, newHeight);
                }
            }
            else if (FitMethod == AutoFitMethod.AdjustViewHeightOrWidth)
            {
                var width = MeasureSpec.GetSize(widthMeasureSpec);
                var height = MeasureSpec.GetSize(heightMeasureSpec);
                if (0 == _ratioWidth || 0 == _ratioHeight)
                {
                    SetMeasuredDimension(width, height);
                }
                else
                {
                    if (width < height * _ratioWidth / _ratioHeight)
                    {
                        SetMeasuredDimension(width, width * _ratioHeight / _ratioWidth);
                    }
                    else
                    {
                        SetMeasuredDimension(height * _ratioWidth / _ratioHeight, height);
                    }
                }

            }
            else
            {
                SetTransform(
                    MatrixUtilities.GetTextureMatrix(
                        MeasuredWidth,
                        MeasuredHeight,
                        _previewSize.Width,
                        _previewSize.Height));
            }
        }
    }
}