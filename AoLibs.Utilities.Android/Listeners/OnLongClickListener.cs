using System;
using Android.Views;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnLongClickListener : Java.Lang.Object , View.IOnLongClickListener
    {
        private readonly Action<View> _onLongClickAction;

        public OnLongClickListener(Action<View> onLongClickAction)
        {
            _onLongClickAction = onLongClickAction;
        }

        public bool OnLongClick(View v)
        {
            _onLongClickAction.Invoke(v);
            return true;
        }
    }
}