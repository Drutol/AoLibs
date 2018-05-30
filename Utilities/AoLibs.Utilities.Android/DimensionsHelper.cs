using Android.App;

namespace AoLibs.Utilities.Android
{
    public static class DimensionsHelper
    {
        public static int DpToPx(float dp)
        {
            return (int)(dp * (Application.Context.Resources.DisplayMetrics.Density));
        }

        public static float PxToDp(int px)
        {
            return px / Application.Context.Resources.DisplayMetrics.Density;
        }
    }
}