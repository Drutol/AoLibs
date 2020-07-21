using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AoLibs.Navigation.UWP.Attributes;
using AoLibs.Navigation.UWP.Pages;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using AoLibs.Sample.Shared.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoLibs.Sample.UWP.Pages
{
    public class TestPageBBase : PageBase<TestViewModelB>
    {
    }

    [NavigationPage(PageIndex.PageB)]
    public sealed partial class TestPageB
    {
        public TestPageB()
        {
            this.InitializeComponent();
        }

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo((PageBNavArgs)NavigationArguments);
        }
    }
}
