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
using AoLibs.Adapters.Core.Dialogs;

namespace AoLibs.Adapters.Android.Dialogs
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
            ViewModel = CustomDialogViewModelResolver?.Resolve<TViewModel>();

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
        protected override void OnDismissed()
        {
            ViewModel.OnDialogDismissedInternal();
            base.OnDismissed();
        }
    }
}