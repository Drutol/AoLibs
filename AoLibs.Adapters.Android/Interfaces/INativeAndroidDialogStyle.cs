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

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Interface for applying additional styles to native Android dialog objects.
    /// </summary>
    public interface INativeAndroidDialogStyle : INativeDialogStyle
    {
        /// <summary>
        /// Gets theme resource that will be used to create the dialog.
        /// </summary>
        int ThemeResourceId { get; }

        /// <summary>
        /// Called when additional configuration can occur.
        /// </summary>
        /// <param name="dialogBuilder">Dialog builder.</param>
        /// <param name="contentView">Root content view contained within dialog.</param>
        void SetStyle(AlertDialog.Builder dialogBuilder, View contentView = null);

        /// <summary>
        /// Called when additional configuration can occur after creating dialog instance.
        /// </summary>
        /// <param name="dialog">Dialog builder.</param>
        void SetStyle(AlertDialog dialog);
    }
}