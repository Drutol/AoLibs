using Foundation;
using System;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    public partial class DialogViewController : UIViewController
    {
        public static DialogViewController Instatiate(CustomDialogBase dialog)
        {
            var vc = (DialogViewController)UIStoryboard.FromName("DialogContainer", null).InstantiateViewController("DialogViewController");
            vc.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            vc._dialog = dialog;
            return vc;
        }

        private CustomDialogBase _dialog;

        public DialogViewController (IntPtr handle) 
            : base (handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            AddChildViewController(_dialog);
            _dialog.View.TranslatesAutoresizingMaskIntoConstraints = false;
            ContainerView.AddSubview(_dialog.View);

            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[]
            {
                _dialog.View.LeadingAnchor.ConstraintEqualTo(ContainerView.LeadingAnchor),
                _dialog.View.TrailingAnchor.ConstraintEqualTo(ContainerView.TrailingAnchor),
                _dialog.View.TopAnchor.ConstraintEqualTo(ContainerView.TopAnchor),
                _dialog.View.BottomAnchor.ConstraintEqualTo(ContainerView.BottomAnchor),
            });

            _dialog.DidMoveToParentViewController(this);
        }
    }
}