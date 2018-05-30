using System;
using GalaSoft.MvvmLight;

namespace AoLibs.Navigation.iOS.Navigation
{
    public abstract class MvvmTabBarController<TViewModel> : TabBarViewControllerBase where TViewModel : ViewModelBase
    {
        public MvvmTabBarController(IntPtr handle) : base(handle)
        {
            _viewModel = ViewModelResolver?.Resolve<TViewModel>();
        }

        public TViewModel _viewModel { get; protected set; }
    }
}