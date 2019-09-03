
using Windows.Storage;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    public class SettingsProvider : ISettingsProvider
    {
        private ApplicationDataContainer _settings;

        public SettingsProvider()
        {
            _settings = ApplicationData.Current.LocalSettings;
        }

        public string GetString(string key)
        {
            return (string) (_settings.Values[key] ?? null);
        }

        public void SetString(string key, string value)
        {
            _settings.Values[key] = value;
        }

        public bool? GetBool(string key)
        {
            return (bool?)(_settings.Values[key] ?? null);
        }

        public void SetBool(string key, bool value)
        {
            _settings.Values[key] = value;
        }

        public int? GetInt(string key)
        {
            return (int?)(_settings.Values[key] ?? null);
        }

        public void SetInt(string key, int value)
        {
            _settings.Values[key] = value;
        }

        public long? GetLong(string key)
        {
            return (long?)(_settings.Values[key] ?? null);
        }

        public void SetLong(string key, long value)
        {
            _settings.Values[key] = value;
        }

        public double? GetDouble(string key)
        {
            return (double?)(_settings.Values[key] ?? null);
        }

        public void SetDouble(string key, double value)
        {
            _settings.Values[key] = value;
        }

        public void Remove(string key)
        {
            _settings.Values.Remove(key);
        }
    }
}