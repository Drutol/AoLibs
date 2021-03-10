using System;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    public class MessageBoxProvider : MessageBoxProviderBase
    {      
        public event EventHandler<(string title, string content)> ShowLoadingPopupRequest;
        public event EventHandler HideLoadingPopupRequest;

        public override async Task<bool> ShowMessageBoxWithInputAsync(
            string title, 
            string content, 
            string positiveText, 
            string negativeText,
            INativeDialogStyle dialogStyle = null)
        {
            dialogStyle ??= DefaultDialogStyles.PasswordInputDialogStyle;
            var style = (INativeiOSDialogStyle) dialogStyle;

            bool result = false;
            var semaphore = new SemaphoreSlim(0);
            var alert = new UIAlertView(title, content, (IUIAlertViewDelegate)null, negativeText, positiveText);
            alert.Clicked += (sender, buttonArgs) =>
            {
                if (buttonArgs.ButtonIndex == 1)
                    result = true;
                semaphore.Release();
            };
            alert.Dismissed += (sender, args) => semaphore.Release();
            style?.SetStyle(alert);
            alert.Show();
            await semaphore.WaitAsync();
            return result;
        }

        public override async Task ShowMessageBoxOkAsync(
            string title,
            string content,
            string neutralText,
            INativeDialogStyle dialogStyle = null)
        {
            dialogStyle ??= DefaultDialogStyles.DialogStyle;
            var style = (INativeiOSDialogStyle) dialogStyle;

            var alert = new UIAlertView(title, content, (IUIAlertViewDelegate)null, neutralText);
            var semaphore = new SemaphoreSlim(0);

            alert.Dismissed += (sender, args) => semaphore.Release();
            style?.SetStyle(alert);
            alert.Show();

            await semaphore.WaitAsync();
        }

        public override Task<string> ShowTextInputBoxAsync(
            string title,
            string content,
            string hint, 
            string positiveText,
            string neutralText,
            INativeDialogStyle dialogStyle = null)
        {
            throw new NotImplementedException();
        }

        public override void ShowLoadingPopup(string title, string content, INativeDialogStyle dialogStyle)
        {
            ShowLoadingPopupRequest?.Invoke(this, (title, content));
        }

        public override void HideLoadingDialog()
        {
            HideLoadingPopupRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}