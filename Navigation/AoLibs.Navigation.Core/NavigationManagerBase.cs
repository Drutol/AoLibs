using System;
using System.Collections.Generic;
using System.Linq;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.Core
{
    /// <summary>
    /// Base class for NavigationMangers providing all basic functionality. It is used as a doorway to mechanisms below, it is capable of managing a number of various stacks.
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    /// <typeparam name="TPageIdentifier"></typeparam>
    public abstract class NavigationManagerBase<TPage, TPageIdentifier> :
        IParentNavigationManager<TPage, TPageIdentifier>,
        INavigationManager<TPageIdentifier>
        where TPage : class, INavigationPage
    {    
        private readonly Dictionary<Stack<BackstackEntry<TPage>>, StackManager<TPage, TPageIdentifier>> _stackManagers =
            new Dictionary<Stack<BackstackEntry<TPage>>, StackManager<TPage, TPageIdentifier>>();

        private readonly IStackResolver<TPage, TPageIdentifier> _stackResolver;

        /// <summary>
        /// Event for when navigation occurs on any stack.
        /// </summary>
        public event EventHandler<TPageIdentifier> Navigated;
        /// <summary>
        /// Event for when back navigation occurs.
        /// </summary>
        public event EventHandler<EventArgs> WentBack;
        /// <summary>
        /// Navigation interceptor.
        /// </summary>
        public NaviagtionInterceptor<TPageIdentifier> Interceptor { get; set; }

        protected NavigationManagerBase(Dictionary<TPageIdentifier, IPageProvider<TPage>> pageDefinitions,
            IStackResolver<TPage, TPageIdentifier> stackResolver = null)
        {
            _stackResolver = stackResolver ?? new DefaultStackResolver();
            PageDefinitions = pageDefinitions;

            foreach (var pageDefinition in pageDefinitions)
            {
                pageDefinition.Value.PageIdentifier = pageDefinition.Key;
            }
        }

        public void Navigate(TPageIdentifier page, object args = null)
        {
            if (Interceptor != null)
                page = Interceptor(page);
            Navigated?.Invoke(this,page);
            ResolveStackManager(page).Navigate(page, args);
        }

        public void Navigate(TPageIdentifier page, NavigationBackstackOption backstackOption, object args = null)
        {
            if (Interceptor != null)
                page = Interceptor(page);
            Navigated?.Invoke(this, page);
            ResolveStackManager(page).Navigate(page, backstackOption, args);
        }

        public void AddActionToBackstack(TPageIdentifier currentPage,Action action)
        {
            ResolveStackManager(currentPage).AddActionToBackstack(action);
        }

        public void GoBack(object args = null)
        {
            foreach (var stackManager in _stackManagers)
            {
                WentBack?.Invoke(this, EventArgs.Empty);
                stackManager.Value.GoBack(args);
            }
        }

        public void GoBack(Enum stackIdentifier, object args = null)
        {
            var result = ResolveStackManager(stackIdentifier).GoBack(args);
            if (!result.Equals(default(TPageIdentifier)))
            {
                Navigated?.Invoke(this,result);
            }

        }

        public void PopFromBackStackFromExternal(Enum stackIdentifier)
        {
            ResolveStackManager(stackIdentifier).PopFromBackstackExternal(stackIdentifier);
        }

        public bool PopActionFromBackStack(Enum stackIdentifier)
        {
           return ResolveStackManager(stackIdentifier).PopActionFromBackstack();
        }

        public void ClearBackStack()
        {
            foreach (var stackManager in _stackManagers)
                stackManager.Value.ClearBackStack();
        }

        public void ClearBackStack(Enum stackIdentifier)
        {
            ResolveStackManager(stackIdentifier).ClearBackStack();
        }

        public bool OnBackRequested()
        {
            var result = _stackManagers.First().Value.OnBackRequested();
            if (!result.current.Equals(default(TPageIdentifier)))
            {
                Navigated?.Invoke(this, result.current);
            }

            return result.handled;
        }

        public bool OnBackRequested(Enum stackIdentifier)
        {
            return ResolveStackManager(stackIdentifier).OnBackRequested().handled;
        }

        public Dictionary<TPageIdentifier, IPageProvider<TPage>> PageDefinitions { get; }

        public abstract void CommitPageTransaction(TPage page);

        public virtual void NotifyPagePopped(INavigationPage targetPage) { }

        public virtual void NotifyPagesPopped(IEnumerable<TPage> pages) { }

        public virtual void NotifyStackCleared() { }

        public virtual void NotifyPagePushed(TPage page) { }

        public virtual void NotifyPagePushedWithoutBackstack(TPage page) { }     

        private StackManager<TPage, TPageIdentifier> ResolveStackManager(TPageIdentifier pageIdentifier)
        {
            return ResolveStackManager(_stackResolver.ResolveStackForIdentifier(pageIdentifier));
        }

        private StackManager<TPage, TPageIdentifier> ResolveStackManager(Enum stackIdentifier)
        {
            return ResolveStackManager(_stackResolver.ResolveStackForTag(stackIdentifier));      
        }

        private StackManager<TPage, TPageIdentifier> ResolveStackManager(TaggedStack<BackstackEntry<TPage>> stack)
        {
            if (_stackManagers.ContainsKey(stack))
                return _stackManagers[stack];

            var mgr = new StackManager<TPage, TPageIdentifier>(stack, this);

            _stackManagers[stack] = mgr;
            return mgr;
        }

        private class DefaultStackResolver : IStackResolver<TPage, TPageIdentifier>
        {
            private readonly TaggedStack<BackstackEntry<TPage>> _stack = new TaggedStack<BackstackEntry<TPage>>();

            public TaggedStack<BackstackEntry<TPage>> ResolveStackForIdentifier(TPageIdentifier identifier) => _stack;

            public TaggedStack<BackstackEntry<TPage>> ResolveStackForTag(Enum tag) => _stack;
        }
    }
}