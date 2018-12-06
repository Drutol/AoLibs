using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    /// <summary>
    /// Defines shared config of dialog.
    /// </summary>
    public class CustomDialogConfig
    {
        /// <summary>
        /// Flags enum allowing to define dialogs position on screen.
        /// </summary>
        [Flags]
        public enum DialogGravity
        {
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8,
            Center = 16
        }

        /// <summary>
        /// Gets or sets the gravity, position on screen of the dialog. The <see cref="DialogGravity"/> is a bitfield.
        /// </summary>
        public DialogGravity Gravity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stretch the dialog container horizontally.
        /// </summary>
        public bool StretchHorizontally { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stretch the dialog container vertically.
        /// </summary>
        public bool StretchVertically { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog can be cancelled manually by the user.
        /// </summary>
        public bool IsCancellable { get; set; } = true;
    }
}
