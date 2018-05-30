using System.Threading;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface INavigationPage
    {        
        object PageIdentifier { get; set; }
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
        /// <summary>
        /// Allows to delegate semaphore that will be release when navigation completes.
        /// </summary>
        SemaphoreSlim ObtainNavigationSemaphore();
    }
}