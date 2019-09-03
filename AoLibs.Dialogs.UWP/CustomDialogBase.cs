using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Dialogs.UWP.Interfaces;

namespace AoLibs.Dialogs.UWP
{
    public abstract class CustomDialogBase : ContentDialog, ICustomDialogForViewModel
    {
        internal static ICustomDialogDependencyResolver CustomDialogDependencyResolver { get; set; }
        internal static IInternalDialogsManager DialogsManager { get; set; }

        /// <summary>
        /// Fires when the dialog is shown.
        /// </summary>
        public event EventHandler DialogShown;

        /// <summary>
        /// Fires when the dialog is hidden.
        /// </summary>
        public event EventHandler DialogHidden;

        /// <summary>
        /// Fired when dialog is about to be shown.
        /// </summary>
        public event EventHandler DialogWillShow;

        /// <summary>
        /// Fired when dialog is about to be hidden.
        /// </summary>
        public event EventHandler DialogWillHide;

        /// <summary>
        /// Token source used to monitor <see cref="AwaitResult{TResult}"/> process.
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// Completion source being the lifeline of <see cref="AwaitResult{TResult}"/> process.
        /// </summary>
        private TaskCompletionSource<object> _resultCompletionSource;

        /// <summary>
        /// Callback when dialog has been dismissed.
        /// </summary>
        protected virtual void OnHidden()
        {
        }

        /// <summary>
        /// Callback when dialog has been shown.
        /// </summary>
        protected virtual void OnShown()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogBase"/> class.
        /// </summary>
        protected CustomDialogBase()
        {
            base.Closing += (sender, args) => HideInternal(false);
        }

        /// <summary>
        /// Gets awaited type passed using <see cref="ICustomDialog.AwaitResult{TResult}"/>
        /// </summary>
        protected Type AwaitedResultType { get; private set; }

        /// <summary>
        /// Gets or sets dialog config used when creating the dialog.
        /// </summary>
        protected CustomDialogConfig CustomDialogConfig { get; set; } = new CustomDialogConfig();

        /// <summary>
        /// Gets or sets the parameter the dialog was called with.
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Shows the dialog with given parameter.
        /// </summary>
        /// <param name="parameter">Parameter which can be passed to dialog's ViewModel.</param>
        public async void Show(object parameter = null)
        {
            DialogsManager.CurrentlyDisplayedDialog = this;
            DialogWillShow?.Invoke(this, EventArgs.Empty);

            Parameter = parameter;
            await base.ShowAsync(ContentDialogPlacement.Popup);
            OnShown();
            DialogShown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        public new void Hide()
        {
            HideInternal(false);
        }

        /// <summary>
        /// Acts in the same way as <see cref="Show"/> in Android implementation.
        /// </summary>
        /// <param name="parameter">Parameter which can be passed to dialog's ViewModel.</param>
        public Task ShowAsync(object parameter = null)
        {
            Parameter = parameter;
            Show();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Acts in the same way as <see cref="Hide"/> in Android implementation.
        /// </summary>
        public Task HideAsync()
        {
            Hide();
            return Task.CompletedTask;
        }

        internal void HideInternal(bool fromHidden)
        {
            DialogWillHide?.Invoke(this, EventArgs.Empty);
            CancelResult();
            if(!fromHidden)
             base.Hide();
            OnHidden();
            DialogHidden?.Invoke(this, EventArgs.Empty);
            DialogsManager.CurrentlyDisplayedDialog = null;
        }

        /// <summary>
        /// Allows to await certain result from dialog.
        /// The dialog can yield the result by using <see cref="SetResult"/> or <see cref="CancelResult"/> methods.
        /// </summary>
        /// <typeparam name="TResult">Awaited return type, it will be checked when dialog calls <see cref="Show"/></typeparam>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Awaited result</returns>
        /// <exception cref="TaskCanceledException">Throws this exception when result gets cancelled by either <see cref="CancellationToken"/> or <see cref="CancelResult"/> method</exception>
        public async Task<TResult> AwaitResult<TResult>(CancellationToken token = default)
        {
            try
            {
                if (_resultCompletionSource != null)
                    return (TResult)await _resultCompletionSource.Task;

                Show();
                AwaitedResultType = typeof(TResult);
                _resultCompletionSource = new TaskCompletionSource<object>();
                _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                using (token.Register(() => _resultCompletionSource.SetCanceled()))
                {
                    return (TResult)await _resultCompletionSource.Task;
                }
            }
            finally
            {
                AwaitedResultType = null;
                _resultCompletionSource = null;
                _cts = null;
            }
        }

        /// <summary>
        /// Sets dialog invocation result.
        /// Completes the task awaited in <see cref="AwaitResult{TResult}"/>.
        /// </summary>
        /// <param name="result">The object to return to the caller. It should be of <see cref="AwaitedResultType"/> type.</param>
        /// <exception cref="ArgumentException">Thrown when given result doesn't match <see cref="AwaitedResultType"/></exception>
        public void SetResult(object result)
        {
            if (AwaitedResultType != result.GetType())
                throw new ArgumentException($"Result should be of {AwaitedResultType.Name} type.");

            _resultCompletionSource?.SetResult(result);
        }

        /// <summary>
        /// Cancels currently awaited <see cref="AwaitResult{TResult}"/> causing <see cref="TaskCanceledException"/>
        /// </summary>
        public void CancelResult()
        {
            _cts?.Cancel();
            _cts = null;
        }

        protected TViewModel ResolveViewModel<TViewModel>() where TViewModel : CustomDialogViewModelBase
        {
            var vm = CustomDialogDependencyResolver?.Resolve<TViewModel>();

            if (vm != null)
            {
                vm.Dialog = this;
                DataContext = vm;
                CustomDialogConfig = vm.CustomDialogConfig;
            }

            return vm;
        }

        protected TArgument ResolveArgument<TArgument>()
        {
            if (Parameter is TArgument param)
                return param;
            return default;
        }
    }
}
