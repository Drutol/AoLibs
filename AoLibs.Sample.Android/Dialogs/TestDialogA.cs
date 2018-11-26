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
using AoLibs.Utilities.Android;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Sample.Android.Dialogs
{
    public class TestDialogA : CustomViewModelDialogBase<TestDialogViewModelA>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.test_dialog_a;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Counter, () => TextView.Text)
                .ConvertSourceToTarget(i => i.ToString()));

            Button.SetOnClickCommand(ViewModel.IncrementCommand);
        }

        #region Views

        private TextView _textView;
        private Button _button;

        public TextView TextView => _textView ?? (_textView = FindViewById<TextView>(Resource.Id.TextView));

        public Button Button => _button ?? (_button = FindViewById<Button>(Resource.Id.Button));

        #endregion
    }
}