using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Provides basic information about platform and package version.
    /// </summary>
    public class VersionProvider : IVersionProvider
    {
        public string Version { get; }
        public PlatformType Platform { get; } = PlatformType.UWP;
    }
}