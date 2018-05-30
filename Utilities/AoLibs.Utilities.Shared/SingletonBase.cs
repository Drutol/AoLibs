using System;

namespace AoLibs.Utilities.Shared
{
    public class SingletonBase<T> where T : class
    {
        private static T _instance;

        public static T Instance => _instance ?? (_instance = Activator.CreateInstance<T>());
    }
}
