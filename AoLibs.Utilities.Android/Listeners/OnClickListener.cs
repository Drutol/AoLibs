using System;
using Android.Views;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnClickListener : Java.Lang.Object , View.IOnClickListener
    {
        private readonly Action<View> _action;
        private readonly Action _actionBasic;

        public OnClickListener(Action<View> action)
        {
            _action = action;
        }

        public OnClickListener(Action action)
        {
            _actionBasic = action;
        }

        public void OnClick(View v)
        {
            if (_action != null)
                _action.Invoke(v);
            else
                _actionBasic.Invoke();
        }
    }
}