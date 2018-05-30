using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AoLibs.Navigation.Core.Interfaces;
using CoreGraphics;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation
{
    public abstract class ViewControllerBase : ArgumentNavigationViewControler
    {
        public static IViewModelResolver ViewModelResolver { get; set; }

        private readonly Dictionary<UIButton, EventHandler> _handlersDictionary =
            new Dictionary<UIButton, EventHandler>();

        protected ViewControllerBase(IntPtr handle) : base(handle)
        {
        }

        protected ViewControllerBase(string name, NSBundle p) : base(name, p)
        {
        }

        protected List<Binding> Bindings { get; } = new List<Binding>();

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            SetEditionEndingRecognizer(this);
		}

		public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetCommands();
            SetStyles();
            SetLocale();
            SetBindings();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ReleaseCommands();
            if (Bindings.Any())
            {
                foreach (var b in Bindings)
                    b.Detach();
                Bindings.Clear();
            }
        }

        public void RefreshBindings()
        {
            foreach (var b in Bindings)
                b.ForceUpdateValueFromSourceToTarget();
        }

        protected async void ShowPopup(UIViewController popup)
        {
            DismissModalViewController(false);
            popup.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            popup.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            await PresentViewControllerAsync(popup, true);
        }

        protected void RegisterButtonCommand(UIButton view, EventHandler handler)
        {
            view.TouchUpInside += handler;
            _handlersDictionary.Add(view, handler);
        }

        protected void RegisterButtonCommand(UIButton view, ICommand command)
        {
            var handler = new EventHandler((sender, args) => command.Execute(null));
            RegisterButtonCommand(view, handler);
        }

        public virtual void ReleaseCommands()
        {
            foreach (var keyValuePair in _handlersDictionary)
                keyValuePair.Key.TouchUpInside -= keyValuePair.Value;
            _handlersDictionary.Clear();
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

        public abstract void SetBindings();

        public void SetEditionEndingRecognizer(UIViewController view)
        {
            view.View.AddGestureRecognizer(new UITapGestureRecognizer((p) =>
            {
                p.View.EndEditing(true);
                SlideDown();
            })
            { CancelsTouchesInView = false });
        }

        private volatile bool _slidedUp;

        public void AddKeyboardListener(UITextField textField)
        {
            textField.Started += (sender, e) => SlideUp();
            textField.Ended += (sender, e) => SlideUp();
        }

        public void AddKeyboardListener(UITextView textView)
        {
            textView.Started += (sender, e) => SlideUp();
            textView.Ended += (sender, e) => SlideUp();
        }

        nfloat initalY;
        private void SlideUp(UIView target = null)
        {
            if (!_slidedUp)
            {
                UIView.BeginAnimations("");
                UIView.SetAnimationDuration(0.3);

                var v = (this as UIViewController).View;
                if (target != null)
                    v = target;
                
                initalY = v.Frame.Y;
                v.Frame = new CGRect(0, -140, v.Frame.Width, v.Frame.Height);
                (this as UIViewController).View.LayoutIfNeeded();
                UIView.CommitAnimations();
                _slidedUp = true;
            }
        }

        private void SlideDown(UIView target = null)
        {
            if (_slidedUp)
            {
                UIView.BeginAnimations("");
                UIView.SetAnimationDuration(0.2);

                var v = (this as UIViewController).View;
                if (target != null)
                    v = target;

                v.Frame = new CGRect(0, initalY, v.Frame.Width, v.Frame.Height);
                (this as UIViewController).View.LayoutIfNeeded();
                UIView.CommitAnimations();
                _slidedUp = false;
            }
        }
    }
}