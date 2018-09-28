using Foundation;
using System;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [NavigationPage((int) PageIndex.PageA, NavigationPageAttribute.PageProvider.Cached, StoryboardName = "Main",
        ViewControllerIdentifier = "TestPageAViewController")]
    public partial class TestPageAViewController : ViewControllerBase<TestViewModelA>
    {
        public TestPageAViewController(IntPtr handle) : base(handle)
        {

        }

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo();
            base.NavigatedTo();
        }

        public override void InitBindings()
        {
            ChooseFancyButton.SetOnClickCommand(ViewModel.AskUserAboutFancyThingsCommand);
            ShowFancyButton.SetOnClickCommand(ViewModel.ShowLastFanciedThingCommand);
            NavigateButton.SetOnClickCommand(ViewModel.NavigateSomewhereElseCommand);
        }
    }
}