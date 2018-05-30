using AoLibs.Adapters.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Adapters.iOS
{
    public class PhoneCallAdapter : IPhoneCallAdapter
    {
        public void Call(string telephoneNumber)
        {         
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var url = new NSUrl($"tel:{telephoneNumber}");
                UIApplication.SharedApplication.OpenUrl(url);
            });
        }
    }
}