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
using AoLibs.Sample.Shared.NavArgs;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.Android;
using GalaSoft.MvvmLight.Helpers;
using NavigationLib.Android.Navigation;

namespace AoLibs.Sample.Android.Fragments
{
    public class TestPageBFragment : FragmentBase<TestViewModelB>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.test_page_b;

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo(NavigationArguments as PageBNavArgs);
        }

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message,() => TextView.Text));

            ButtonGoBack.SetOnClickCommand(ViewModel.GoBackCommand);
        }

        #region Views

        private TextView _textView;
        private Button _buttonGoBack;

        public TextView TextView => _textView ?? (_textView = FindViewById<TextView>(Resource.Id.TextView));

        public Button ButtonGoBack => _buttonGoBack ?? (_buttonGoBack = FindViewById<Button>(Resource.Id.ButtonGoBack));

        #endregion
    }
}