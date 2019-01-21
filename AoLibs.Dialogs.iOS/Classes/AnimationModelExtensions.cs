using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Dialogs.iOS.Models;
using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    internal static class AnimationModelExtensions
    {
        internal static UIModalTransitionStyle ToModalTransition(this DialogAnimationType type)
        {
            switch (type)
            {
                case DialogAnimationType.SystemCoverVertical:
                    return UIModalTransitionStyle.CoverVertical;
                case DialogAnimationType.SystemFlipHorizontal:
                    return UIModalTransitionStyle.FlipHorizontal;
                default:
                    return UIModalTransitionStyle.CrossDissolve;
            }
        }

        internal static bool IsSystemAnimation(this DialogAnimationType type)
        {
            return type == DialogAnimationType.SystemCrossDissolve ||
                   type == DialogAnimationType.SystemCoverVertical ||
                   type == DialogAnimationType.SystemFlipHorizontal;
        }
    }
}