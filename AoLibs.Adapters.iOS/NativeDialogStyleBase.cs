using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    public abstract class NativeDialogStyleBase : INativeDialogStyle
    {
        public abstract void SetStyle(UIAlertView alertView); 

        public void SetStyle(object nativeDialog, object contentView)
        {
            SetStyle(nativeDialog as UIAlertView);
        }
    }
}