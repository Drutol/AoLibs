using Windows.UI.Xaml.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.Core.PageProviders;
using AoLibs.Navigation.UWP.Pages;

namespace AoLibs.Navigation.UWP.Providers
{
    public class FrameCachedPageProvider<TPage> : CachedPageProvider<TPage> where TPage : NavigationPageBase, INavigationPage
    {
        protected override void OnPageCreated(TPage page)
        {
            page.NavigationCacheMode = NavigationCacheMode.Required;
        }
    }
}
