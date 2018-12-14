using System;
using AoLibs.Dialogs.Core;
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

            vc.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            vc._childDialog = dialog;

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
    }
}