using Foundation;
using System;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [NavigationPage((int)PageIndex.PageB, NavigationPageAttribute.PageProvider.Cached, StoryboardName = "Main",
        ViewControllerIdentifier = "TestPageBViewController")]
    public partial class TestPageBViewController : ViewControllerBase<TestViewModelB>
    {
        public TestPageBViewController (IntPtr handle) : base (handle)
        {
        }

        public override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message, () => Label.Text));
            GoBackButton.SetOnClickCommand(ViewModel.GoBackCommand);
        }
    }
}