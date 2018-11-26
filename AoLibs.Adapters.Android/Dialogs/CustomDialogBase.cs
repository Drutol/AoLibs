using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Core.Dialogs;
using GalaSoft.MvvmLight.Helpers;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace AoLibs.Adapters.Android.Dialogs
{
    public abstract class CustomDialogBase : DialogFragment, ICustomDialogForViewModel
    {
        internal static ICustomDialogViewModelResolver CustomDialogViewModelResolver { get; set; }
        internal static FragmentManager FragmentManager { get; set; }

        /// <summary>
        /// Used to indicate whether this fragment went through whole initialization procedure.
        /// </summary>
        private bool _initialized;

        public object Parameter { get; set; }

        protected Type AwaitedResultType { get; private set; }
        private TaskCompletionSource<object> _resultCompletionSource;
        private CancellationTokenSource _cts;

        protected List<Binding> Bindings { get; } = new List<Binding>();
        protected View RootView { get; private set; }

        protected CustomDialogConfig  CustomDialogConfig { get; set; }

        /// <summary>
        /// Gets the layot Id.
        /// Defines which resource Id use to inflate the view.
        /// </summary>
        public abstract int LayoutResourceId { get; }

        public sealed override Context Context
        {
            get
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                    return Activity;
                return base.Context;
            }
        }

        protected abstract void InitBindings();

        /// <summary>
        /// Gets application's Theme.
        /// </summary>
        public Resources.Theme Theme => Activity.Theme;

        /// <summary>
        /// Utility shorthand to FindViewById on current view.
        /// </summary>
        /// <param name="id">View Id.</param>
        /// <typeparam name="T">The type od the View behind the Id.</typeparam>
        protected T FindViewById<T>(int id)
            where T : View
        {
            return RootView.FindViewById<T>(id);
        }

        public override void OnStart()
        {
            base.OnStart();
            if (CustomDialogConfig.StretchHorizontally || CustomDialogConfig.StretchVertically)
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

        public void Show()
        {
            Show(FragmentManager, GetType().Name);
        }

        public void Hide()
        {
            DismissAllowingStateLoss();
        }    

        public Task ShowAsync()
        {
            Show();
            return Task.CompletedTask;
        }

        public Task HideAsync()
        {
            Hide();
            return Task.CompletedTask;
        }

        public async Task<TResult> AwaitResult<TResult>(CancellationToken token = default)
        {
            AwaitedResultType = typeof(TResult);
            _resultCompletionSource = new TaskCompletionSource<object>();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            using (token.Register(() => _resultCompletionSource.SetCanceled()))
            {
                var result = await _resultCompletionSource.Task;
                return (TResult)result;
            }
        }

        public void SetResult(object result)
        {
            if (AwaitedResultType != result.GetType())
                throw new ArgumentException($"Result should be of {AwaitedResultType.Name} type.");

            _resultCompletionSource.SetResult(result);
        }

        public void CancelResult()
        {
            _cts.Cancel();
        }      
    }
}