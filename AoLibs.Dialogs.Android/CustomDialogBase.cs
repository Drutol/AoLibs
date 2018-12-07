using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Renderscripts;
using Android.Support.V4.App;
using Android.Views;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using GalaSoft.MvvmLight.Helpers;
using Type = System.Type;

namespace AoLibs.Dialogs.Android
{
    /// <summary>
    /// Base android implementation of <see cref="ICustomDialog"/>
    /// </summary>
    public abstract class CustomDialogBase : DialogFragment, ICustomDialogForViewModel
    {
        internal static ICustomDialogViewModelResolver CustomDialogViewModelResolver { get; set; }
        internal static FragmentManager ConfiguredFragmentManager { get; set; }
    
        /// <summary>
        /// Token source used to monitor <see cref="AwaitResult{TResult}"/> process.
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// Completion source being the lifeline of <see cref="AwaitResult{TResult}"/> process.
        /// </summary>
        private TaskCompletionSource<object> _resultCompletionSource;

        /// <summary>
        /// Used to indicate whether this fragment went through whole initialization procedure.
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// Method used to populate <see cref="Bindings"/> collection, called once when <see cref="RootView"/> is ready.
        /// </summary>
        protected abstract void InitBindings();

        /// <summary>
        /// Callback when dialog has been dismissed.
        /// </summary>
        protected virtual void OnDismissed()
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
            FragmentTag = GetType().Name;
        }

        /// <summary>
        /// Gets tag used to identify fragments.
        /// </summary>
        private string FragmentTag { get; }

        /// <summary>
        /// Gets awaited type passed using <see cref="ICustomDialog.AwaitResult{TResult}"/>
        /// </summary>
        protected Type AwaitedResultType { get; private set; }

        /// <summary>
        /// Gets of all bindings defined in dialog.
        /// </summary>
        protected List<Binding> Bindings { get; } = new List<Binding>();

        /// <summary>
        /// Gets root dialog view inflated from <see cref="LayoutResourceId"/>
        /// </summary>
        protected View RootView { get; private set; }

        /// <summary>
        /// Gets or sets dialog config used when creating the dialog.
        /// </summary>
        protected CustomDialogConfig CustomDialogConfig { get; set; } = new CustomDialogConfig();

        /// <summary>
        /// Gets the layout Id.
        /// Defines which resource Id use to inflate the view.
        /// </summary>
        protected abstract int LayoutResourceId { get; }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        public sealed override Context Context
        {
            get
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                    return Activity;
                return base.Context;
            }
        }

        /// <summary>
        /// Gets application's Theme.
        /// Just a shortcut to <see cref="Resources.Theme"/> contained in parent activity.
        /// </summary>
        public new global::Android.Content.Res.Resources.Theme Theme => Activity.Theme;

        /// <summary>
        /// Gets or sets the parameter the dialog was called with.
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Shows the dialog with given parameter.
        /// </summary>
        /// <param name="parameter">Parameter which can be passed to dialog's ViewModel.</param>
        public void Show(object parameter = null)
        {
            Parameter = parameter;
            Show(ConfiguredFragmentManager, FragmentTag);
            OnShown();
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        public void Hide()
        {
            DismissAllowingStateLoss();
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

        /// <inheritdoc />
        public sealed override void DismissAllowingStateLoss()
        {
            CancelResult();
            base.DismissAllowingStateLoss();
            OnDismissed();
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
                    return (TResult) await _resultCompletionSource.Task;

                Show();
                AwaitedResultType = typeof(TResult);
                _resultCompletionSource = new TaskCompletionSource<object>();
                _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                using (token.Register(() => _resultCompletionSource.SetCanceled()))
                {
                    return (TResult) await _resultCompletionSource.Task;
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
            _cts?.Cancel();
            _cts = null;
        }

        /// <summary>
        ///     Utility shorthand to FindViewById on current view.
        /// </summary>
        /// <param name="id">View Id.</param>
        /// <typeparam name="T">The type od the View behind the Id.</typeparam>
        protected T FindViewById<T>(int id)
            where T : View
        {
            return RootView.FindViewById<T>(id);
        }
 
        /// <inheritdoc />
        public override void OnStart()
        {
            base.OnStart();
            if (CustomDialogConfig != null &&
                (CustomDialogConfig.StretchHorizontally || CustomDialogConfig.StretchVertically))
            {
#pragma warning disable SA1118 // Parameter must not span multiple lines
                Dialog.Window.SetLayout(
                    CustomDialogConfig.StretchHorizontally
                        ? ViewGroup.LayoutParams.MatchParent
                        : ViewGroup.LayoutParams.WrapContent,
                    CustomDialogConfig.StretchVertically
                        ? ViewGroup.LayoutParams.MatchParent
                        : ViewGroup.LayoutParams.WrapContent);
#pragma warning restore SA1118 // Parameter must not span multiple lines
            }
        }

        /// <inheritdoc />
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (RootView == null)
            {
                RootView = inflater.Inflate(LayoutResourceId, container, false);

                if (CustomDialogConfig != null)
                {
                    Cancelable = CustomDialogConfig.IsCancellable;
                    Dialog.Window.SetGravity(GetGravityFromConfig(CustomDialogConfig.Gravity));
                }
            }

            // if bindings are present for this view we won't generate new ones, if it's first creation we have to do this anyway
            if (!_initialized || !Bindings.Any())
                InitBindings();

            _initialized = true;

            return RootView;
        }

        private GravityFlags GetGravityFromConfig(CustomDialogConfig.DialogGravity gravity)
        {
            var outputGravity = GravityFlags.NoGravity;

            if ((gravity & CustomDialogConfig.DialogGravity.Bottom) == CustomDialogConfig.DialogGravity.Bottom)
                outputGravity |= GravityFlags.Bottom;
            if ((gravity & CustomDialogConfig.DialogGravity.Center) == CustomDialogConfig.DialogGravity.Center)
                outputGravity |= GravityFlags.Center;
            if ((gravity & CustomDialogConfig.DialogGravity.Left) == CustomDialogConfig.DialogGravity.Left)
                outputGravity |= GravityFlags.Left;
            if ((gravity & CustomDialogConfig.DialogGravity.Right) == CustomDialogConfig.DialogGravity.Right)
                outputGravity |= GravityFlags.Right;
            if ((gravity & CustomDialogConfig.DialogGravity.Top) == CustomDialogConfig.DialogGravity.Top)
                outputGravity |= GravityFlags.Top;

            return outputGravity;
        }
    }
}