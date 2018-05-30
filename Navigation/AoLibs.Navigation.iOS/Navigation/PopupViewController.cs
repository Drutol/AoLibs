using System;
using Foundation;

namespace AoLibs.Navigation.iOS.Navigation
{
    public abstract class PopupViewController : ViewControllerBase
    {
        protected readonly ViewControllerBase Parent;

        public PopupViewController(IntPtr handle) : base(handle)
        {
        }

        public PopupViewController(string name, NSBundle p, ViewControllerBase parent) : base(name, p)
        {
            Parent = parent;
        }

        public virtual void Hide()
        {
            Parent.DismissViewController(true, null);
        }
    }
}