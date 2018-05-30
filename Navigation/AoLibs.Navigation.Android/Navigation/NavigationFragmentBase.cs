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
    public abstract class NavigationFragmentBase : Fragment, INavigationPage
    {
        private readonly bool _hasNonTrackableBindings;
        public static IViewModelResolver ViewModelResolver { get; set; }

        public NavigationFragmentBase(bool hasNonTrackableBindings = false)
        {
            _hasNonTrackableBindings = hasNonTrackableBindings;
        }

        protected List<Binding> Bindings = new List<Binding>();
        private bool _initialized;
        protected virtual View RootView { get; private set; }

        public object PageIdentifier { get; set; }
        public object NavigationArguments { get; set; }
        public bool NavigatingBack { get; set; }

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

        public virtual void NavigatedTo()
        {

        }

        public virtual void NavigatedBack()
        {
            
        }

        public virtual void NavigatedFrom()
        {
            
        }

        public SemaphoreSlim ObtainNavigationSemaphore()
        {
            throw new NotImplementedException();
        }

        protected virtual void Init(Bundle savedInstanceState)
        {
            
        }

        protected abstract void InitBindings();

        public Resources.Theme Theme => Activity.Theme;

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