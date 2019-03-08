using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AoLibs.Camera.Android.Utils
{
    public static class MatrixUtilities
    {
        public static Matrix GetTextureMatrix(int viewWidth, int viewHeight, int previewWidth, int previewHeight)
        {
            var ratioSurface = (float)viewWidth / viewHeight;
            var ratioPreview = (float)previewWidth / previewHeight;

            float scaleX;
            float scaleY;

            if (Math.Abs(ratioSurface - ratioPreview) < .001)
                scaleY = scaleX = 1;

            else if (ratioPreview > ratioSurface)
            {
                scaleX = ((float)previewWidth / previewHeight) * viewHeight / viewWidth;
                scaleY = 1;
            }
            else
            {
                scaleX = 1;
                scaleY = ((float)previewHeight / previewWidth) * viewWidth / viewHeight;
            }

            var matrix = new Matrix();
            matrix.SetScale(scaleX, scaleY, viewWidth / 2.0f, viewHeight / 2.0f);
            return matrix;
        }
    }
}