using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml.Controls;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.UWP.Attributes;
using AoLibs.Navigation.UWP.Pages;
using AoLibs.Navigation.UWP.Providers;

namespace AoLibs.Navigation.UWP
{
    /// <summary>
    /// Class that fulfills the purpose of executing actual navigation transactions.
    /// </summary>
    /// <typeparam name="TPageIdentifier">Page enum type.</typeparam>
    public class NavigationManager<TPageIdentifier> : NavigationManagerBase<NavigationPageBase, TPageIdentifier>
    {
        private Frame _rootFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationManager{TPageIdentifier}"/> class.
        /// </summary>
        /// <param name="rootFrame">The view which will be used as the one being replaced with new Views</param>
        /// <param name="pageDefinitions">The dictionary defining pages.</param>
        /// <param name="dependencyResolver">Class used to resolve ViewModels for pages derived from <see cref="FragmentBase{TViewModel}"/></param>
        /// <param name="stackResolver">Class allowing to differentiate to which stack given indentigier belongs.</param>
        public NavigationManager(
            Frame rootFrame,
            Dictionary<TPageIdentifier, IPageProvider<NavigationPageBase>> pageDefinitions,
            IDependencyResolver dependencyResolver = null,
            IStackResolver<NavigationPageBase, TPageIdentifier> stackResolver = null)
            : base(pageDefinitions, stackResolver)
        {
            _rootFrame = rootFrame;

            NavigationPageBase.DependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationManager{TPageIdentifier}"/> class.
        /// To gather page definitions it searches for classes marked with <see cref="NavigationPageAttribute"/> from <see cref="Assembly.GetCallingAssembly"/>
        /// </summary>
        /// <param name="rootFrame">The view which will be used as the one being replaced with new Views</param>
        /// <param name="dependencyResolver">Class used to resolve ViewModels for pages derived from <see cref="FragmentBase{TViewModel}"/></param>
        /// <param name="stackResolver">Class allowing to differentiate to which stack given indecenter belongs.</param>
        public NavigationManager(
            Frame rootFrame,
            IDependencyResolver dependencyResolver = null,
            IStackResolver<NavigationPageBase, TPageIdentifier> stackResolver = null)
            : base(stackResolver)
        {
            _rootFrame = rootFrame;

            NavigationPageBase.DependencyResolver = dependencyResolver;

            var types = Assembly.GetCallingAssembly().GetTypes();

            foreach (var type in types)
            {
                var attr = type.GetTypeInfo().GetCustomAttribute<NavigationPageAttribute>();

                if (attr != null)
                {
                    IPageProvider<NavigationPageBase> provider = null;

                    switch (attr.PageProviderType)
                    {
                        case NavigationPageAttribute.PageProvider.Cached:
                            provider = ObtainProviderFromType(typeof(FrameCachedPageProvider<>));
                            break;
                        case NavigationPageAttribute.PageProvider.Oneshot:
                            provider = ObtainProviderFromType(typeof(FrameOneshotPageProvider<>));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    PageDefinitions.Add((TPageIdentifier)(object)attr.Page, provider);
                }

                IPageProvider<NavigationPageBase> ObtainProviderFromType(Type providerType)
                {
                    return (IPageProvider<NavigationPageBase>)providerType.MakeGenericType(type)
                        .GetConstructor(new Type[] { })
                        .Invoke(null);
                }
            }

            foreach (var pageDefinition in PageDefinitions)
            {
                pageDefinition.Value.PageIdentifier = pageDefinition.Key;
            }
        }

        public override void CommitPageTransaction(NavigationPageBase page)
        {
            _rootFrame.Navigate(page.GetType());
        }
    }
}
