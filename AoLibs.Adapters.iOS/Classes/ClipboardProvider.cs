using AoLibs.Adapters.Core.Interfaces;
using UIKit;

namespace AoLibs.Adapters.IOS
{
    public class ClipboardProvider : IClipboardProvider
    {
        public void SetText(string text)
        {
            UIPasteboard.General.String = text;
        }
    }
}