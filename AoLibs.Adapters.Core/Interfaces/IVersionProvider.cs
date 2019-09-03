namespace AoLibs.Adapters.Core.Interfaces
{
#pragma warning disable SA1300 // Element must begin with upper-case letter
    /// <summary>
    /// Defines on which platform the app is running.
    /// </summary>
    public enum PlatformType
    {
        Android,
        iOS,
        UWP
    }
#pragma warning restore SA1300 // Element must begin with upper-case letter

    public interface IVersionProvider
    {
        string Version { get; }
        PlatformType Platform { get; }
    }
}
