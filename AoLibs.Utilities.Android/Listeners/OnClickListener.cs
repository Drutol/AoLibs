using System;
using Android.Views;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnClickListener : Java.Lang.Object , View.IOnClickListener
    {
        private readonly Action<View> _action;

        public OnClickListener(Action<View> action)
        {
            _action = action;
        }

        public void OnClick(View v)
        {
            _action.Invoke(v);
        }
    }
}