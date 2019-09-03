using System;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Allows to invoke given delegate on UI thread.
    /// </summary>
    public class DispatcherAdapter : IDispatcherAdapter
    {
        public void Run(Action action)
        {
            throw new NotImplementedException();
        }
    }
}