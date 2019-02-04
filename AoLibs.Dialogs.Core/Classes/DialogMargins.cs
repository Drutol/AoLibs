using System;
namespace AoLibs.Dialogs.Core
{
    /// <summary>
    /// Defines margins that will be applied to dialog.
    /// </summary>
    public class DialogMargins
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogMargins"/> class.
        /// </summary>
        /// <param name="margin">The margin for all four directions</param>
        public DialogMargins(int margin)
        {
            Left = margin;
            Right = margin;
            Top = margin;
            Bottom = margin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogMargins"/> class.
        /// </summary>
        /// <param name="horizontal">Margin for left and right sides</param>
        /// <param name="vertical">Margin for top and bottom sides</param>
        public DialogMargins(int horizontal, int vertical)
        {
            Left = horizontal;
            Right = horizontal;
            Top = vertical;
            Bottom = vertical;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogMargins"/> class with all margins specified in clockwise direction starting from left.
        /// </summary>
        /// <param name="left">Left margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="right">Right margin.</param>
        /// <param name="bottom">Bottom margin.</param>
        public DialogMargins(int left, int top, int right, int bottom)
        {
            Left = left;
            Right = top;
            Top = right;
            Bottom = bottom;
        }

        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
    }
}
