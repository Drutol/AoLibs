using System.Runtime.CompilerServices;
using AoLibs.Dialogs.Core.Interfaces;
using GalaSoft.MvvmLight;

[assembly: InternalsVisibleTo("AoLibs.Dialogs.Android")]
namespace AoLibs.Dialogs.Core
{
    /// <summary>
    /// Base ViewModel to be used for dialogs.
    /// </summary>
    public abstract class CustomDialogViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Gets the config used for dialog definition.
        /// </summary>
        public virtual CustomDialogConfig CustomDialogConfig { get; }

        /// <summary>
        /// Gets or sets instance associated with this ViewModel.
        /// </summary>
        public ICustomDialogForViewModel Dialog { get; set; }

        /// <summary>
        /// Internal callback for when the dialog appears.
        /// </summary>
        protected internal virtual void OnDialogAppearedInternal()
        {
            OnDialogAppeared();
        }

        /// <summary>
        /// Internal callback for when the dialog is dismissed.
        /// </summary>
        protected internal void OnDialogDismissedInternal()
        {
            OnDialogDismissed();
        }

        /// <summary>
        /// Callback for when the dialog appears.
        /// </summary>
        protected virtual void OnDialogAppeared()
        {
        }

        /// <summary>
        /// Callback for when the dialog is dismissed.
        /// </summary>
        protected virtual void OnDialogDismissed()
        {
        }
    }
}
