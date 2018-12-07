using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Dialogs.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    public class CustomDialogBase : UIViewController, ICustomDialog
    {
        internal static ICustomDialogViewModelResolver CustomDialogViewModelResolver { get; set; }
        internal static UIViewController RootViewController { get; set; }

        public object Parameter { get; set; }

        protected CustomDialogBase(IntPtr handle)
            : base(handle)
        {
        }

        protected CustomDialogBase(string name, NSBundle p)
            : base(name, p)
        {
        }

        public void Show(object parameter = null)
        {
            throw new NotImplementedException();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        public Task ShowAsync(object parameter = null)
        {
            throw new NotImplementedException();
        }

        public Task HideAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> AwaitResult<TResult>(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}