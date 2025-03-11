using AoLibs.Dialogs.Android;
using AoLibs.Sample.Shared.DialogViewModels;
using AoLibs.Utilities.Android;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Sample.Android.Net.Dialogs
{
    public class TestDialogA : CustomViewModelDialogBase<TestDialogViewModelA>
    {
        protected override int LayoutResourceId { get; } = Resource.Layout.test_dialog_a;

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