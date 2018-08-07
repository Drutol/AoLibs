using System;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface INavigationManager<TPageIdentifier>
    {
        /// <summary>
        /// Event for when navigation occurs. The argument is the destination page.
        /// </summary>
        event EventHandler<TPageIdentifier> Navigated;
        /// <summary>
        /// Event for when back navigation occurs. The argument is the desitnation page.
        /// </summary>
        event EventHandler<TPageIdentifier> WentBack;

        /// <summary>
        /// Allows to intercept navigation and change target page in advance.
        /// </summary>
        NaviagtionInterceptor<TPageIdentifier> Interceptor { get; set; }

        /// <summary>
        /// Navigates to page with <see cref="NavigationBackstackOption.AddToBackstack" /> option.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="args"></param>
        void Navigate(TPageIdentifier page, object args = null);

        /// <summary>
        /// Navigates to page with given options.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="backstackOption"></param>
        /// <param name="args"></param>
        void Navigate(TPageIdentifier page, NavigationBackstackOption backstackOption, object args = null);

        /// <summary>
        /// Adds artificial action to next back navigation. Could be used for popups for example.
        /// </summary>
        /// <param name="action"></param>
        void AddActionToBackstack(Action action);

        /// <summary>
        ///     Adds artificial action to next back navigation. Could be used for popups for example.
        /// </summary>
        /// <param name="action"></param>
        void AddActionToBackstack(TPageIdentifier stackIdentifier, Action action);         

        /// <summary>
        /// Goes back on all backstacks.
        /// </summary>
        /// <param name="args"></param>
        void GoBack(object args = null);

        /// <summary>
        /// Goes back on given stack.
        /// </summary>
        /// <param name="stackIdentifier"></param>
        /// <param name="args"></param>
        void GoBack(TPageIdentifier stackIdentifier, object args = null);

        /// <summary>
        /// Pops topmost entry on main (first) navigation stack.
        /// </summary>
        void PopFromBackstack();

        /// <summary>
        /// Pops topmost entry on navigation stack.
        /// </summary>
        /// <param name="stackIdentifier">Page associated with desired backstack.</param>
        void PopFromBackstack(TPageIdentifier stackIdentifier);

        /// <summary>
        /// Pops page when the page was removed from stack by extranal source, like native iOS navigation.
        /// </summary>
        void PopFromBackStackFromExternal(TPageIdentifier pageIdentifier);        
        
        /// <summary>
        /// Removes topmost action from main (first) backstack, if there was one returns true, false otherwise.
        /// </summary>
        bool PopActionFromBackStack();   
        
        /// <summary>
        /// Removes topmost action from backstack, if there was one returns true, false otherwise.
        /// </summary>
        bool PopActionFromBackStack(TPageIdentifier stackIdentifier);              

        /// <summary>
        /// Returns false if main (first) back stack is empty and back naviagtion sould be handled by app framework.
        /// </summary>
        /// <returns></returns>
        bool OnBackRequested();

        /// <summary>
        /// Returns false if back stack is empty and back naviagtion sould be handled by app framework.
        /// </summary>
        /// <param name="stackIdentifier">Stack identifier.</param>
        /// <returns></returns>
        bool OnBackRequested(TPageIdentifier stackIdentifier);

        /// <summary>
        /// Clears whole main (first) backstack
        /// </summary>
        void ClearBackStack();

        /// <summary>
        /// Clears whole backstack of indicated stack.
        /// </summary>
        /// <param name="stackIdentifier">Stack identifier.</param>
        void ClearBackStack(TPageIdentifier stackIdentifier);
    }
}