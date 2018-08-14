using System;
using AoLibs.Adapters.Core.Interfaces;
using Foundation;

namespace AoLibs.Adapters.iOS
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetString(string key)
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(key);
        }

        public void SetString(string key, string value)
        {
            if (value == null)
                NSUserDefaults.StandardUserDefaults.RemoveObject(key);
            else
                NSUserDefaults.StandardUserDefaults.SetString(value, key);
        }
   
        public bool? GetBool(string key)
        {
            try
            {
                if (NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString(key)) == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            return NSUserDefaults.StandardUserDefaults.BoolForKey(key);
        }

        public void SetBool(string key, bool value)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(value, key);
        }

        public int? GetInt(string key)
        {
            try
            {
                if (NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString(key)) == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            return (int)NSUserDefaults.StandardUserDefaults.IntForKey(key);
        }

        public void SetInt(string key, int value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt(value, key);
        }

        public long? GetLong(string key)
        {
            try
            {
                if (NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString(key)) == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            return long.Parse(NSUserDefaults.StandardUserDefaults.StringForKey(key));
        }

        public void SetLong(string key, long value)
        {
            NSUserDefaults.StandardUserDefaults.SetString(value.ToString(), key);
        }

        public double? GetDouble(string key)
        {
            try
            {
                if (NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString(key)) == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            return (double)NSUserDefaults.StandardUserDefaults.DoubleForKey(key);
        }

        public void SetDouble(string key, double value)
        {
            NSUserDefaults.StandardUserDefaults.SetDouble(value, key);
        }
    }
}