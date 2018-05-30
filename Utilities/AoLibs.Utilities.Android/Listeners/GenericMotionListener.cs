using System;
using Android.Views;

namespace AoLibs.Utilities.Android.Listeners
{
    public class GenericMotionListener : Java.Lang.Object, View.IOnGenericMotionListener
    {
        private readonly Func<View, MotionEvent, bool> _handler;

        public GenericMotionListener(Func<View,MotionEvent,bool> handler)
        {
            _handler = handler;
        }

        public bool OnGenericMotion(View v, MotionEvent e)
        {
            return _handler(v,e);
        }
    }
}