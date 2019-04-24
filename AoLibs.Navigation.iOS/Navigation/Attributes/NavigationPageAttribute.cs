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
        /// Initializes a new instance of the <see cref="NavigationPageAttribute"/> class.
        /// </summary>
        /// <param name="page">Integer value of your TPageIdentifier enum.</param>
        /// <param name="pageProvider">Indicates whether to use <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.</param>
        public NavigationPageAttribute(int page, PageProvider pageProvider = PageProvider.Cached)
        {
            Page = page;
            PageProviderType = pageProvider;
        }    
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPageAttribute"/> class.
        /// </summary>
        /// <param name="page">Object that will be casted to <see cref="int"/>.</param>
        /// <param name="pageProvider">Indicates whether to use <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.</param>
        public NavigationPageAttribute(object page, PageProvider pageProvider = PageProvider.Cached)
        {
            Page = (int)page;
            PageProviderType = pageProvider;
        }

        /// <summary>
        /// Gets integer value of specified TPageIdentifier enum.
        /// </summary>
        /// <value>
        /// Integer value of specified TPageIdentifier enum.
        /// </value>
        public int Page { get; }

        /// <summary>
        /// Gets which provider to use, <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.
        /// </summary>
        /// <value>
        /// Which provider to use, <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.
        /// </value>
        public PageProvider PageProviderType { get; }

        /// <summary>
        /// Gets or sets the name of the storyboard that will be used for ViewController instantination. 
        /// </summary>
        /// <value>
        /// The name of the storyboard that will be used for ViewController instantination.
        /// </value>
        public string StoryboardName { get; set; }

        /// <summary>
        /// Gets or sets the name of ViewController associated with the page given that there are multiple ViewContollers defined within storyboad. Leave default if there's only initial ViewController.
        /// </summary>
        /// <value>
        /// The name of ViewController associated with the page given that there are multiple ViewContollers defined within storyboad. Leave default if there's only initial ViewController.
        /// </value>
        public string ViewControllerIdentifier { get; set; }
    }
}