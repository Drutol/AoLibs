using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public Dictionary<TPageIdentifier, IPageProvider<TPage>> PageDefinitions { get; }

        public abstract void CommitPageTransaction(TPage page);

        /// <summary>
        /// Event for when navigation occurs on any stack.
        /// </summary>
        public event EventHandler<TPageIdentifier> Navigated;

        /// <summary>
        /// Event for when back navigation occurs.
        /// </summary>
        public event EventHandler<TPageIdentifier> WentBack;

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

        protected NavigationManagerBase(IStackResolver<TPage, TPageIdentifier> stackResolver = null)
        {
            _stackResolver = stackResolver ?? new DefaultStackResolver();
            PageDefinitions = new Dictionary<TPageIdentifier, IPageProvider<TPage>>();
        }

        public void Navigate(TPageIdentifier page, object args = null)
        {
            if (Interceptor != null)
                page = Interceptor(page);
            Navigated?.Invoke(this, page);
            ResolveStackManager(page).Navigate(page, args);
        }

        public void Navigate(TPageIdentifier page, NavigationBackstackOption backstackOption, object args = null)
        {
            if (Interceptor != null)
                page = Interceptor(page);
            Navigated?.Invoke(this, page);
            ResolveStackManager(page).Navigate(page, backstackOption, args);
        }

        public void AddActionToBackstack(Action action)
        {
            _stackManagers.First().Value.AddActionToBackstack(action);
        }

        public void AddActionToBackstack(TPageIdentifier currentPage, Action action)
        {
            ResolveStackManager(currentPage).AddActionToBackstack(action);
        }

        public void GoBack(object args = null)
        {
            var result = _stackManagers.First().Value.GoBack(args);
            if(result.WentBack)
                WentBack?.Invoke(this, result.TargetPage);
        }

        public void GoBack(TPageIdentifier stackIdentifier, object args = null)
        {
            var result = ResolveStackManager(stackIdentifier).GoBack(args);
            if(result.WentBack)
                WentBack?.Invoke(this, result.TargetPage);
        }

        public void PopFromBackstack()
        {
            _stackManagers.First().Value.PopFromBackstack();
        }

        public void PopFromBackstack(TPageIdentifier stackIdentifier)
        {
            ResolveStackManager(stackIdentifier).PopFromBackstack();
        }

        public bool PopActionFromBackStack()
        {
            return _stackManagers.First().Value.PopActionFromBackstack();
        }

        public bool PopActionFromBackStack(TPageIdentifier stackIdentifier)
        {
            return ResolveStackManager(stackIdentifier).PopActionFromBackstack();
        }

        public void ClearBackStack()
        {
            _stackManagers.First().Value.ClearBackStack();
        }

        public void ClearBackStack(TPageIdentifier stackIdentifier)
        {
            ResolveStackManager(stackIdentifier).ClearBackStack();
        }

        public void PopFromBackStackFromExternal(TPageIdentifier pageIdentifier)
        {
            ResolveStackManager(pageIdentifier).PopFromBackstackExternal(pageIdentifier);
        }

        public bool OnBackRequested()
        {
            var result = _stackManagers.First().Value.OnBackRequested();
            if(result.WentBack)
                WentBack?.Invoke(this, result.TargetPage);
            
            return result.WentBack;
        }

        public bool OnBackRequested(TPageIdentifier stackIdentifier)
        {
            var result = ResolveStackManager(stackIdentifier).OnBackRequested();
            if(result.WentBack)
                WentBack?.Invoke(this, result.TargetPage);

            return result.WentBack;
        }

        public virtual void NotifyPagePopped(INavigationPage poppedPage)
        {
        }

        public virtual void NotifyPagesPopped(IEnumerable<TPage> pages)
        {
        }

        public virtual void NotifyStackCleared()
        {
        }

        public virtual void NotifyPagePushed(TPage page)
        {
        }

        public virtual void NotifyPagePushedWithoutBackstack(TPage page)
        {
        }

        private StackManager<TPage, TPageIdentifier> ResolveStackManager(TPageIdentifier pageIdentifier)
        {
            return ResolveStackManager(_stackResolver.ResolveStackForIdentifier(pageIdentifier));
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