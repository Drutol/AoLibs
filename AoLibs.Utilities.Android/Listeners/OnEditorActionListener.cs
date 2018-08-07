using System;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnEditorActionListener : Java.Lang.Object, TextView.IOnEditorActionListener
    {
        private readonly Action<(TextView,ImeAction,KeyEvent)> _action;

        public OnEditorActionListener(Action<(TextView, ImeAction, KeyEvent)> action)
        {
            _action = action;
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            _action.Invoke((v,actionId,e));
            return false;
        }
    }
}