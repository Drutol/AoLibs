using System;
using System.Threading;
using AoLibs.Navigation.Core.Interfaces;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation.Controllers
{
    public class ArgumentNavigationTabBarViewController : UITabBarController, INavigationPage, INativeNavigationPage
    {
        internal static IViewModelResolver ViewModelResolver { get; set; }

        private event EventHandler NativeBackNavigation;

        public object PageIdentifier { get; set; }
        public object NavigationArguments { get; set; }

        event EventHandler INativeNavigationPage.NativeBackNavigation
        {
            add
            {
                if (NativeBackNavigation == null)
                    NativeBackNavigation += value;
            }
            remove => NativeBackNavigation -= value;
        }

        protected ArgumentNavigationTabBarViewController(IntPtr handle) 
            : base(handle)
        {
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

        public override void ViewWillDisappear(bool animated)
        {
            if (IsMovingFromParentViewController || IsBeingDismissed)
                NativeBackNavigation?.Invoke(this, EventArgs.Empty);
            base.ViewWillDisappear(animated);
        }
    }
}