using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Interface for applying additional styles to native dialog objects.
    /// </summary>
    public interface INativeDialogStyle
    {
        /// <summary>
        /// Called when additional configuration can occur.
        /// </summary>
        /// <param name="nativeDialog">Dialog object.</param>
        /// <param name="contentView">Root content view contained within dialog.</param>
        void SetStyle(object nativeDialog, object contentView = null);
    }
}
