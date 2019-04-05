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
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.Android;

namespace AoLibs.Sample.Android.Fragments
{
    [NavigationPage((int) PageIndex.PageA, NavigationPageAttribute.PageProvider.Cached)]
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
            ButtonReset.SetOnClickCommand(ViewModel.ResetFanciness);
            ButtonDialog.SetOnClickCommand(ViewModel.ShowDialogCommand);
            ButtonDialogB.SetOnClickCommand(ViewModel.ShowDialogBCommand);
            ButtonInput.SetOnClickCommand(ViewModel.InputFanciness);
        }

        #region Views

        private Button _buttonChoose;
        private Button _buttonInput;
        private Button _buttonShow;
        private Button _buttonReset;
        private Button _buttonDialog;
        private Button _buttonDialogB;
        private Button _buttonNavigate;

        public Button ButtonChoose => _buttonChoose ?? (_buttonChoose = FindViewById<Button>(Resource.Id.ButtonChoose));

        public Button ButtonInput => _buttonInput ?? (_buttonInput = FindViewById<Button>(Resource.Id.ButtonInput));

        public Button ButtonShow => _buttonShow ?? (_buttonShow = FindViewById<Button>(Resource.Id.ButtonShow));

        public Button ButtonReset => _buttonReset ?? (_buttonReset = FindViewById<Button>(Resource.Id.ButtonReset));

        public Button ButtonDialog => _buttonDialog ?? (_buttonDialog = FindViewById<Button>(Resource.Id.ButtonDialog));

        public Button ButtonDialogB => _buttonDialogB ?? (_buttonDialogB = FindViewById<Button>(Resource.Id.ButtonDialogB));

        public Button ButtonNavigate => _buttonNavigate ?? (_buttonNavigate = FindViewById<Button>(Resource.Id.ButtonNavigate));

        #endregion
    }
}