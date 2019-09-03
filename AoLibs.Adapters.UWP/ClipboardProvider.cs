
using Windows.ApplicationModel.DataTransfer;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Provides basic clipboard functionality.
    /// </summary>
    public class ClipboardProvider : IClipboardProvider
    {
        public void SetText(string text)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
    }
}