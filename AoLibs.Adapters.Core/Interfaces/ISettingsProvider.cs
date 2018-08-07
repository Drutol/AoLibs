namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Provides simple facade over underlying <see cref="PreferenceManager"/>
    /// </summary>
    public interface ISettingsProvider
    {
        string GetString(string key);
        void SetString(string key, string value);

        bool? GetBool(string key);
        void SetBool(string key, bool value);

        int? GetInt(string key);
        void SetInt(string key, int value);

        long? GetLong(string key);
        void SetLong(string key, long value);

        double? GetDouble(string key);
        void SetDouble(string key, double value);
    }
}
