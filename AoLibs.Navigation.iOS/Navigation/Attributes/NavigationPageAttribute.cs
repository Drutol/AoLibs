using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Navigation.Core.PageProviders;
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
    public class NavigationPageAttribute : Attribute
    {
        public enum PageProvider
        {
            Cached,
            Oneshot,
        }

        /// <summary>
        /// Initializes new page attribute.
        /// </summary>
        /// <param name="page">Integer value of your TPageIdentifier enum.</param>
        /// <param name="pageProvider">Indicates whether to use <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.</param>
        public NavigationPageAttribute(int page, PageProvider pageProvider)
        {
            Page = page;
            PageProviderType = pageProvider;
        }

        /// <summary>
        /// Integer value of your TPageIdentifier enum.
        /// </summary>
        public int Page { get;  }
        /// <summary>
        /// Indicates whether to use <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.
        /// </summary>
        public PageProvider PageProviderType { get;  }

        /// <summary>
        /// Allows to specify the name of the storyboard that will be used for ViewController instantination. 
        /// </summary>
        public string StoryboardName { get; set; }
        /// <summary>
        /// Specifies the name of ViewController associated with the page given that there are multiple ViewContollers defined within storyboad. Leave default if there's only initial ViewController.
        /// </summary>
        public string ViewControllerIdentifier { get; set; }
    }
}