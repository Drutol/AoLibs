using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using Google.Android.Material.TextField;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Provides functionality of presenting message boxes,
    /// additionally allows to streamline display of any loaders you may happen to have.
    /// </summary>
    public class MessageBoxProvider : MessageBoxProviderBase
    {
        private readonly IContextProvider _contextProvider;
        private AlertDialog _currentLoadingDialog;

        public event EventHandler<(string title, string content)> ShowLoadingPopupRequest;
        public event EventHandler HideLoadingPopupRequest;

        private class DialogDissmissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        {
            private readonly Action _action;

            public DialogDissmissListener(Action action)
            {
                _action = action;
            }

            public void OnDismiss(IDialogInterface dialog)
            {
                _action.Invoke();
            }
        }

        private class DialogCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
        {
            private readonly Action _action;

            public DialogCancelListener(Action action)
            {
                _action = action;
            }

            public void OnCancel(IDialogInterface dialog)
            {
                _action.Invoke();
            }
        }

        public MessageBoxProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public override async Task<bool> ShowMessageBoxWithInputAsync(
            string title,
            string content,
            string positiveText,
            string negativeText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            nativeDialogStyle ??= DefaultDialogStyles.PasswordInputDialogStyle;
            var style = (INativeAndroidDialogStyle) nativeDialogStyle;

            var sem = new SemaphoreSlim(0);
            bool res = false;
            var dialog = style is null || style.ThemeResourceId == default
                ? new AlertDialog.Builder(_contextProvider.CurrentContext)
                : new AlertDialog.Builder(_contextProvider.CurrentContext, style.ThemeResourceId);

            dialog.SetPositiveButton(positiveText, (sender, args) =>
            {
                res = true;
                sem.Release();
            });
            dialog.SetNegativeButton(negativeText, (sender, args) =>
            {
                res = false;
                sem.Release();
            });

            dialog.SetTitle(title);
            dialog.SetMessage(content);
            dialog.SetCancelable(false);
            style?.SetStyle(dialog);
            var alertDialog = dialog.Show();
            style?.SetStyle(alertDialog);

            await sem.WaitAsync();
            return res;
        }

        public override async Task ShowMessageBoxOkAsync(
            string title,
            string content,
            string neutralText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            nativeDialogStyle ??= DefaultDialogStyles.DialogStyle;
            var style = (INativeAndroidDialogStyle)nativeDialogStyle;

            var sem = new SemaphoreSlim(0);
            var dialog = style is null || style.ThemeResourceId == default
                ? new AlertDialog.Builder(_contextProvider.CurrentContext)
                : new AlertDialog.Builder(_contextProvider.CurrentContext, style.ThemeResourceId);

            dialog.SetNeutralButton(neutralText, (sender, args) => { sem.Release(); });
            dialog.SetTitle(title);
            dialog.SetMessage(content);
            dialog.SetCancelable(false);
            style?.SetStyle(dialog);
            var alertDialog = dialog.Show();
            style?.SetStyle(alertDialog);

            dialog.SetOnDismissListener(new DialogDissmissListener(() => sem.Release()));
            dialog.SetOnCancelListener(new DialogCancelListener(() => sem.Release()));
            await sem.WaitAsync();
        }

        public override async Task<string> ShowTextInputBoxAsync(
            string title,
            string content,
            string hint,
            string positiveText,
            string neutralText,
            INativeDialogStyle nativeDialogStyle = null)
        {
            nativeDialogStyle ??= DefaultDialogStyles.PasswordInputDialogStyle;
            var style = (INativeAndroidDialogStyle) nativeDialogStyle;

            // prepare input
            var inputLayout = new TextInputLayout(_contextProvider.CurrentContext);
            var input = new TextInputEditText(_contextProvider.CurrentContext) {Hint = hint};
            inputLayout.SetPadding(
                left: (int) (19 * Application.Context.Resources.DisplayMetrics.Density),
                top: 0,
                right: (int) (19 * Application.Context.Resources.DisplayMetrics.Density),
                bottom: 0);
            inputLayout.AddView(input);

            var sem = new SemaphoreSlim(0);
            string res = null;
            var dialog = style is null || style.ThemeResourceId == default
                ? new AlertDialog.Builder(_contextProvider.CurrentContext)
                : new AlertDialog.Builder(_contextProvider.CurrentContext, style.ThemeResourceId);

            dialog.SetPositiveButton(positiveText, (sender, args) =>
            {
                res = input.Text;
                sem.Release();
            });
            dialog.SetNegativeButton(neutralText, (sender, args) =>
            {
                res = null;
                sem.Release();
            });
            dialog.SetView(inputLayout);
            dialog.SetTitle(title);
            dialog.SetMessage(content);
            dialog.SetCancelable(false);
            style?.SetStyle(dialog, inputLayout);
            var alertDialog = dialog.Show();
            style?.SetStyle(alertDialog);

            await sem.WaitAsync();
            return res;
        }

        public override void ShowLoadingPopup(
            string title,
            string content,
            INativeDialogStyle nativeDialogStyle = null)
        {
            nativeDialogStyle ??= DefaultDialogStyles.LoadingDialogStyle;
            var style = (INativeAndroidLoadingDialogStyle)nativeDialogStyle;

            _currentLoadingDialog?.Hide();

            // use default when not specified
            if (!(style?.UseDefault ?? true))
            {
                ShowLoadingPopupRequest?.Invoke(this, (title, content));
                return;
            }

            var bottomMargin = title == null && content == null ? 16 : 32;

            var layout = new FrameLayout(_contextProvider.CurrentContext);
            var loadingView = new ProgressBar(_contextProvider.CurrentContext)
            {
                LayoutParameters = new FrameLayout.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent)
                {
                    Gravity = GravityFlags.Center,
                    TopMargin = (int)(16 * Application.Context.Resources.DisplayMetrics.Density),
                    BottomMargin = (int)(bottomMargin * Application.Context.Resources.DisplayMetrics.Density),
                }
            };
            layout.AddView(loadingView);

            var dialog = style is null || style.ThemeResourceId == default
                ? new AlertDialog.Builder(_contextProvider.CurrentContext)
                : new AlertDialog.Builder(_contextProvider.CurrentContext, style.ThemeResourceId);

            dialog.SetView(layout);
            dialog.SetTitle(title);
            dialog.SetMessage(content);
            dialog.SetCancelable(false);
            style?.SetStyle(dialog, layout);
            _currentLoadingDialog = dialog.Show();
            style?.SetStyle(_currentLoadingDialog);
        }

        public override void HideLoadingDialog()
        {
            HideLoadingPopupRequest?.Invoke(this, EventArgs.Empty);
            _currentLoadingDialog?.Hide();
        }
    }
}