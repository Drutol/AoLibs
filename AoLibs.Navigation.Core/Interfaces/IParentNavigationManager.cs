using System;
using System.Collections.Generic;

namespace AoLibs.Navigation.Core.Interfaces
{
    internal interface IParentNavigationManager<TPage, TPageIdentifier> 
        where TPage : INavigationPage
    {
        Dictionary<TPageIdentifier, IPageProvider<TPage>> PageDefinitions { get; }
        bool ReturnsPageInstanceAfterNavigation { get; }
        void CommitPageTransaction(TPage page);
        TPage CommitPageTransaction(Type pageType);

        void NotifyPagePopped(INavigationPage targetPage);
        void NotifyPagesPopped(IEnumerable<TPage> pages);

        void NotifyStackCleared();

        void NotifyPagePushed(TPage page);
        void NotifyPagePushedWithoutBackstack(TPage newFragment);
    }
}