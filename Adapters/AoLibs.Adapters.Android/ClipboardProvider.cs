using Android.App;
using Android.Content;
using Android.Runtime;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    [Preserve(AllMembers = true)]
    public class ClipboardProvider : IClipboardProvider
    {
        public void SetText(string text)
        {
            var clipboard = (ClipboardManager)Application.Context.GetSystemService(Context.ClipboardService);
            var clip = ClipData.NewPlainText("", text);
            clipboard.PrimaryClip = clip;
        }
    }
}