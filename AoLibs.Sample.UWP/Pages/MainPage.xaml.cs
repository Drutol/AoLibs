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
using AoLibs.Dialogs.Android;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.UWP;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.Statics;
using AoLibs.Sample.Shared.ViewModels;
using Autofac;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AoLibs.Sample.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.NavigationManager = new NavigationManager<PageIndex>(RootFrame, new DependencyResolver());
            App.DialogManager = new CustomDialogsManager<DialogIndex>(new Dictionary<DialogIndex, ICustomDialogProvider>());

            ResourceLocator.ObtainScope().Resolve<MainViewModel>().Initialize();
        }

        public class DependencyResolver : IDependencyResolver
        {
            public TDependency Resolve<TDependency>()
            {
                return ResourceLocator.ObtainScope().Resolve<TDependency>();
            }
        }
    }
}
