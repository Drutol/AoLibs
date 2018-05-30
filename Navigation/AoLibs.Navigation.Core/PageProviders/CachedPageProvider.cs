using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core.PageProviders
{
    public class CachedPageProvider<TPage> : IPageProvider<TPage> where TPage : class, INavigationPage
    {
        private TPage _page;

        public CachedPageProvider()
        {
        }

        public CachedPageProvider(TPage instance)
        {
            Page = instance;
        }

        public void ForceReinstantination()
        {
            Page = Activator.CreateInstance<TPage>();
            Page.PageIdentifier = PageIdentifier;
        }

        public Type PageType { get; } = typeof(TPage);

        public TPage Page
        {
            get
            {
                if (_page == null)
                {
                    _page = _page = Activator.CreateInstance<TPage>();
                    _page.PageIdentifier = PageIdentifier;
                }

                return _page;
            }
            private set => _page = value;
        }

        public object PageIdentifier { get; set; }
    }
}