using System;
using Android.Widget;

namespace AoLibs.Utilities.Android.Listeners
{
    /// <summary>
    /// Listens for date changes, returned month is offseted by 1 to mach .NET convention.
    /// </summary>
    public class DateSetListener : Java.Lang.Object, global::Android.App.DatePickerDialog.IOnDateSetListener
    {
        private Action<(int year,int monthOfYear,int dayOfMonth)> _callback;

        public DateSetListener(Action<(int year, int monthOfYear, int dayOfMonth)> callback)
        {
            _callback = callback;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            _callback.Invoke((year, monthOfYear+1, dayOfMonth));
        }
    }
}