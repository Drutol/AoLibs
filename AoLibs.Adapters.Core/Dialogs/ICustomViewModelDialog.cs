using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    /// <summary>
    /// Dialog with ViewModel
    /// </summary>
    /// <typeparam name="TViewModel">The type of ViewModel.</typeparam>
    public interface ICustomViewModelDialog<out TViewModel> : ICustomDialog
        where TViewModel : CustomDialogViewModelBase
    {
        /// <summary>
        /// Gets the ViewModel.
        /// </summary>
        TViewModel ViewModel { get; }
    }
}
