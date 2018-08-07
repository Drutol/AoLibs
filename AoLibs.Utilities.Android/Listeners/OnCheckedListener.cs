using System;
using Android.Widget;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnCheckedListener : Java.Lang.Object,RadioGroup.IOnCheckedChangeListener
    {
        private readonly Action<int> _callback;

        public OnCheckedListener(Action<int> callback)
        {
            _callback = callback;
        }

        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            _callback.Invoke(checkedId);
        }
    }
}