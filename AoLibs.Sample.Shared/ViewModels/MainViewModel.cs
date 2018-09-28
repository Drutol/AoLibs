using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Shared.Models;
using GalaSoft.MvvmLight;

namespace AoLibs.Sample.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        public MainViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void Initialize()
        {
            _navigationManager.Navigate(PageIndex.PageA);
        }
    }
}
