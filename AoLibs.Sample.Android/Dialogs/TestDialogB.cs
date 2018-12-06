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
using AoLibs.Adapters.Android.Dialogs;
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