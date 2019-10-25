using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Android.Interfaces;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Specialized dialog style interface specifying whether to use default or custom implementation.
    /// </summary>
    public interface INativeAndroidLoadingDialogStyle : INativeAndroidDialogStyle
    {
        /// <summary>
        /// Gets a value indicating whether to call event or bring default loading dialog.
        /// </summary>
        bool UseDefault { get; }
    }
}
