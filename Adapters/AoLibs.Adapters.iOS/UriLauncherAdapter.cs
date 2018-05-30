using System;
using AoLibs.Adapters.Core.Interfaces;
using Foundation;

namespace AoLibs.Adapters.iOS
{
    public class UriLauncherAdapter : IUriLauncherAdapter
    {
        public void LaunchUri(Uri uri)
        {
            UIKit.UIApplication.SharedApplication.OpenUrl(new NSUrl(uri.ToString()));
        }

    }
}
