using System.Threading;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface INavigationPage
    {        
        /// <summary>
        /// Assigned TPageIdentifier.
        /// </summary>
        object PageIdentifier { get; set; }
        /// <summary>
        /// Current navigation arguments.
        /// </summary>
        object NavigationArguments { set; }

        /// <summary>
        /// Called when we navigate to this page going forward.
        /// </summary>
        void NavigatedTo();
        /// <summary>
        /// Called when we navigate to this page by going back.
        /// </summary>
        void NavigatedBack();
        /// <summary>
        /// Called when we leave given page.
        /// </summary>
        void NavigatedFrom();
    }
}