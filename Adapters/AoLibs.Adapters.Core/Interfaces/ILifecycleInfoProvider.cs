using System;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface ILifecycleInfoProvider
    {
        event EventHandler AppWentBackground;
        event EventHandler AppWentForegound;
    }
}
