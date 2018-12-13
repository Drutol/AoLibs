using System;
using System.Collections.Generic;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Dialogs.iOS;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.iOS.Navigation;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.Statics;
using Autofac;
using UIKit;

namespace AoLibs.Sample.iOS.ViewControllers
{
    public partial class RootNavigationViewController : UINavigationController
    {
        public RootNavigationViewController (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitalizeNavigation();
            InitalizeDialogs();

            ViewModelLocator.MainViewModel.Initialize();
        }

        private void InitalizeNavigation()
        {
            var navigationManager = new NavigationManager<PageIndex>(this, new ViewModelResolver());
            AppDelegate.Instance.NavigationManager = navigationManager;
        }

        private void InitalizeDialogs()
        {
            var dialogDefinitions = new Dictionary<DialogIndex, ICustomDialogProvider>
            {
                {DialogIndex.TestDialogA, new StoryboardOneshotDialogProvider<TestDialogAViewController>()},
                {DialogIndex.TestDialogB, new StoryboardOneshotDialogProvider<TestDialogBViewController>()},
            };

            var dialogManager = new CustomDialogsManager<DialogIndex>(
                this,
                dialogDefinitions,
                new ViewModelResolver());

            AppDelegate.Instance.DialogsManager = dialogManager;
        }

        private class ViewModelResolver : IViewModelResolver, ICustomDialogViewModelResolver
        {
            TViewModel IViewModelResolver.Resolve<TViewModel>()
            {
                using (var scope = ResourceLocator.ObtainScope())
                {
                    return scope.Resolve<TViewModel>();
                }
            }

            public TViewModel Resolve<TViewModel>()
                where TViewModel : CustomDialogViewModelBase
            {
                using (var scope = ResourceLocator.ObtainScope())
                {
                    return scope.Resolve<TViewModel>();
                }
            }
        }
    }
}