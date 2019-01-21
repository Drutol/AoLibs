using System;
using AoLibs.Dialogs.iOS;
using AoLibs.Dialogs.iOS.Models;
using AoLibs.Sample.iOS.Utils;
using AoLibs.Sample.Shared.DialogViewModels;
using AoLibs.Sample.Shared.Models;
using AoLibs.Utilities.iOS.Extensions;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [CustomDialog((int)DialogIndex.TestDialogA, Constants.MainDialogsStoryboard, nameof(TestDialogAViewController))]
    public partial class TestDialogAViewController : CustomViewModelDialogBase<TestDialogViewModelA>
    {
        public TestDialogAViewController (IntPtr handle) : base (handle)
        {
        }

        protected TestDialogAViewController(string name, NSBundle p) : base(name, p)
        {  
        }

        public override DialogBackgroundConfig BackgroundConfig => new DialogBackgroundConfig() { BlurStyle = UIBlurEffectStyle.Dark };

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Counter, () => CounterLabel.Text));
            DoStuffButton.SetOnClickCommand(ViewModel.IncrementCommand);
        }
    }
}
