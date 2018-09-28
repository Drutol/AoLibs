using System;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.NavArgs;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Sample.iOS.ViewControllers
{
    [NavigationPage((int)PageIndex.PageB, NavigationPageAttribute.PageProvider.Cached, StoryboardName = "Main",
        ViewControllerIdentifier = "TestPageBViewController")]
    public partial class TestPageBViewController : ViewControllerBase<TestViewModelB>
    {
        public TestPageBViewController (IntPtr handle) : base (handle)
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
            ViewModel.NavigatedTo(NavigationArguments as PageBNavArgs);
            base.NavigatedTo();
        }

        public override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message, () => Label.Text));
            GoBackButton.SetOnClickCommand(ViewModel.GoBackCommand);
        }
    }
}