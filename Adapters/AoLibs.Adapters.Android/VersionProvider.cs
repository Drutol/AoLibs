using Android.App;
using Android.Runtime;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    [Preserve(AllMembers = true)]
    public class VersionProvider : IVersionProvider
    {
        private static string _currentVersion;

        public VersionProvider()
        {
            var package = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
            _currentVersion = package.VersionName;
        }

        public string Version => _currentVersion;

        public PlatformType Platform { get; } = PlatformType.Android;
    }
}