using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using Google.Android.Material.TextField;

namespace AoLibs.Adapters.Android.DialogStyles
{
    /// <summary>
    /// Simple style for changing input type to password one.
    /// </summary>
    public class PasswordInputDialogStyle : NativeDialogStyleBase
    {
        public override void SetStyle(AlertDialog.Builder dialogBuilder, View contentView = null)
        {
            var child = (contentView as TextInputLayout).GetChildAt(0) as ViewGroup;

            for (var i = 0; i < child.ChildCount; i++)
            {
                var subChild = (child.GetChildAt(i) as TextInputEditText);

                if (subChild != null)
                {
                    subChild.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
                    return;
                }
            }
        }
    }
}