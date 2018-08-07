using Android.Views;

namespace AoLibs.Utilities.Android
{
    public static class Converters
    {
        public static ViewStates BoolToVisibility(bool arg)
        {
            return arg ? ViewStates.Visible : ViewStates.Gone;
        }

        public static ViewStates IsStringEmptyToVisibility(string arg)
        {
            return string.IsNullOrEmpty(arg) ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}