using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AoLibs.Navigation.Core.Interfaces;
using GalaSoft.MvvmLight.Helpers;
using Fragment = Android.Support.V4.App.Fragment;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace NavigationLib.Android.Navigation
{
    /// <summary>
    /// Base class for library managed pages.
    /// </summary>
    public abstract class NavigationFragmentBase : Fragment, INavigationPage
    {
        /// <summary>
        /// Used to indicate whether this fragment went through whole initialization procedure.
        /// </summary>
        private bool _initialized;
        /// <summary>
        /// Specifies if bindings should be recreated if none have been added.
        /// </summary>
        private readonly bool _hasNonTrackableBindings;
        /// <summary>
        /// Allows <see cref="FragmentBase{TViewModel}"/> to resolve ViewModels automatically.
        /// </summary>
        public static IViewModelResolver ViewModelResolver { get; set; }

        protected List<Binding> Bindings = new List<Binding>();
        protected View RootView { get; private set; }

        public NavigationFragmentBase(bool hasNonTrackableBindings = false)
        {
            _hasNonTrackableBindings = hasNonTrackableBindings;
        }

        /// <inheritdoc />
        public object PageIdentifier { get; set; }
        /// <inheritdoc />
        public object NavigationArguments { get; set; }

        /// <summary>
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

        /// <summary>
        /// Called upon forward navigation.
        /// </summary>
        public virtual void NavigatedTo()
        {

        }

        /// <summary>
        /// Called when navigation occured backwards on the stack.
        /// That is when we went to next page and going back to this one.
        /// </summary>
        public virtual void NavigatedBack()
        {
            
        }

        /// <summary>
        /// Called whenever this page is left.
        /// </summary>
        public virtual void NavigatedFrom()
        {
            
        }

        protected virtual void Init(Bundle savedInstanceState)
        {
            
        }

        protected abstract void InitBindings();

        /// <summary>
        /// Utility shorthand to application's Theme.
        /// </summary>
        public Resources.Theme Theme => Activity.Theme;

        /// <summary>
        /// Utility shorthand to FindViewById on current view.
        /// </summary>
        protected T FindViewById<T>(int id) where T : View
        {            
            return RootView.FindViewById<T>(id);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Init(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {         
            if (RootView == null)
                RootView = inflater.Inflate(LayoutResourceId, container, false);
            if (!_initialized || (!Bindings.Any() && !_hasNonTrackableBindings)) //if bindings are present for this view we won't generate new ones, if it's first creation we have to do this anyway
                InitBindings();

            _initialized = true;

            return RootView;
        }
    }
}