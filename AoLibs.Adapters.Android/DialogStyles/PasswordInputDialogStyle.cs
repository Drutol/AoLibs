using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android.DialogStyles
{
    /// <summary>
    /// Simple style for changing input type to password one.
    /// </summary>
    public class PasswordInputDialogStyle : NativeDialogStyleBase
    {
        public override void SetStyle(AlertDialog.Builder dialog, View contentView)
        {
            var child = (contentView as TextInputLayout).GetChildAt(0) as ViewGroup;
            (child.GetChildAt(0) as TextInputEditText).InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
        }
    }
}