using System;
using GalaSoft.MvvmLight;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation
{
    public abstract class MvvmViewController<TViewModel> : ViewControllerBase where TViewModel : ViewModelBase
    {
        public MvvmViewController(IntPtr handle) : base(handle)
        {
            _viewModel = ViewModelResolver?.Resolve<TViewModel>();
        }

        public MvvmViewController(string name) : base(name, null)
        {
            _viewModel = ViewModelResolver?.Resolve<TViewModel>();
        }

        public TViewModel _viewModel { get; protected set; }

		public override void ViewWillAppear(bool animated)
        {
            UIApplication.SharedApplication.KeyWindow.BackgroundColor = UIColor.White;
            base.ViewWillAppear(animated);
		}
	}
}