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
    [Register ("TestDialogAViewController")]
    partial class TestDialogAViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CounterLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DoStuffButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CounterLabel != null) {
                CounterLabel.Dispose ();
                CounterLabel = null;
            }

            if (DoStuffButton != null) {
                DoStuffButton.Dispose ();
                DoStuffButton = null;
            }
        }
    }
}