using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public class CustomDialogConfig
    {
        [Flags]
        public enum DialogGravity
        {
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8,
            Center = 16
        }

        public DialogGravity Gravity { get; set; }

        public bool StretchHorizontally { get; set; }
        public bool StretchVertically { get; set; }

        public bool IsCancellable { get; set; } = true;
    }
}
