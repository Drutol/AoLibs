using System;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Allows to invoke given delegate on UI thread.
    /// </summary>
    public class DispatcherAdapter : IDispatcherAdapter
    {
        private readonly IContextProvider _contextProvider;

        public DispatcherAdapter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public void Run(Action action)
        {
            _contextProvider.CurrentContext.RunOnUiThread(action);
        }
    }
}