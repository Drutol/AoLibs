// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace AoLibs.Sample.iOS
{
    [Register ("TestDialogBViewController")]
    partial class TestDialogBViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView StaticSizeContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MessageLabel != null) {
                MessageLabel.Dispose ();
                MessageLabel = null;
            }

            if (StaticSizeContainer != null) {
                StaticSizeContainer.Dispose ();
                StaticSizeContainer = null;
            }
        }
    }
}