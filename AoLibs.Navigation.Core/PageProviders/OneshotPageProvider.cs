using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core.PageProviders
{
    /// <summary>
    /// Provider which will recreate the bage instance whnever it is requested so every time we start with clean state.
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    public class OneshotPageProvider<TPage> : IPageProvider<TPage> where TPage : INavigationPage
    {
        protected Func<TPage> Factory { get; set; }

        /// <summary>
        /// Creates provicer with specified factory method.
        /// </summary>
        /// <param name="factory">Facotry used to build actual page.</param>
        public OneshotPageProvider(Func<TPage> factory)
        {
            Factory = new Func<TPage>(() =>
            {
                var page = factory.Invoke();
                page.PageIdentifier = PageIdentifier;
                return page;
            });
        }

        /// <summary>
        /// Creates provider instance. <see cref="Activator.CreateInstance{T}"/> will be used for instantination.
        /// </summary>
        public OneshotPageProvider()
        {
            Factory = new Func<TPage>(() =>
            {
                var page = Activator.CreateInstance<TPage>();
                page.PageIdentifier = PageIdentifier;
                return page;
            });
        }

        /// <summary>
        /// Obsolete in this implementation.
        /// </summary>
        public void ForceReinstantination()
        {
        }

        /// <summary>
        /// The actual type of held page.
        /// </summary>
        /// 
        public Type PageType { get; } = typeof(TPage);
        /// <summary>
        /// Actual instance of held page.
        /// </summary>
        public TPage Page => Factory.Invoke();

        /// <summary>
        /// Current TPageIdentifier hidden beyond <see cref="Object"/>
        /// </summary>
        public object PageIdentifier { get; set; }
    }
}