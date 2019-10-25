using System;
using AoLibs.Adapters.Core.Interfaces;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    public class DispatcherAdapter : IDispatcherAdapter
    {
        public void Run(Action action)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(action);
        }
    }
}
