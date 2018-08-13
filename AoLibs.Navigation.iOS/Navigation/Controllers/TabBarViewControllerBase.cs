using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.iOS.Navigation.Controllers
{
    public abstract class TabBarViewControllerBase<TViewModel> : ArgumentNavigationTabBarViewController where TViewModel : class 
    {
        public TViewModel ViewModel { get; protected set; }

        public TabBarViewControllerBase(IntPtr handle) 
            : base(handle)
        {
            ViewModel = ViewModelResolver?.Resolve<TViewModel>();
        }    
    }
}