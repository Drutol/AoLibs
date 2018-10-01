using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.Shared.ViewModels
{
    public class TestViewModelB : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        private string _message;

        public TestViewModelB(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigatedTo(PageBNavArgs navArgs)
        {
            Message = navArgs.Message;
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoBackCommand => new RelayCommand(() => { _navigationManager.GoBack(); });

        public RelayCommand NavigateCCommand =>
            new RelayCommand(() => { _navigationManager.Navigate(PageIndex.PageC); });

        public RelayCommand NavigateCNoBackCommand => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.PageC, NavigationBackstackOption.NoBackstack);
        });
    }
}
