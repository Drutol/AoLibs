using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.Android;
using NavigationLib.Android.Navigation;

namespace AoLibs.Sample.Android.Fragments
{
    [NavigationPage((int)PageIndex.PageA,NavigationPageAttribute.PageProvider.Cached)]
    public class TestPageAFragment : FragmentBase<TestViewModelA>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.test_page_a;

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo();
        }

        protected override void InitBindings()
        {
            ButtonChoose.SetOnClickCommand(ViewModel.AskUserAboutFancyThingsCommand);
            ButtonShow.SetOnClickCommand(ViewModel.ShowLastFanciedThingCommand);
            ButtonNavigate.SetOnClickCommand(ViewModel.NavigateSomewhereElseCommand);
        }

        #region Views

        private Button _buttonChoose;
        private Button _buttonShow;
        private Button _buttonNavigate;

        public Button ButtonChoose => _buttonChoose ?? (_buttonChoose = FindViewById<Button>(Resource.Id.ButtonChoose));

        public Button ButtonShow => _buttonShow ?? (_buttonShow = FindViewById<Button>(Resource.Id.ButtonShow));

        public Button ButtonNavigate =>
            _buttonNavigate ?? (_buttonNavigate = FindViewById<Button>(Resource.Id.ButtonNavigate));

        #endregion
    }
}