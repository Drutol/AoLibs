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
    public class StoryboardOneshotPageProvider<TPage> : OneshotPageProvider<TPage> where TPage : class, INavigationPage
    {
        /// <summary>
        /// Builds ViewController from given string parameters.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        /// <param name="storyboardName">Name of the storyboard file.</param>
        /// <param name="viewControllerIdentifier">Name of the controller within the storyboard.</param>
        public StoryboardOneshotPageProvider(string storyboardName, string viewControllerIdentifier)
        {
            SetUpFactory(storyboardName, viewControllerIdentifier);
        }

        /// <summary>
        /// Builds ViewController based on data contained in <see cref="StoryboardViewControllerAttribute"/> attached to <see cref="TPage"/>.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        public StoryboardOneshotPageProvider()
        {
            var attr = typeof(TPage).GetTypeInfo().GetCustomAttribute<StoryboardViewControllerAttribute>();
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