using AoLibs.Adapters.Core.Interfaces;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    public class ClipboardProvicer : IClipboardProvider
    {
        public void SetText(string text)
        {
            UIPasteboard.General.String = text;
        }
    }
}