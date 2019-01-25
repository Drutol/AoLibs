using System;
using UIKit;

namespace AoLibs.Dialogs.iOS.Models
{
    /// <summary>
    /// Dialog background config.
    /// </summary>
    public class DialogBackgroundConfig
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public UIColor Color { get; set; } = UIColor.Clear;
       
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:AoLibs.Dialogs.iOS.Models.DialogBackgroundConfig"/> blur enabled.
        /// </summary>
        public bool BlurEnabled { get; set; } = true;
       
        /// <summary>
        /// Gets or sets the blur style.
        /// </summary>
        public UIBlurEffectStyle BlurStyle { get; set; } = UIBlurEffectStyle.Regular;
    }
}
