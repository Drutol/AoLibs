using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    [Preserve(AllMembers = true)]
    public class UriLauncherAdapter : IUriLauncherAdapter
    {
        public void LaunchUri(Uri uri)
        {
            var i = new Intent(Intent.ActionView);
            i = i.AddFlags(ActivityFlags.NewTask);
            i = i.SetData(global::Android.Net.Uri.Parse(uri.ToString()));
            Application.Context.StartActivity(i);
        }
    }
}