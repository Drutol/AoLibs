using System;
using Android.Views;
using Android.Widget;

namespace AoLibs.Utilities.Android.Listeners
{
    /// <summary>
    /// Listens for item click and casts view tag into given model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnItemClickListener<T> : Java.Lang.Object , AdapterView.IOnItemClickListener where T : class
    {
        private readonly Action<(AdapterView,View,int position,long id,T model)> _callback;

        public OnItemClickListener(Action<(AdapterView, View, int position, long id, T model)> callback)
        {
            _callback = callback;
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            _callback.Invoke((parent,view,position,id,view.Tag.Unwrap<T>()));
        }
    }
}