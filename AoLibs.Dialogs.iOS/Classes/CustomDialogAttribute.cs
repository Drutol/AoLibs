using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    /// <summary>
    /// Attribute used to mark the backing storyboard of ViewController.
    /// Used by <see cref="StoryboardOneshotDialogProvider{TPage}"/> to instantiate ViewController.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomDialogAttribute : Attribute
    {
        public enum PageProvider
        {
            Oneshot,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogAttribute"/> class.
        /// </summary>
        /// <param name="page">Integer value of your TPageIdentifier enum.</param>
        /// <param name="storyboardName">The name of the storyboard fro which to retrieve view controller.</param>
        public CustomDialogAttribute(int page, string storyboardName)
        {
            Page = page;
            StoryboardName = storyboardName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogAttribute"/> class.
        /// </summary>
        /// <param name="page">Integer value of your TPageIdentifier enum.</param>
        /// <param name="storyboardName">The name of the storyboard fro which to retrieve view controller.</param>
        /// <param name="viewControllerIdentifier">Identifier within storyboard of desired view controller.</param>
        public CustomDialogAttribute(int page, string storyboardName, string viewControllerIdentifier)
        {
            Page = page;
            StoryboardName = storyboardName;
            ViewControllerIdentifier = viewControllerIdentifier;
        }

        /// <summary>
        /// Gets integer value of specified TDialogIndex enum.
        /// </summary>
        /// <value>
        /// Integer value of specified TDialogIndex enum.
        /// </value>
        public int Page { get; }

        /// <summary>
        /// Gets which provider to use. Currently only one available.
        /// </summary>
        public PageProvider PageProviderType { get; } = PageProvider.Oneshot;

        /// <summary>
        /// Gets or sets the name of the storyboard that will be used for ViewController instantiation. 
        /// </summary>
        public string StoryboardName { get; set; }

        /// <summary>
        /// Gets or sets the name of ViewController associated with the page given that there are multiple ViewControllers defined within storyboard. Leave default if there's only initial ViewController.
        /// </summary>
        public string ViewControllerIdentifier { get; set; }
    }
}