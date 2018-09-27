using Foundation;
using System;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.iOS.Navigation;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Statics;
using Autofac;
using UIKit;

namespace AoLibs.Sample.iOS
{
    public partial class RootNavigationViewController : UINavigationController
    {
        public RootNavigationViewController (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var manager = new NavigationManager<PageIndex>(this, new ViewModelResolver());

            AppDelegate.Instance.NavigationManager = manager;

            ViewModelLocator.MainViewModel.Initialize();
        }

        class ViewModelResolver : IViewModelResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                using (var scope = ResourceLocator.ObtainScope())
                {
                    return scope.Resolve<TViewModel>();
                }
            }
        }
    }
}