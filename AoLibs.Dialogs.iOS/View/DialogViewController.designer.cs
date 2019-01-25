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

namespace AoLibs.Dialogs.iOS
{
    [Register ("DialogViewController")]
    partial class DialogViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ContainerBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ContainerCenterX { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ContainerCenterY { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ContainerLeading { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ContainerTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ContainerTrailing { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIVisualEffectView EffectView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LayoutView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RootView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContainerBottom != null) {
                ContainerBottom.Dispose ();
                ContainerBottom = null;
            }

            if (ContainerCenterX != null) {
                ContainerCenterX.Dispose ();
                ContainerCenterX = null;
            }

            if (ContainerCenterY != null) {
                ContainerCenterY.Dispose ();
                ContainerCenterY = null;
            }

            if (ContainerLeading != null) {
                ContainerLeading.Dispose ();
                ContainerLeading = null;
            }

            if (ContainerTop != null) {
                ContainerTop.Dispose ();
                ContainerTop = null;
            }

            if (ContainerTrailing != null) {
                ContainerTrailing.Dispose ();
                ContainerTrailing = null;
            }

            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (EffectView != null) {
                EffectView.Dispose ();
                EffectView = null;
            }

            if (LayoutView != null) {
                LayoutView.Dispose ();
                LayoutView = null;
            }

            if (RootView != null) {
                RootView.Dispose ();
                RootView = null;
            }
        }
    }
}