// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [Register ("TestPageAViewController")]
    partial class TestPageAViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChooseFancyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NavigateButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShowFancyButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ChooseFancyButton != null) {
                ChooseFancyButton.Dispose ();
                ChooseFancyButton = null;
            }

            if (NavigateButton != null) {
                NavigateButton.Dispose ();
                NavigateButton = null;
            }

            if (ShowFancyButton != null) {
                ShowFancyButton.Dispose ();
                ShowFancyButton = null;
            }
        }
    }
}