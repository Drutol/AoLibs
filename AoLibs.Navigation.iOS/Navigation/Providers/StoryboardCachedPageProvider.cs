using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.Core.PageProviders;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using Foundation;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation.Providers
{
    public class StoryboardCachedPageProvider<TPage> : CachedPageProvider<TPage> 
        where TPage : class, INavigationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardCachedPageProvider{TPage}"/> class.
        /// Builds ViewController based on data contained in <see cref="NavigationPageAttribute"/> attached to <see cref="TPage"/>.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        public StoryboardCachedPageProvider()
        {
            var attr = typeof(TPage).GetTypeInfo().GetCustomAttribute<NavigationPageAttribute>();
            SetUpFactory(attr.StoryboardName, attr.ViewControllerIdentifier);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardCachedPageProvider{TPage}"/> class.
        /// Builds ViewController from given string parameters.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        /// <param name="storyboardName">Name of the storyboard file.</param>
        /// <param name="viewControllerIdentifier">Name of the controller within the storyboard.</param>
        public StoryboardCachedPageProvider(string storyboardName, string viewControllerIdentifier)
        {
            SetUpFactory(storyboardName, viewControllerIdentifier);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardCachedPageProvider{TPage}"/> class.
        /// Creates new instance setting up the provider with provided page.
        /// </summary>
        /// <param name="instance">Page to be used by provider.</param>
        /// <param name="factory">Optional factory to reinstantinate the page if need araises. <see cref="Activator.CreateInstance{T}"/> will be used if null.</param>
        public StoryboardCachedPageProvider(TPage instance, Func<TPage> factory = null) 
            : base(instance, factory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotPageProvider{TPage}"/> class.
        /// Extracts data from <see cref="attr"/> to prepare the provider.
        /// </summary>
        /// <param name="attr">Page attribute.</param>
        public StoryboardCachedPageProvider(NavigationPageAttribute attr)
        {
            SetUpFactory(attr.StoryboardName, attr.ViewControllerIdentifier);
        }

        private void SetUpFactory(string storyboardName, string viewControllerIdentifier)
        {
            if (string.IsNullOrEmpty(viewControllerIdentifier))
            {
                Factory = () => UIStoryboard.FromName(storyboardName, null)
                    .InstantiateInitialViewController() as TPage;
            }
            else
            {
                Factory = () =>
                    UIStoryboard.FromName(storyboardName, null)
                        .InstantiateViewController(viewControllerIdentifier) as TPage;
            }
        }
    }
}