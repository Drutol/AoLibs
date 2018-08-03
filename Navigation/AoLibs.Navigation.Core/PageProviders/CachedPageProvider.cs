using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core.PageProviders
{
    /// <summary>
    /// Page provider that will cache the page it is responsible for.
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    public class CachedPageProvider<TPage> : IPageProvider<TPage> where TPage : class, INavigationPage
    {
        private readonly Func<TPage> _factory;
        private TPage _page;

        /// <summary>
        /// Creates new instance, the page will be created using <see cref="Activator.CreateInstance{T}"/>, be sure it can be instantinated this way.
        /// </summary>
        public CachedPageProvider()
        {
        }

        /// <summary>
        /// Creates new instance setting up the provider with provided page.
        /// </summary>
        /// <param name="instance">Page to be used by provider.</param>
        /// <param name="factory">Optional factory to reinstantinate the page if need araises. <see cref="Activator.CreateInstance{T}"/> will be used if null.</param>
        public CachedPageProvider(TPage instance, Func<TPage> factory = null)
        {
            _factory = factory ?? Activator.CreateInstance<TPage>;
            Page = instance;
        }

        /// <summary>
        /// Creates new page instance in place of the old one.
        /// </summary>
        public void ForceReinstantination()
        {
            Page = _factory();
            Page.PageIdentifier = PageIdentifier;
        }

        /// <summary>
        /// The actual type of held page.
        /// </summary>
        public Type PageType { get; } = typeof(TPage);

        /// <summary>
        /// Actual instance of held page.
        /// </summary>
        public TPage Page
        {
            get
            {
                if (_page == null)
                {
                    _page = _factory();
                    _page.PageIdentifier = PageIdentifier;
                }

                return _page;
            }
            private set => _page = value;
        }

        /// <summary>
        /// Current TPageIdentifier hidden beyond <see cref="Object"/>
        /// </summary>
        public object PageIdentifier { get; set; }
    }
}