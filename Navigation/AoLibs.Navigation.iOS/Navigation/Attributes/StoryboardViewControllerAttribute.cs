using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Navigation.iOS.Navigation.Providers;
using Foundation;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation.Attributes
{
    /// <summary>
    /// Attribute used to mark the backing storyboard of ViewController.
    /// Used by <see cref="StoryboardOneshotPageProvider{TPage}"/> and <see cref="StoryboardCachedPageProvider{TPage}"/> to instantinate ViewController.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StoryboardViewControllerAttribute : Attribute
    {
        public string StoryboardName { get; set; }
        public string ViewControllerIdentifier { get; set; }
    }
}