namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Defines on which platform the app is running.
    /// </summary>
    public enum PlatformType
    {
        Android,
        iOS
    }

    public interface IVersionProvider
    {
        string Version { get; }
        PlatformType Platform { get; }
    }
}
