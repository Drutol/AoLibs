using System;
using AoLibs.Navigation.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation.Controllers
{
    public class ArgumentNavigationViewControler : UIViewController, INavigationPage, INativeNavigationPage
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

        protected ArgumentNavigationViewControler(IntPtr handle) 
            : base(handle)
        {
        }

        protected ArgumentNavigationViewControler(string name, NSBundle p)
            : base(name, p)
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