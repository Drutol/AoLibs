using System;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface INavigationManager<TPageIdentifier>
    {
        event EventHandler<TPageIdentifier> Navigated;
        event EventHandler<EventArgs> WentBack;

        /// <summary>
        /// Allows to intercept navigation and change target page in advance.
        /// </summary>
        NaviagtionInterceptor<TPageIdentifier> Interceptor { get; set; }

        /// <summary>
        ///     Navigates to page with <see cref="NavigationBackstackOption.AddToBackstack" /> option.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="args"></param>
        void Navigate(TPageIdentifier page, object args = null);

        /// <summary>
        ///     Navigates to page with given options.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="backstackOption"></param>
        /// <param name="args"></param>
        void Navigate(TPageIdentifier page, NavigationBackstackOption backstackOption, object args = null);

        /// <summary>
        ///     Adds artificail action to next back navigation. Could be used for popups for example.
        /// </summary>
        /// <param name="action"></param>
        void AddActionToBackstack(TPageIdentifier currentPage, Action action);

        /// <summary>
        ///     Goes back on all backstacks.
        /// </summary>
        /// <param name="args"></param>
        void GoBack(object args = null);

        /// <summary>
        ///     Goes back on given stack.
        /// </summary>
        /// <param name="stackIdentifier"></param>
        /// <param name="args"></param>
        void GoBack(Enum stackIdentifier, object args = null);

        /// <summary>
        ///     Clears all backstacks.
        /// </summary>
        void PopFromBackStackFromExternal(Enum stackIdentifier);        
        
        /// <summary>
        ///     Removes topmost action from backstack, if there was one returns true, false otherwise.
        /// </summary>
        bool PopActionFromBackStack(Enum stackIdentifier);        
        
        /// <summary>
        ///     Clears all backstacks.
        /// </summary>
        //void ClearBackStack();

        /// <summary>
        ///     Clears backstack identifier by given page.
        /// </summary>
        /// <param name="stackIdentifier"></param>
        //void ClearBackStack(Enum stackIdentifier);

        /// <summary>
        ///     Returns false if main (first) back stack is empty and back naviagtion sould be handled by app framework.
        /// </summary>
        /// <returns></returns>
        bool OnBackRequested();

        /// <summary>
        ///     Returns false if back stack is empty and back naviagtion sould be handled by app framework.
        /// </summary>
        /// <param name="stackIdentifier"></param>
        /// <returns></returns>
        bool OnBackRequested(Enum stackIdentifier);
    }
}