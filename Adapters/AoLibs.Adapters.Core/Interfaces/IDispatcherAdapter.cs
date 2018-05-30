using System;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface IDispatcherAdapter
    {
        void Run(Action action);
    }
}
