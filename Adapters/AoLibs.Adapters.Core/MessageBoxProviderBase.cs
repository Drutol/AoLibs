using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Core
{
    public abstract class MessageBoxProviderBase : IMessageBoxProvider
    {
        public abstract Task<bool> ShowMessageBoxWithInputAsync(string title, string content, string positiveText, string negativeText);
        public abstract Task ShowMessageBoxOkAsync(string title, string content, string neutralText);
        
        public abstract void ShowLoadingPopup(string title = null, string content = null);
        public abstract void HideLoadingDialog();

        public IDisposable LoaderLifetime => new LoaderLifetimeManager(this);

        class LoaderLifetimeManager : IDisposable
        {
            private readonly MessageBoxProviderBase _parent;

            public LoaderLifetimeManager(MessageBoxProviderBase parent)
            {
                _parent = parent;
                _parent.ShowLoadingPopup();
            }

            public void Dispose()
            {
                _parent.HideLoadingDialog();
            }
        }

    }
}
