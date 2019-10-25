using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AoLibs.Adapters.Core.Interfaces;
using UIKit;

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Interface for applying additional styles to native iOS dialog objects.
    /// </summary>
    public interface INativeiOSDialogStyle : INativeDialogStyle
    {
        /// <summary>
        /// Called when additional configuration can occur.
        /// </summary>
        /// <param name="alertView">Dialog builder.</param>
        void SetStyle(UIAlertView alertView);
    }
}