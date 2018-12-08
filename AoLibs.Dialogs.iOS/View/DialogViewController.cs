using System;
using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    public partial class DialogViewController : UIViewController
    {
        private const string StoryboardName = "DialogContainer";
        private const string ControllerName = "DialogViewController";

        public static DialogViewController Instantiate(CustomDialogBase dialog)
        {
            var vc = (DialogViewController) UIStoryboard.FromName(StoryboardName, null)
                .InstantiateViewController(ControllerName);

            vc.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            vc._childDialog = dialog;

            return vc;
        }

        private CustomDialogBase _childDialog;

        public DialogViewController (IntPtr handle) 
            : base (handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            AddChildViewController(_childDialog);
            _childDialog.View.TranslatesAutoresizingMaskIntoConstraints = false;
            ContainerView.AddSubview(_childDialog.View);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _childDialog.View.LeadingAnchor.ConstraintEqualTo(ContainerView.LeadingAnchor),
                _childDialog.View.TrailingAnchor.ConstraintEqualTo(ContainerView.TrailingAnchor),
                _childDialog.View.TopAnchor.ConstraintEqualTo(ContainerView.TopAnchor),
                _childDialog.View.BottomAnchor.ConstraintEqualTo(ContainerView.BottomAnchor),
            });

            _childDialog.DidMoveToParentViewController(this);
        }
    }
}