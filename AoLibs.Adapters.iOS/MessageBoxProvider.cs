using System;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Adapters.Core;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    public class MessageBoxProvider : MessageBoxProviderBase
    {      
        public event EventHandler<(string title, string content)> ShowLoadingPopupRequest;
        public event EventHandler HideLoadingPopupRequest;

        public override async Task<bool> ShowMessageBoxWithInputAsync(string title, string content, string positiveText, string negativeText)
        {
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
            alert.Show();
            await semaphore.WaitAsync();
            return result;
        }

        public override async Task ShowMessageBoxOkAsync(string title, string content, string neutralText)
        {
            var alert = new UIAlertView(title, content, (IUIAlertViewDelegate)null, neutralText);
            var semaphore = new SemaphoreSlim(0);

            alert.Dismissed += (sender, args) => semaphore.Release();
            alert.Show();

            await semaphore.WaitAsync();
        }

        public override Task<string> ShowTextInputBoxAsync(string title, string content, string hint, string positiveText, string neutralText)
        {
            throw new NotImplementedException();
        }

        public override void ShowLoadingPopup(string title, string content)
        {
            ShowLoadingPopupRequest?.Invoke(this, (title, content));
        }

        public override void HideLoadingDialog()
        {
            HideLoadingPopupRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}