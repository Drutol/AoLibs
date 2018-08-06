using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.Core.PageProviders;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Providers;
using UIKit;

namespace AoLibs.Navigation.iOS.Navigation
{
    public class
        NavigationManager<TPageIdentifier> : NavigationManagerBase<INavigationPage, TPageIdentifier>
    {
        private readonly UINavigationController _navigationController;
        private TaskCompletionSource<INavigationPage> _naviagtionCompletionSource;

        public bool Intercepting => _naviagtionCompletionSource != null && !_naviagtionCompletionSource.Task.IsCompleted;

        public NavigationManager(UINavigationController navigationController,
            Dictionary<TPageIdentifier, IPageProvider<INavigationPage>> pageDefinitions,
            IStackResolver<INavigationPage, TPageIdentifier> stackResolver = null
        ) : base(pageDefinitions, stackResolver)
        {
            _navigationController = navigationController;
        }

        public NavigationManager(UINavigationController navigationController,
            IStackResolver<INavigationPage, TPageIdentifier> stackResolver = null) : base(stackResolver)
        {
            _navigationController = navigationController;

            var types = Assembly.GetCallingAssembly().GetTypes();

            foreach (var type in types)
            {
                var attr = type.GetTypeInfo().GetCustomAttribute<NavigationPageAttribute>();

                if (attr != null)
                {
                    IPageProvider<INavigationPage> providerType = null;

                    if (string.IsNullOrEmpty(attr.StoryboardName))
                    {
                        switch (attr.Type)
                        {
                            case NavigationPageAttribute.PageProvider.Cached:
                                providerType = ObtainProviderFromType(typeof(CachedPageProvider<>));
                                break;
                            case NavigationPageAttribute.PageProvider.Oneshot:
                                providerType = ObtainProviderFromType(typeof(OneshotPageProvider<>));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(attr.ViewControllerIdentifier))
                        {
                            switch (attr.Type)
                            {
                                case NavigationPageAttribute.PageProvider.Cached:
                                    providerType = ObtainProviderFromType(typeof(StoryboardCachedPageProvider<>),true);
                                    break;
                                case NavigationPageAttribute.PageProvider.Oneshot:
                                    providerType = ObtainProviderFromType(typeof(StoryboardCachedPageProvider<>),true);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    PageDefinitions.Add((TPageIdentifier)(object)attr.Page, providerType);
                }
              
                IPageProvider<INavigationPage> ObtainProviderFromType(Type providerType, bool isStoryboard = false)
                {
                    return (IPageProvider<INavigationPage>) providerType.MakeGenericType(type)
                        .GetConstructor(isStoryboard ? new[] {typeof(NavigationPageAttribute)} : new Type[] { })
                        .Invoke(isStoryboard ? new object[] {attr} : null);
                }
            }

            foreach (var pageDefinition in PageDefinitions)
            {
                pageDefinition.Value.PageIdentifier = pageDefinition.Key;
            }
        }

        public override void CommitPageTransaction(INavigationPage page)
        {
            //instead of navigating there directly we are forwarding this naviagtion to awaiting handler method
            //or just ignore it given the circumstances
            _naviagtionCompletionSource?.TrySetResult(page);
            if(page is INativeNavigationPage nativePage)
                nativePage.NativeBackNavigation += OnNativeBackNavigation;
            
        }

        private void OnNativeBackNavigation(object sender, EventArgs eventArgs)
        {
            var page = sender as INavigationPage;
            PopFromBackStackFromExternal((Enum)page.PageIdentifier);
        }

        public override void NotifyPagePopped(INavigationPage targetPage)
        {
            //just pop and be happy
            _navigationController.PopViewController(true);
        }

        public override void NotifyPagesPopped(IEnumerable<INavigationPage> pages)
        {
            for (int i = 0; i <= pages.Count() + 1; i++)
            {
				_navigationController.PopViewController(false);
            }
        }

        public override void NotifyPagePushed(INavigationPage page)
        {
            //we are just pushing new page, not trickery required
            if(!Intercepting)
                _navigationController.PushViewController((UIViewController) page, true);
        }

        public override void NotifyPagePushedWithoutBackstack(INavigationPage page)
        {
            //first we copy current stack
            var viewControllers = _navigationController.ViewControllers;
            //we have the page we are pushing so we pass on intercepting and replace last page with new one
            viewControllers[viewControllers.Length-1] = (UIViewController)page;
            //after navigation is completed we silently remove previous page
            _navigationController.SetViewControllers(viewControllers, true);
        }

        public override async void NotifyStackCleared()
        {
            //first we will prepare new stack
            var viewControllers = new UIViewController[1];
            //then after creating new stack we will wait for CommitPageTransation to know where we need to navigate
            _naviagtionCompletionSource = new TaskCompletionSource<INavigationPage>();
            var targetController = await _naviagtionCompletionSource.Task;           
            viewControllers[0] = (UIViewController) targetController;
            //now check if appropriate instance is already on stack      
            var controllerPresentOnStack = _navigationController.ViewControllers.FirstOrDefault(controller =>
                (controller as INavigationPage).PageIdentifier.Equals(targetController.PageIdentifier));
            if (controllerPresentOnStack != null)   
                viewControllers[0] = controllerPresentOnStack;              
           
            //and set new root
            _navigationController.SetViewControllers(viewControllers,true);
        }
    }
}