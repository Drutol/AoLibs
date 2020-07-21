using System;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core.PageProviders
{
    /// <summary>
    /// Provider which will recreate the bage instance whnever it is requested so every time we start with clean state.
    /// </summary>
    /// <typeparam name="TPage">Type of page on target platfrom.</typeparam>
    public class OneshotPageProvider<TPage> : IPageProvider<TPage> 
        where TPage : INavigationPage
    {
        protected Func<TPage> Factory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneshotPageProvider{TPage}"/> class.
        /// </summary>
        /// <param name="factory">Factory used to build actual page.</param>
        public OneshotPageProvider(Func<TPage> factory)
        {
            Factory = new Func<TPage>(() =>
            {
                var page = factory.Invoke();
                page.PageIdentifier = PageIdentifier;
                OnPageCreated(page);
                return page;
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneshotPageProvider{TPage}"/> class.
        /// <see cref="Activator.CreateInstance{T}"/> will be used for instantination.
        /// </summary>
        public OneshotPageProvider()
        {
            Factory = new Func<TPage>(() =>
            {
                var page = Activator.CreateInstance<TPage>();
                page.PageIdentifier = PageIdentifier;
                OnPageCreated(page);
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
        /// Gets actual type of held page.
        /// </summary>
        public Type PageType { get; } = typeof(TPage);

        /// <summary>
        /// Gets or sets instance of held page.
        /// </summary>
        public TPage Page
        {
            get => Factory.Invoke();
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets TPageIdentifier hidden beyond <see cref="object"/>
        /// </summary>
        public object PageIdentifier { get; set; }

        protected virtual void OnPageCreated(TPage page)
        {
        }
    }
}