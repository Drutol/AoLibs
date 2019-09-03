using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Allows to invoke given delegate on UI thread.
    /// </summary>
    public class DispatcherAdapter : IDispatcherAdapter
    {
        public async void Run(Action action)
        {
            var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;
            if(dispatcher != null)
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}