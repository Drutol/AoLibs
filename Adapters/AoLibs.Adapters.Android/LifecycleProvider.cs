using System;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    public class LifecycleInfoProvider : ILifecycleInfoProvider
    {
        public event EventHandler AppWentBackground;
        public event EventHandler AppWentForegound;

        public void OnResume()
        {
            AppWentForegound?.Invoke(this, EventArgs.Empty);
        }

        public void OnPause()
        {
            AppWentBackground?.Invoke(this, EventArgs.Empty);
        }
    }
}