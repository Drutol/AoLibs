// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AoLibs.Sample.iOS.ViewControllers
{
    [Register ("TestPageBViewController")]
    partial class TestPageBViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GoBackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NavigateCNoBackstack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NavigateMore { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (GoBackButton != null) {
                GoBackButton.Dispose ();
                GoBackButton = null;
            }

            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }

            if (NavigateCNoBackstack != null) {
                NavigateCNoBackstack.Dispose ();
                NavigateCNoBackstack = null;
            }

            if (NavigateMore != null) {
                NavigateMore.Dispose ();
                NavigateMore = null;
            }
        }
    }
}