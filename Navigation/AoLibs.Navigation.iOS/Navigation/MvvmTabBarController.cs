﻿using System;

namespace AoLibs.Navigation.iOS.Navigation
{
    public abstract class MvvmTabBarController<TViewModel> : TabBarViewControllerBase where TViewModel : class
    {
        public MvvmTabBarController(IntPtr handle) : base(handle)
        {
            _viewModel = ViewModelResolver?.Resolve<TViewModel>();
        }

        public TViewModel _viewModel { get; protected set; }
    }
}