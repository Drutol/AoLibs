using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Runtime;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Provides simple facade over underlying <see cref="PreferenceManager"/>
    /// </summary>
    public class SettingsProvider : ISettingsProvider
    {
        private static ISharedPreferences _preferenceManager;

        public SettingsProvider()
        {
            _preferenceManager = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
        }

        public string GetString(string key)
        {
            return _preferenceManager.GetString(key, null);
        }

        public void SetString(string key, string value)
        {
            var editor = _preferenceManager.Edit();
            editor.PutString(key, value);
            editor.Commit();
        }

        public bool? GetBool(string key)
        {
            if (_preferenceManager.Contains(key))
                return _preferenceManager.GetBoolean(key, false);
            return null;
        }

        public void SetBool(string key, bool value)
        {
            var editor = _preferenceManager.Edit();
            editor.PutBoolean(key, value);
            editor.Commit();
        }

        public int? GetInt(string key)
        {
            if (_preferenceManager.Contains(key))
                return _preferenceManager.GetInt(key, 0);
            return null;
        }

        public void SetInt(string key, int value)
        {
            var editor = _preferenceManager.Edit();
            editor.PutInt(key, value);
            editor.Commit();
        }

        public long? GetLong(string key)
        {
            if (_preferenceManager.Contains(key))
                return _preferenceManager.GetLong(key, 0);
            return null;
        }

        public void SetLong(string key, long value)
        {
            var editor = _preferenceManager.Edit();
            editor.PutLong(key, value);
            editor.Commit();
        }

        public double? GetDouble(string key)
        {
            if (_preferenceManager.Contains(key))
                return _preferenceManager.GetFloat(key, 0);
            return null;
        }

        public void SetDouble(string key, double value)
        {
            var editor = _preferenceManager.Edit();
            editor.PutFloat(key, (float)value);
            editor.Commit();
        }
    }
}