using System;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
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