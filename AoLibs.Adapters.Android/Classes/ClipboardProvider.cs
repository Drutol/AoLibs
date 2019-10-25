using Android.App;
using Android.Content;
using Android.Runtime;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Provides basic clipboard functionality.
    /// </summary>
    public class ClipboardProvider : IClipboardProvider
    {
        public void SetText(string text)
        {
            var clipboard = (ClipboardManager)Application.Context.GetSystemService(Context.ClipboardService);
            var clip = ClipData.NewPlainText(string.Empty, text);
            clipboard.PrimaryClip = clip;
        }
    }
}