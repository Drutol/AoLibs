using Foundation;
using System;
using AoLibs.Dialogs.iOS;
using AoLibs.Sample.iOS.Utils;
using AoLibs.Sample.Shared.DialogViewModels;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using GalaSoft.MvvmLight.Helpers;
using UIKit;
using AoLibs.Dialogs.iOS.Models;

namespace AoLibs.Sample.iOS
{
    [CustomDialog((int)DialogIndex.TestDialogB, Constants.MainDialogsStoryboard, nameof(TestDialogBViewController))]
    public partial class TestDialogBViewController : CustomArgumentViewModelDialogBase<TestDialogViewModelB, DialogBNavArgs>
    {
        public TestDialogBViewController (IntPtr handle) : base (handle)
        {
        }

        public override DialogBackgroundConfig BackgroundConfig => new DialogBackgroundConfig() { BlurStyle = UIBlurEffectStyle.ExtraLight, Color = UIColor.Orange.ColorWithAlpha(0.3f) };

        public override DialogAnimationConfig AnimationConfig => new DialogAnimationConfig();

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message, () => MessageLabel.Text));
        }
    }
}