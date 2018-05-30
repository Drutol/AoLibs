using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.iOS.Navigation
{
    public abstract class TabBarViewControllerBase : ArgumentNavigationTabBarController
    {
        public static IViewModelResolver ViewModelResolver { get; set; }

        protected TabBarViewControllerBase(IntPtr handle) : base(handle)
        {
        }
    }
}