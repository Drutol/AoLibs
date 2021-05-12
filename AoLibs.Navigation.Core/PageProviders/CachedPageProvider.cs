using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core.PageProviders
{
    /// <summary>
    /// Page provider that will cache the page it is responsible for.
    /// </summary>
    /// <typeparam name="TPage">The type of concrete naviagtion component.</typeparam>
    public class CachedPageProvider<TPage> : IPageProvider<TPage> 
        where TPage : class, INavigationPage
    {
        protected Func<TPage> Factory { get; set; }
        private TPage _page;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedPageProvider{TPage}"/> class.
        /// Creates new instance, the page will be created using <see cref="Activator.CreateInstance{T}"/>, be sure it can be instantinated this way.
        /// </summary>
        public CachedPageProvider()
        {
            Factory = Activator.CreateInstance<TPage>;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedPageProvider{TPage}"/> class.
        /// Creates new instance setting up the provider with provided page.
        /// </summary>
        /// <param name="instance">Page to be used by provider.</param>
        /// <param name="factory">Optional factory to reinstantinate the page if need araises. <see cref="Activator.CreateInstance{T}"/> will be used if null.</param>
        public CachedPageProvider(TPage instance, Func<TPage> factory = null)
        {
            Factory = factory ?? Activator.CreateInstance<TPage>;
            Page = instance;
        }

        /// <summary>
        /// Creates new page instance in place of the old one.
        /// </summary>
        public void ForceReinstantination()
        {
            Page.Dispose();
            Page = Factory();
            OnPageCreated(Page);
            Page.PageIdentifier = PageIdentifier;
        }

        /// <summary>
        /// Gets actual type of held page.
        /// </summary>
        public Type PageType { get; } = typeof(TPage);

        /// <summary>
        /// Gets or sets instance of held page.
        /// </summary>
        public TPage Page
        {
            get
            {
                if (_page == null)
                {
                    _page = Factory();
                    _page.PageIdentifier = PageIdentifier;
                    OnPageCreated(_page);
                }

                return _page;
            }

            set => _page = value;
        }

        /// <summary>
        /// Gets or sets current TPageIdentifier hidden beyond <see cref="object"/>.
        /// </summary>
        public object PageIdentifier { get; set; }

        protected virtual void OnPageCreated(TPage page)
        {
        }
    }
}