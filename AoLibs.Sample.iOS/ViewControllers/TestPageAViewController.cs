using System;
using AoLibs.Dialogs.iOS;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;
using AoLibs.Utilities.iOS.Extensions;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.iOS.ViewControllers
{
    [NavigationPage((int)PageIndex.PageA, NavigationPageAttribute.PageProvider.Cached, StoryboardName = "Main",
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
            ResetButton.SetOnClickCommand(ViewModel.ResetFanciness);
            NavigateButton.SetOnClickCommand(ViewModel.NavigateSomewhereElseCommand);
            ShowDialogButton.SetOnClickCommand(ViewModel.ShowDialogCommand);
            ShowDifferentDialogButton.SetOnClickCommand(ViewModel.ShowDialogBCommand);
        }
    }
}