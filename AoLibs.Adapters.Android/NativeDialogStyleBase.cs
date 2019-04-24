using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    public abstract class NativeDialogStyleBase : INativeDialogStyle
    {
        public abstract void SetStyle(AlertDialog.Builder dialog, View contentView);

        public void SetStyle(object nativeDialog, object contentView)
        {
            SetStyle(nativeDialog as AlertDialog.Builder, contentView as View);
        }
    }
}