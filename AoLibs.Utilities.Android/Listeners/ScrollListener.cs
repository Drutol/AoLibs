using System;
using Android.Views;
using Android.Widget;

namespace AoLibs.Utilities.Android.Listeners
{
    public class ScrollListener : Java.Lang.Object, AbsListView.IOnScrollChangeListener
    {
        private readonly Action<(View, int scrollX, int scrollY, int oldScrollX, int oldScrollY)> _callback;

        public ScrollListener(Action<(View, int scrollX, int scrollY, int oldScrollX, int oldScrollY)> callback)
        {
            _callback = callback;
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            _callback.Invoke((v, scrollX, scrollY, oldScrollX, oldScrollY));
        }
    }
}