using System;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Allows to invoke given delegate on UI thread.
    /// </summary>
    public interface IDispatcherAdapter
    {
        void Run(Action action);
    }
}
