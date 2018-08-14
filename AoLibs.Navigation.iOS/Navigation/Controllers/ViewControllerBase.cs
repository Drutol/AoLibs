using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AoLibs.Navigation.Core.Interfaces;
using CoreGraphics;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation.Controllers
{
    public abstract class ViewControllerBase<TViewModel> : ArgumentNavigationViewControler 
        where TViewModel : class
    {
        protected List<Binding> Bindings { get; } = new List<Binding>();

        public TViewModel ViewModel { get; protected set; }

        protected ViewControllerBase(IntPtr handle) 
            : base(handle)
        {
            ViewModel = ViewModelResolver?.Resolve<TViewModel>();
        }

        protected ViewControllerBase(string name, NSBundle p) 
            : base(name, p)
        {
            ViewModel = ViewModelResolver?.Resolve<TViewModel>();
        }
    
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetCommands();
            SetStyles();
            SetLocale();
            InitBindings();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            if (Bindings.Any())
            {
                foreach (var b in Bindings)
                    b.Detach();
                Bindings.Clear();
            }
        }

        public virtual void SetCommands()
        {
        }

        public virtual void SetStyles()
        {
        }

        public virtual void SetLocale()
        {
        }

        public abstract void InitBindings();
    }
}