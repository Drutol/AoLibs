using System;
using System.Threading;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface INavigationPage : IDisposable
    {        
        /// <summary>
        /// Gets or sets TPageIdentifier.
        /// </summary>
        object PageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets current navigation arguments.
        /// </summary>
        object NavigationArguments { get; set; }

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