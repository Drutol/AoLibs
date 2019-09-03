using Windows.ApplicationModel;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Provides basic information about platform and package version.
    /// </summary>
    public class VersionProvider : IVersionProvider
    {
        public string Version
        {
            get
            {
                var package = Package.Current;
                var packageId = package.Id;
                var version = packageId.Version;

                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public PlatformType Platform { get; } = PlatformType.UWP;
    }
}