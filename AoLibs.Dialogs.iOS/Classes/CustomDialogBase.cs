using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Dialogs.iOS.Interfaces;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    public abstract class CustomDialogBase : UIViewController, ICustomDialogForViewModel
    {
        internal static ICustomDialogViewModelResolver CustomDialogViewModelResolver { get; set; }
        internal static UIViewController RootViewController { get; set; }
        internal static IInternalDialogsManager DialogsManager { get; set; }

        public event EventHandler DialogShown;
        public event EventHandler DialogHidden;

        protected List<Binding> Bindings { get; } = new List<Binding>();

        protected DialogViewController ParentContainerViewController { get; set; }
        protected Type AwaitedResultType { get; set; }

        public virtual bool ShouldAnimateOnShow { get; } = true;
        public virtual bool ShouldAnimateOnDismiss { get; } = true;

        public object Parameter { get; set; }

        private SemaphoreSlim _showSemaphore;
        private SemaphoreSlim _hideSemaphore;
        private TaskCompletionSource<object> _resultCompletionSource;
        private CancellationTokenSource _resultCancellationTokenSource;

        /// <summary>
        /// Gets or sets dialog config used when creating the dialog.
        /// </summary>
        public CustomDialogConfig CustomDialogConfig { get; set; } = new CustomDialogConfig();

        protected CustomDialogBase(IntPtr handle) 
            : base(handle)
        {
            Initialize();
        }

        protected CustomDialogBase(string name, NSBundle p) 
            : base(name, p)
        {
            Initialize();
        }

        private void Initialize()
        {
            ParentContainerViewController = DialogViewController.Instantiate(this);
        }

        private void HideDialogOnOutsideTap(object sender, EventArgs e)
        {
            Hide();
        }

        public void Show(object parameter = null)
        {
            Parameter = parameter; // is my addition okay?
            DialogsManager.CurrentlyDisplayedDialog = this;
            RootViewController.PresentViewController(ParentContainerViewController, false, OnDialogPresentationFinished);
        }

        public void Hide()
        {
            RootViewController.DismissViewController(false, OnDialogDismissFinished);
        }

        public Task ShowAsync(object parameter = null)
        {
            _showSemaphore = new SemaphoreSlim(0);
            Show(parameter);
            return _showSemaphore.WaitAsync();
        }

        public Task HideAsync()
        {
            _hideSemaphore = new SemaphoreSlim(0);
            Hide();
            return _hideSemaphore.WaitAsync();
        }

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

        private void OnDialogDismissFinished()
        {
            DialogHidden?.Invoke(this, EventArgs.Empty);
            _hideSemaphore?.Release();
            _hideSemaphore = null;
            OnHidden();
        }

        private void OnDialogPresentationFinished()
        {
            DialogsManager.CurrentlyDisplayedDialog = null;

            DialogShown?.Invoke(this, EventArgs.Empty);
            _showSemaphore?.Release();
            _showSemaphore = null;
            OnShown();
        }

        /// <summary>
        /// Allows to await certain result from dialog.
        /// The dialog can yield the result by using <see cref="SetResult"/> or <see cref="CancelResult"/> methods.
        /// </summary>
        /// <typeparam name="TResult">Awaited return type, it will be checked when dialog calls <see cref="Show"/></typeparam>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Awaited result</returns>
        /// <exception cref="TaskCanceledException">Throws this exception when result gets cancelled by either <see cref="token"/> or <see cref="CancelResult"/> method</exception>
        public async Task<TResult> AwaitResult<TResult>(CancellationToken token = default)
        {
            try
            {
                if (_resultCompletionSource != null)
                    return (TResult)await _resultCompletionSource.Task;

                Show();
                AwaitedResultType = typeof(TResult);
                _resultCompletionSource = new TaskCompletionSource<object>();
                _resultCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
                using (token.Register(() => _resultCompletionSource.SetCanceled()))
                {
                    return (TResult)await _resultCompletionSource.Task;
                }
            }
            finally
            {
                AwaitedResultType = null;
                _resultCompletionSource = null;
                _resultCancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Sets dialog invocation result.
        /// Completes the task awaited in <see cref="AwaitResult{TResult}"/>.
        /// </summary>
        /// <param name="result">The object to return to the caller. It should be of <see cref="AwaitedResultType"/> type.</param>
        /// <exception cref="ArgumentException">Thrown when given <see cref="result"/> doesn't match <see cref="AwaitedResultType"/></exception>
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
            _resultCancellationTokenSource?.Cancel();
            _resultCancellationTokenSource = null;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (CustomDialogConfig?.IsCancellable ?? true)
                ParentContainerViewController.TappedOutsideTheDialog += HideDialogOnOutsideTap;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetCommands();
            SetStyles();
            SetLocale();
            InitBindings();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            if (Bindings.Any())
            {
                foreach (var b in Bindings)
                    b.Detach();
                Bindings.Clear();
            }
        }

        public virtual void SetCommands()
        {
        }

        public virtual void SetStyles()
        {
        }

        public virtual void SetLocale()
        {
        }

        public abstract void InitBindings();
    }
}