using System;
using Android.OS;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace NavigationLib.Android.Navigation
{
    public abstract class FragmentBase<TViewModel> : NavigationFragmentBase where TViewModel : class
    {
        public FragmentBase(bool hasNonTrackableBindings = false) : base(hasNonTrackableBindings)
        {
            ViewModel = ViewModelResolver?.Resolve<TViewModel>();
        }

        protected TViewModel ViewModel { get; }
    }
}