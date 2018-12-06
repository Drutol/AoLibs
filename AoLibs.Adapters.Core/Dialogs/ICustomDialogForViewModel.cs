using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    /// <summary>
    /// Extended dialog interface for providing result oriented methods to ViewModel.
    /// </summary>
    public interface ICustomDialogForViewModel : ICustomDialog
    {
        /// <summary>
        /// Sets the dialog result.
        /// </summary>
        /// <param name="result">Result.</param>
        void SetResult(object result);

        /// <summary>
        /// Cancels awaiting dialog's result.
        /// </summary>
        void CancelResult();
    }
}
