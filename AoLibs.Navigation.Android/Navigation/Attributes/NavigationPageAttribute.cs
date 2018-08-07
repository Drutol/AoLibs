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
using AoLibs.Navigation.Core.PageProviders;

namespace NavigationLib.Android.Navigation
{
    /// <summary>
    /// Attribute used by <see cref="NavigationManager{TPageIdentifier}"/> to index all pages and initialize navigation with them.
    /// </summary>
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
        public int Page { get; }
        /// <summary>
        /// Indicates whether to use <see cref="CachedPageProvider{TPage}"/> or <see cref="OneshotPageProvider{TPage}"/> when creating page entries.
        /// </summary>
        public PageProvider PageProviderType { get; }
    }
}