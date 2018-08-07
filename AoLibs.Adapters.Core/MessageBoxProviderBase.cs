using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Core
{
    /// <summary>
    /// Base class for MessageBoxProviders, addiditonnaly allows to obtain "Lifetime" object of your loader so it can be conviniently used in using statement.
    /// </summary>
    public abstract class MessageBoxProviderBase : IMessageBoxProvider
    {
        public abstract Task<bool> ShowMessageBoxWithInputAsync(string title, string content, string positiveText, string negativeText);
        public abstract Task ShowMessageBoxOkAsync(string title, string content, string neutralText);

        public abstract void ShowLoadingPopup(string title = null, string content = null);
        public abstract void HideLoadingDialog();

        public IDisposable LoaderLifetime => new LoaderLifetimeManager(this);

        public IDisposable ObtainLoaderLifetime(string title, string content) => new LoaderLifetimeManager(this,title,content);

        class LoaderLifetimeManager : IDisposable
        {
            private readonly MessageBoxProviderBase _parent;

            public LoaderLifetimeManager(MessageBoxProviderBase parent)
            {
                _parent = parent;
                
            }

            public LoaderLifetimeManager(MessageBoxProviderBase parent, string title, string content)
            {
                _parent = parent;
                _parent.ShowLoadingPopup(title, content);
            }

            public void Dispose()
            {
                _parent.HideLoadingDialog();
            }
        }
    }
}
