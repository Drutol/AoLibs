﻿using System;
using System.Threading;
using AoLibs.Navigation.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation
{
    public class ArgumentNavigationViewControler : UIViewController, INavigationPage, INativeNavigationPage
    {
        private SemaphoreSlim _navigationSemaphore;

        private event EventHandler NativeBackNavigation;

        event EventHandler INativeNavigationPage.NativeBackNavigation
        {
            add
            {
                if (NativeBackNavigation == null)
                    this.NativeBackNavigation += value;
            }
            remove { this.NativeBackNavigation -= value; }
        }

        protected ArgumentNavigationViewControler(IntPtr handle) : base(handle)
        {
        }

        protected ArgumentNavigationViewControler(string name, NSBundle p) : base(name, p)
        {
        }

        public object PageIdentifier { get; set; }
        public object NavigationArguments { get; set; }

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
            if(IsMovingFromParentViewController || IsBeingDismissed)
                NativeBackNavigation?.Invoke(this,EventArgs.Empty);
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _navigationSemaphore?.Release();
        }

        public SemaphoreSlim ObtainNavigationSemaphore()
        {
            _navigationSemaphore = new SemaphoreSlim(0);
            return _navigationSemaphore;
        }
    }
}