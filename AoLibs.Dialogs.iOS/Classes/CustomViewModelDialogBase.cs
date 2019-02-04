using System;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using Foundation;

namespace AoLibs.Dialogs.iOS
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
        public TViewModel ViewModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomViewModelDialogBase{TViewModel}"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected CustomViewModelDialogBase(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomViewModelDialogBase{TViewModel}"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="p">Bundle.</param>
        protected CustomViewModelDialogBase(string name, NSBundle p)
            : base(name, p)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomViewModelDialogBase{TViewModel}"/> class.
        /// </summary>
        private void Initialize()
        {
            ViewModel = CustomDialogViewModelResolver?.Resolve<TViewModel>();

            if (ViewModel != null)
            {
                ViewModel.Dialog = this;
                CustomDialogConfig = ViewModel.CustomDialogConfig;
            }
        }

        /// <inheritdoc />
        protected override void OnWillBeShown()
        {
            base.OnWillBeShown();
            ViewModel.OnDialogAppearedInternal();
        }

        /// <inheritdoc />
        protected override void OnWillBeHidden()
        {
            base.OnWillBeHidden();
            ViewModel.OnDialogDismissedInternal();
        }
    }
}