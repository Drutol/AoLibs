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
    [Register ("TestPageCViewController")]
    partial class TestPageCViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GoBackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NavigateAToFirstOccurenceButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NavigateBToFirstOccurence { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (GoBackButton != null) {
                GoBackButton.Dispose ();
                GoBackButton = null;
            }

            if (NavigateAToFirstOccurenceButton != null) {
                NavigateAToFirstOccurenceButton.Dispose ();
                NavigateAToFirstOccurenceButton = null;
            }

            if (NavigateBToFirstOccurence != null) {
                NavigateBToFirstOccurence.Dispose ();
                NavigateBToFirstOccurence = null;
            }
        }
    }
}