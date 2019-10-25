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
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Utility class implementing <see cref="INativeAndroidDialogStyle"/>
    /// </summary>
    public abstract class NativeDialogStyleBase : INativeAndroidDialogStyle
    {
        /// <inheritdoc/>
        public virtual int ThemeResourceId { get; }

        /// <inheritdoc/>
        public virtual void SetStyle(AlertDialog.Builder dialogBuilder, View contentView = null)
        {
        }

        /// <inheritdoc/>
        public virtual void SetStyle(AlertDialog dialog)
        {
        }
    }
}