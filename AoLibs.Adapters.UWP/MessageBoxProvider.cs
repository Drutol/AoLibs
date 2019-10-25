using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        public override async Task<bool> ShowMessageBoxWithInputAsync(
            string title,
            string content,
            string positiveText,
            string negativeText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            var messageDialog = new MessageDialog(content, title);
            messageDialog.Commands.Add(new UICommand(positiveText){Id = 1});
            messageDialog.Commands.Add(new UICommand(negativeText){Id = null});
            var result = await messageDialog.ShowAsync();

            return result.Id != null;
        }

        public override async Task ShowMessageBoxOkAsync(string title, string content, string neutralText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            var messageDialog = new MessageDialog(content, title);
            messageDialog.Commands.Add(new UICommand(neutralText));
            await messageDialog.ShowAsync();
        }

        public override async Task<string> ShowTextInputBoxAsync(string title, string content, string hint,
            string positiveText, string neutralText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            var inputTextBox = new TextBox
            {
                AcceptsReturn = false,
                PlaceholderText = hint,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            var dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = positiveText,
                SecondaryButtonText = neutralText
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;

            return null;
        }

        public override void ShowLoadingPopup(
            string title = null,
            string content = null,
            INativeDialogStyle nativeDialogStyle = null)
        {
            ShowLoadingPopupRequest?.Invoke(this, (title,content));
        }

        public override void HideLoadingDialog()
        {
            HideLoadingPopupRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}