namespace AoLibs.Adapters.Core.Interfaces
{
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
