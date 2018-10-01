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
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.Android;
using GalaSoft.MvvmLight.Helpers;
using NavigationLib.Android.Navigation;

namespace AoLibs.Sample.Android.Fragments
{
    [NavigationPage((int)PageIndex.PageC, NavigationPageAttribute.PageProvider.Cached)]
    public class TestPageCFragment : FragmentBase<TestViewModelC>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.test_page_c;

        protected override void InitBindings()
        {
            ButtonGoBack.SetOnClickCommand(ViewModel.GoBackCommand);
            ButtonNavigateA.SetOnClickCommand(ViewModel.NavigateAWithFirstOccurrence);
            ButtonNavigateB.SetOnClickCommand(ViewModel.NavigateBWithFirstOccurrence);
        }

        #region Views

        private Button _buttonGoBack;
        private Button _buttonNavigateA;
        private Button _buttonNavigateB;

        public Button ButtonGoBack => _buttonGoBack ?? (_buttonGoBack = FindViewById<Button>(Resource.Id.ButtonGoBack));

        public Button ButtonNavigateA => _buttonNavigateA ?? (_buttonNavigateA = FindViewById<Button>(Resource.Id.ButtonNavigateA));

        public Button ButtonNavigateB => _buttonNavigateB ?? (_buttonNavigateB = FindViewById<Button>(Resource.Id.ButtonNavigateB));

        #endregion
    }
}