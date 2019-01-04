using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Dialogs.iOS.Interfaces;
using AoLibs.Dialogs.iOS.Models;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    /// <summary>
    /// Base implementation of <see cref="ICustomDialog"/> for iOS.
    /// </summary>
    public abstract class CustomDialogBase : UIViewController, ICustomDialogForViewModel
    {
        internal static ICustomDialogViewModelResolver CustomDialogViewModelResolver { get; set; }
        internal static UIViewController RootViewController { get; set; }
        internal static IInternalDialogsManager DialogsManager { get; set; }

        /// <summary>
        /// Fired when dialog is fully shown.
        /// </summary>       
        public event EventHandler DialogShown;

        /// <summary>
        /// Fired when dialog is fully hidden
        /// </summary>
        public event EventHandler DialogHidden;

        /// <summary>
        /// Gets a container for view bindings.
        /// </summary>
        protected List<Binding> Bindings { get; } = new List<Binding>();

        /// <summary>
        /// Gets the parent view controller in which the dialog is hosted.
        /// </summary>
        protected DialogViewController ParentContainerViewController { get; private set; }

        /// <summary>
        /// Gets the type that is expected to be passed to <see cref="SetResult"/>.
        /// </summary>
        protected Type AwaitedResultType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the dialog will show with animation.
        /// </summary>
        public virtual DialogAnimationConfig AnimationConfig { get; } = new DialogAnimationConfig();

        /// <summary>
        /// Gets a value indicating whether the dialog will hide with animation.
        /// </summary>
        public virtual DialogBackgroundConfig BackgroundConfig { get; } = new DialogBackgroundConfig();

        /// <summary>
        /// Gets or sets the model that was passed to <see cref="Show"/> method.
        /// </summary>
        public virtual object Parameter { get; set; }

        private SemaphoreSlim _showSemaphore;
        private SemaphoreSlim _hideSemaphore;
        private TaskCompletionSource<object> _resultCompletionSource;
        private CancellationTokenSource _resultCancellationTokenSource;

        /// <summary>
        /// Called when the dialog is ready to create bindings between view and ViewModel.
        /// </summary>
        protected abstract void InitBindings();

        /// <summary>
        /// Gets or sets dialog config used when creating the dialog.
        /// </summary>
        public CustomDialogConfig CustomDialogConfig { get; set; } = new CustomDialogConfig();

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogBase"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected CustomDialogBase(IntPtr handle) 
            : base(handle)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogBase"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="p">Bundle.</param>
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

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public void Show(object parameter = null)
        {        
            Parameter = parameter;
            OnWillBeShown();
            DialogsManager.CurrentlyDisplayedDialog = this;
            RootViewController.PresentViewController(ParentContainerViewController, false, OnDialogPresentationFinished);
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        public void Hide()
        {
            OnWillBeHidden();
            RootViewController.DismissViewController(false, OnDialogDismissFinished);
        }

        /// <summary>
        /// Shows the dialog asynchronously. The task will be completed when the dialog is fully shown.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public Task ShowAsync(object parameter = null)
        {
            _showSemaphore = new SemaphoreSlim(0);
            Show(parameter);
            return _showSemaphore.WaitAsync();
        }

        /// <summary>
        /// Hides the dialog asynchronously. The task will be completed once the dialog has fully disappeared.
        /// </summary>
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
        
        /// <summary>
        /// Callback when dialog started show animation.
        /// </summary>
        protected virtual void OnWillBeShown()
        {
        }  
        
        /// <summary>
        /// Callback when dialog started hide animation.
        /// </summary>
        protected virtual void OnWillBeHidden()
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
            _resultCancellationTokenSource?.Cancel();
            _resultCancellationTokenSource = null;
        }

        /// <inheritdoc />
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (CustomDialogConfig.IsCancellable)
                ParentContainerViewController.TappedOutsideTheDialog += HideDialogOnOutsideTap;

            InitBindings();
        }

        /// <inheritdoc />
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
    }
}
