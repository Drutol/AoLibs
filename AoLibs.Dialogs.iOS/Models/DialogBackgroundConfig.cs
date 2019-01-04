using System;
using UIKit;

namespace AoLibs.Dialogs.iOS.Models
{
    public class DialogBackgroundConfig
    {
        public UIColor Color { get; set; } = UIColor.Clear;
        public bool BlurEnabled { get; set; } = true;
        public UIBlurEffectStyle BlurStyle { get; set; } = UIBlurEffectStyle.Regular;
    }
}
