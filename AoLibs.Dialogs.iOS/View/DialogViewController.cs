using System;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.iOS.Models;
using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    /// <summary>
    /// ViewController that wraps injected dialog.
    /// </summary>
    public partial class DialogViewController : UIViewController
    {
        private const string StoryboardName = "DialogContainer";
        private const string ControllerName = "DialogViewController";

        /// <summary>
        /// Method that creates the instance of <see cref="DialogViewController"/> with <see cref="CustomDialogBase"/> as the dialog to inject.
        /// </summary>
        /// <param name="dialog">Dialog to inject.</param>
        public static DialogViewController Instantiate(CustomDialogBase dialog)
        {
            var vc = (DialogViewController) UIStoryboard.FromName(StoryboardName, null)
                .InstantiateViewController(ControllerName);

            vc._childDialog = dialog;
            vc.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;

            vc._childDialog.DialogWillHide+= vc.Dialog_DialogWillHide;

            return vc;
        }

        /// <summary>
        /// Called when user taps outside defined container.
        /// </summary>
        public event EventHandler TappedOutsideTheDialog;

        private CustomDialogBase _childDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public DialogViewController (IntPtr handle) 
            : base (handle)
        {
        }

        /// <inheritdoc />
        public override void ViewWillAppear(bool animated)
        {
            SetupBackground();
            PrepareCustomAnimation();

            AddChildViewController(_childDialog);
            _childDialog.View.TranslatesAutoresizingMaskIntoConstraints = false;
            ContainerView.AddSubview(_childDialog.View);
            ContainerView.UserInteractionEnabled = true;
            ContainerView.AddGestureRecognizer(new UITapGestureRecognizer(() => { }));

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _childDialog.View.LeadingAnchor.ConstraintEqualTo(ContainerView.LeadingAnchor),
                _childDialog.View.TrailingAnchor.ConstraintEqualTo(ContainerView.TrailingAnchor),
                _childDialog.View.TopAnchor.ConstraintEqualTo(ContainerView.TopAnchor),
                _childDialog.View.BottomAnchor.ConstraintEqualTo(ContainerView.BottomAnchor),
            });

            _childDialog.DidMoveToParentViewController(this);

            var gestureRecognizer =
                new UITapGestureRecognizer(() => TappedOutsideTheDialog?.Invoke(this, EventArgs.Empty));
            RootView.AddGestureRecognizer(gestureRecognizer);
        }

        /// <inheritdoc />
        public override void ViewDidAppear(bool animated)
        {
            if (_childDialog.AnimationConfig.ShowAnimationType == DialogAnimationType.CustomBlurFade &&
                _childDialog.BackgroundConfig.BlurEnabled)
            {
                UIView.Animate(_childDialog.AnimationConfig.ShowCustomAnimationDurationSeconds, () =>
                {
                    _childDialog.View.Alpha = 1f;
                    EffectView.Effect = UIBlurEffect.FromStyle(_childDialog.BackgroundConfig.BlurStyle);
                });
            }
        }

        private void Dialog_DialogWillHide(object sender, EventArgs e)
        {
            if (_childDialog.AnimationConfig.HideAnimationType == DialogAnimationType.CustomBlurFade)
            {
                EffectView.Effect = UIBlurEffect.FromStyle(_childDialog.BackgroundConfig.BlurStyle);
                UIView.Animate(_childDialog.AnimationConfig.ShowCustomAnimationDurationSeconds, () =>
                {
                    _childDialog.View.Alpha = 0f;
                    EffectView.Effect = null;
                });
            }
        }

        private void SetupBackground()
        {
            RootView.BackgroundColor = _childDialog.BackgroundConfig.Color;

            if (_childDialog.BackgroundConfig.BlurEnabled)
                EffectView.Effect = UIBlurEffect.FromStyle(_childDialog.BackgroundConfig.BlurStyle);
            else
                EffectView.Effect = null;
        }

        private void PrepareCustomAnimation()
        {
            if (_childDialog.AnimationConfig.ShowAnimationType == DialogAnimationType.CustomBlurFade)
            {
                _childDialog.View.Alpha = 0f;
                EffectView.Effect = null;
            }
        }
    }
}
