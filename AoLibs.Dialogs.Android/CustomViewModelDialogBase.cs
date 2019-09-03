using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;

namespace AoLibs.Dialogs.Android
{
    /// <summary>
    /// Dialog base with possibility of providing the type of ViewModel associated with given dialog.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel associated with this dialog.</typeparam>
    public abstract class CustomViewModelDialogBase<TViewModel> 
        : CustomDialogBase, ICustomViewModelDialog<TViewModel>
        where TViewModel : CustomDialogViewModelBase
    {
        /// <summary>
        /// Gets the ViewModel of this dialog.
        /// </summary>
        public TViewModel ViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomViewModelDialogBase{TViewModel}"/> class.
        /// </summary>
        protected CustomViewModelDialogBase()
        {
            ViewModel = CustomDialogDependencyResolver?.Resolve<TViewModel>();

            if (ViewModel != null)
            {
                ViewModel.Dialog = this;
                CustomDialogConfig = ViewModel.CustomDialogConfig;
            }
        }

        /// <inheritdoc />
        protected override void OnShown()
        {
            ViewModel.OnDialogAppearedInternal();
            base.OnShown();
        }

        /// <inheritdoc />
        protected override void OnHidden()
        {
            ViewModel.OnDialogDismissedInternal();
            base.OnHidden();
        }
    }
}