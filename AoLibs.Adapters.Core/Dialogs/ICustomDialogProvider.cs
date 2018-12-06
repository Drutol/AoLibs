using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    /// <summary>
    /// Interface used for defining dialog providers.
    /// </summary>
    public interface ICustomDialogProvider
    {
        /// <summary>
        /// Gets the dialog instance.
        /// </summary>
        ICustomDialog Dialog { get; }
    }
}
