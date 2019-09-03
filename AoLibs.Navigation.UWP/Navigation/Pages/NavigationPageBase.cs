using Windows.UI.Xaml.Controls;
using AoLibs.Navigation.Core.Interfaces;

namespace AoLibs.Navigation.UWP.Pages
{
    public abstract class NavigationPageBase : Page, INavigationPage
    {
        internal static IDependencyResolver DependencyResolver { get; set; }

        public abstract object PageIdentifier { get; set; }
        public abstract object NavigationArguments { get; set; }

        public NavigationPageBase()
        {

        }

        protected virtual T Resolve<T>()
            where T : class
        {
            return DependencyResolver?.Resolve<T>();
        }

        public virtual void NavigatedTo()
        {
        }

        public virtual void NavigatedBack()
        {
        }

        public virtual void NavigatedFrom()
        {
        }
    }
}
