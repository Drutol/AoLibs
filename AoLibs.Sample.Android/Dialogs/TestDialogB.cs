using System;

using Android.Widget;
using AoLibs.Dialogs.Android;
using AoLibs.Sample.Shared.DialogViewModels;
using AoLibs.Sample.Shared.NavArgs;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Sample.Android.Dialogs
{
    public class TestDialogB : CustomArgumentViewModelDialogBase<TestDialogViewModelB, DialogBNavArgs>
    {
        protected override int LayoutResourceId { get; } = Resource.Layout.test_dialog_b;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message, () => MessageTextView.Text));
        }

        #region Views

        private TextView _messageTextView;

        public TextView MessageTextView => _messageTextView ?? (_messageTextView = FindViewById<TextView>(Resource.Id.MessageTextView));

        #endregion
    }
}