using System;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.iOS
{
    public class LifecycleInfoProvider : ILifecycleInfoProvider
    {
        public LifecycleInfoProvider()
        {
        }

        public event EventHandler AppWentBackground;
        public event EventHandler AppWentForegound;
    }
}
