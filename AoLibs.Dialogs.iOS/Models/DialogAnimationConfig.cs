using System;
using UIKit;

namespace AoLibs.Dialogs.iOS.Models
{
    /// <summary>
    /// Dialog animation configuration.
    /// </summary>
    public class DialogAnimationConfig
    {
        /// <summary>
        /// Gets or sets the type of the show animation.
        /// </summary>
        /// <value>The type of the show animation.</value>
        public DialogAnimationType ShowAnimationType { get; set; } = DialogAnimationType.CustomBlurFade;
        /// <summary>
        /// Gets or sets the type of the hide animation.
        /// </summary>
        /// <value>The type of the hide animation.</value>
        public DialogAnimationType HideAnimationType { get; set; } = DialogAnimationType.CustomBlurFade;
        /// <summary>
        /// Gets or sets the show custom animation duration in seconds.
        /// </summary>
        /// <value>The show custom animation duration seconds.</value>
        public float ShowCustomAnimationDurationSeconds { get; set; } = .3f;
        /// <summary>
        /// Gets or sets the hide custom animation duration in seconds.
        /// </summary>
        /// <value>The hide custom animation duration seconds.</value>
        public float HideCustomAnimationDurationSeconds { get; set; } = .3f;
    }

    /// <summary>
    /// Type of the animation, contains both system and custom animations. Partial curl is not supported.
    /// </summary>
    public enum DialogAnimationType
    {
        /// <summary>
        /// No animation.
        /// </summary>
        None,

        /// <summary>
        /// The system UIModalTransitionStyle.CrossDissolve.
        /// </summary>
        SystemCrossDissolve,

        /// <summary>
        /// The system UIModalTransitionStyle.CoverVertical.
        /// </summary>
        SystemCoverVertical,

        /// <summary>
        /// The system UIModalTransitionStyle.FlipHorizontal.
        /// </summary>
        SystemFlipHorizontal,

        /// <summary>
        /// Custom AOLibs animation that involves gradually blurring in/out the background if blur background is selected.
        /// </summary>
        CustomBlurFade,
    }

    internal static class AnimationModelExtensions
    {
        internal static UIModalTransitionStyle ToUIModalTransition(this DialogAnimationType type)
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
