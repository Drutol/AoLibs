using System;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;

namespace AoLibs.Sample.iOS.ViewControllers
{
    [NavigationPage((int) PageIndex.PageA, NavigationPageAttribute.PageProvider.Cached, StoryboardName = "Main",
        ViewControllerIdentifier = "TestPageAViewController")]
    public partial class TestPageAViewController : ViewControllerBase<TestViewModelA>
    {
        public TestPageAViewController(IntPtr handle) : base(handle)
        {

        }

        public override void NavigatedBack()
        {
            base.NavigatedBack();
        }

        public override void NavigatedFrom()
        {
            base.NavigatedFrom();
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