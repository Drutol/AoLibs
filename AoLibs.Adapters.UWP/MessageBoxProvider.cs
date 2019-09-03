using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Provides functionality of presenting message boxes,
    /// additionally allows to streamline display of any loaders you may happen to have.
    /// </summary>
    public class MessageBoxProvider : MessageBoxProviderBase
    {
        public event EventHandler<(string title, string content)> ShowLoadingPopupRequest;
        public event EventHandler HideLoadingPopupRequest;


        public override Task<bool> ShowMessageBoxWithInputAsync(string title, string content, string positiveText, string negativeText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            throw new NotImplementedException();
        }

        public override Task ShowMessageBoxOkAsync(string title, string content, string neutralText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            throw new NotImplementedException();
        }

        public override Task<string> ShowTextInputBoxAsync(string title, string content, string hint, string positiveText, string neutralText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            throw new NotImplementedException();
        }

        public override void ShowLoadingPopup(string title = null, string content = null, INativeLoadingDialogStyle nativeDialogStyle = null)
        {
            throw new NotImplementedException();
        }

        public override void HideLoadingDialog()
        {
            throw new NotImplementedException();
        }
    }
}