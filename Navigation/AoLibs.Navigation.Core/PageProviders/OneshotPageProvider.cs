using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core.PageProviders
{
    public class OneshotPageProvider<TPage> : IPageProvider<TPage> where TPage : INavigationPage
    {
        private readonly Func<TPage> _factory;

        public OneshotPageProvider(Func<TPage> factory)
        {
            _factory = new Func<TPage>(() =>
            {
                var page = factory.Invoke();
                page.PageIdentifier = PageIdentifier;
                return page;
            });
        }

        public OneshotPageProvider()
        {
            _factory = new Func<TPage>(() =>
            {
                var page = Activator.CreateInstance<TPage>();
                page.PageIdentifier = PageIdentifier;
                return page;
            });
        }

        public void ForceReinstantination()
        {
        }

        public Type PageType { get; } = typeof(TPage);

        public TPage Page => _factory.Invoke();

        public object PageIdentifier { get; set; }
    }
}