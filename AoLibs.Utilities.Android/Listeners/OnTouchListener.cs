using System;
using Android.Views;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private readonly Action<MotionEvent> _action;

        public OnTouchListener(Action<MotionEvent> action)
        {
            _action = action;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            _action.Invoke(e);
            return true;
        }
    }
}
