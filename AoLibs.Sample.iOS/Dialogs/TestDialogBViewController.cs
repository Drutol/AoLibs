using System;
using AoLibs.Dialogs.iOS;
using AoLibs.Dialogs.iOS.Models;
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
        public TestDialogBViewController(IntPtr handle) : base(handle)
        {
        }

        public override DialogBackgroundConfig BackgroundConfig => new DialogBackgroundConfig() { BlurStyle = UIBlurEffectStyle.ExtraDark };

        public override DialogAnimationConfig AnimationConfig 
        {
            get
            {
                var config = base.AnimationConfig;
                config.ShowAnimationType = DialogAnimationType.CustomBlurFade;
                config.HideAnimationType = DialogAnimationType.SystemFlipHorizontal;
                config.ShowCustomAnimationDurationSeconds = 1f;
                return config;
            }
        }

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message, () => MessageLabel.Text));
        }
    }
}