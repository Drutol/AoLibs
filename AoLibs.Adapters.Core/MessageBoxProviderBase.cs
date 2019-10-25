using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Core
{
    /// <summary>
    /// Base class for MessageBoxProviders, additionally allows to obtain "Lifetime" object of your loader so it can be conviniently used in using statement.
    /// </summary>
    public abstract class MessageBoxProviderBase : IMessageBoxProvider
    {
        public abstract Task<bool> ShowMessageBoxWithInputAsync(
            string title,
            string content,
            string positiveText,
            string negativeText,
            INativeDialogStyle nativeDialogStyle = null);

        public abstract Task ShowMessageBoxOkAsync(
            string title,
            string content,
            string neutralText,
            INativeDialogStyle nativeDialogStyle = null);

        public abstract Task<string> ShowTextInputBoxAsync(
            string title,
            string content,
            string hint,
            string positiveText,
            string neutralText,
            INativeDialogStyle nativeDialogStyle = null);

        public abstract void ShowLoadingPopup(
            string title = null,
            string content = null,
            INativeDialogStyle nativeDialogStyle = null);

        public abstract void HideLoadingDialog();

        public IDisposable LoaderLifetime => new LoaderLifetimeManager(this);
        public IDisposable ObtainLoaderLifetime(string title, string content, INativeDialogStyle nativeDialogStyle) =>
            new LoaderLifetimeManager(this, title, content);

        private class LoaderLifetimeManager : IDisposable
        {
            private readonly MessageBoxProviderBase _parent;

            public LoaderLifetimeManager(MessageBoxProviderBase parent)
            {
                _parent = parent;
                _parent.ShowLoadingPopup();
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
