using Foundation;
using System;
using AoLibs.Dialogs.iOS;
using AoLibs.Sample.iOS.Utils;
using AoLibs.Sample.Shared.DialogViewModels;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [CustomDialog((int)DialogIndex.TestDialogB, Constants.MainDialogsStoryboard, nameof(TestDialogBViewController))]
    public partial class TestDialogBViewController : CustomArgumentViewModelDialogBase<TestDialogViewModelB, DialogBNavArgs>
    {
        public TestDialogBViewController (IntPtr handle) : base (handle)
        {
        }

        public override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message, () => MessageLabel.Text));
        }
    }
}