using System;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;
using UIKit;

namespace AoLibs.Sample.iOS.ViewControllers
{
    [NavigationPage((int)PageIndex.PageC,NavigationPageAttribute.PageProvider.Cached,StoryboardName = "Main",ViewControllerIdentifier = nameof(TestPageCViewController))]
    public partial class TestPageCViewController : ViewControllerBase<TestViewModelC>
    {
        public TestPageCViewController (IntPtr handle) : base (handle)
        {
        }


        public override void InitBindings()
        {
            NavigateAToFirstOccurenceButton.SetOnClickCommand(ViewModel.NavigateAWithFirstOccurrence);
            GoBackButton.SetOnClickCommand(ViewModel.GoBackCommand);
        }
    }
}