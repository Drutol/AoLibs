using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoLibs.Navigation.UWP.Pages
{
    public abstract class PageBase<TViewModel> : NavigationPageBase
    {
        public override object PageIdentifier { get; set; }
        public override object NavigationArguments { get; set; }

        public TViewModel ViewModel { get; protected set; }

        public PageBase()
        {
            ViewModel = DependencyResolver.Resolve<TViewModel>();
            DataContext = ViewModel;
        }
    }
}
