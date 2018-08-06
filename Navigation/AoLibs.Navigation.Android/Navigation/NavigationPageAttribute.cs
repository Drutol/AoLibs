using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NavigationLib.Android.Navigation
{
    public class NavigationPageAttribute : Attribute
    {
        public enum PageProvider
        {
            Cached,
            Oneshot,
        }

        public int Page { get; set; }
        public PageProvider Type { get; set; }
    }
}