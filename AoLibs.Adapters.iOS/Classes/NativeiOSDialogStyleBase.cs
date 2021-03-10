using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    /// <summary>
    /// Utility class implementing <see cref="INativeiOSDialogStyle"/>.
    /// </summary>
    public abstract class NativeiOSDialogStyleBase : INativeiOSDialogStyle
    {
        /// <inheritdoc />
        public virtual void SetStyle(UIAlertView alertView)
        {
        }
    }
}