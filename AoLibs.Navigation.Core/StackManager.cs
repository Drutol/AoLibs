using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AoLibs.Navigation.Core.Interfaces;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("AoLibs.Navigation.Test")]
namespace AoLibs.Navigation.Core
{
    internal class StackManager<TPage, TPageIdentifier> where TPage : class, INavigationPage
    {
        private readonly IParentNavigationManager<TPage, TPageIdentifier> _navigationManager;
        private readonly Stack<BackstackEntry<TPage>> _stack;
        private NavigationBackstackOption? _currentBackstackOption;

        public StackManager(Stack<BackstackEntry<TPage>> stack, IParentNavigationManager<TPage, TPageIdentifier> navigationManager)
        {
            _stack = stack;
            _navigationManager = navigationManager;
        }

        public TPage CurrentFragment { get; set; }

        public void Navigate(TPageIdentifier page, object args = null)
        {
            bool addToBackstack = CurrentFragment != null &&
                                  _currentBackstackOption != NavigationBackstackOption.NoBackstack;
            //add to backstack if needed
            if (addToBackstack)
                _stack.Push(new BackstackEntry<TPage> {Page = CurrentFragment});
                
            _currentBackstackOption = null;

            //obtain new page and push naviagtion arguments
            var newFragment = _navigationManager.PageDefinitions[page].Page;

            if (args != null)
                newFragment.NavigationArguments = args;

            //do actual naviagtion
            if (addToBackstack || CurrentFragment == null)
                _navigationManager.NotifyPagePushed(newFragment);
            else
                _navigationManager.NotifyPagePushedWithoutBackstack(newFragment);
            _navigationManager.CommitPageTransaction(newFragment);
            
            
            //if there was previous page notify about navigating from
            CurrentFragment?.NavigatedFrom();

            CurrentFragment = newFragment;

            //notify new page about navigation
            CurrentFragment.NavigatedTo();
        }

        public void Navigate(TPageIdentifier page, NavigationBackstackOption backstackOption, object args = null)
        {
            //We gotta clean all entries on backstack until we find the desired one
            if (backstackOption == NavigationBackstackOption.ClearBackstackToFirstOccurence)
            {
                var poppedPages = new List<TPage>();
                var top = _stack.Pop();
                var provider = _navigationManager.PageDefinitions[page];
                while (!top.Page.PageIdentifier.Equals(provider.PageIdentifier))
                {
                    if(top.Page != null)
                        poppedPages.Add(top.Page);
                    if (!_stack.Any())
                        break;
                    top = _stack.Pop();
                }
                CurrentFragment?.NavigatedFrom();
                _navigationManager.NotifyPagesPopped(poppedPages);
                CurrentFragment = null;
            } //before navigation new page instance will be created
            else if (backstackOption == NavigationBackstackOption.ForceNewPageInstance)
            {
                _navigationManager.PageDefinitions[page].ForceReinstantination();
            }
            else if (backstackOption == NavigationBackstackOption.SetAsRootPage)
            {
                _stack.Clear();
                CurrentFragment?.NavigatedFrom();
                _navigationManager.NotifyStackCleared();
                CurrentFragment = null;
            }

            _currentBackstackOption = backstackOption;
            Navigate(page, args);          
        }

        public void AddActionToBackstack(Action action)
        {
            _stack.Push(new BackstackEntry<TPage> {OnBackNavigation = action});
        }

        public (bool WentBack, TPageIdentifier TargetPage) GoBack(object args = null)
        {
            _currentBackstackOption = null;

            if (_stack.Count == 0)
                return default;

            var oldFragment = _stack.Pop();

            if (oldFragment.Page == null)
            {
                oldFragment.OnBackNavigation.Invoke();
                return (false,default);
            }
            else
            {
                CurrentFragment.NavigatedFrom();

                if (args != null)
                    oldFragment.Page.NavigationArguments = args;

                if (CurrentFragment == oldFragment.Page)
                {
                    CurrentFragment.NavigatedBack();
                }
                else
                {
                    _navigationManager.NotifyPagePopped(CurrentFragment);
                    CurrentFragment = oldFragment.Page;                   
                    _navigationManager.CommitPageTransaction(oldFragment.Page);
                                   
                    CurrentFragment.NavigatedBack();
                }

                return (true, (TPageIdentifier) CurrentFragment.PageIdentifier);
            }
        }

        public void ClearBackStack()
        {
            _stack.Clear();
            _navigationManager.NotifyStackCleared();
        }

        public (bool WentBack, TPageIdentifier TargetPage) OnBackRequested()
        {
            if (_stack.Count == 0)
                return (false,default);
            return GoBack();
        }

        public void PopFromBackstack()
        {
            var entry = _stack.Pop();
            if (entry.Page != null)
            {
                _navigationManager.NotifyPagePopped(entry.Page);
            }
        }

        public bool PopActionFromBackstack()
        {
            if (_stack.Peek().OnBackNavigation != null)
            {
                _stack.Pop();
                return true;
            }

            return false;
        }

        public void PopFromBackstackExternal(TPageIdentifier stackIdentifier)
        {
            if (_stack.Count > 0 && _stack.Peek().Page.PageIdentifier.Equals(stackIdentifier))
                _stack.Pop();
        }
    }
}