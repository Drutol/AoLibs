using AoLibs.Adapters.Core.Interfaces;
using Foundation;

namespace AoLibs.Adapters.iOS
{
    public class VersionProvider : IVersionProvider
    {
        public string Version { get; } = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
        public PlatformType Platform { get; } = PlatformType.iOS;
    }
}