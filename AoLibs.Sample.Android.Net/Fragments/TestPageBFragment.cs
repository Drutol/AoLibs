using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.Android;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Sample.Android.Net.Fragments
{
    [NavigationPage((int)PageIndex.PageB,NavigationPageAttribute.PageProvider.Oneshot)]
    public class TestPageBFragment : FragmentBase<TestViewModelB>
    {
        public override int LayoutResourceId { get; } = Utilities.Android.Resource.Layout.test_page_b;

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo(NavigationArguments as PageBNavArgs);
        }

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Message,() => TextView.Text));

            ButtonGoBack.SetOnClickCommand(ViewModel.GoBackCommand);
            ButtonNavigateC.SetOnClickCommand(ViewModel.NavigateCCommand);
            ButtonNavigateCNoBackstack.SetOnClickCommand(ViewModel.NavigateCNoBackCommand);
        }

        #region Views

        private TextView _textView;
        private Button _buttonGoBack;
        private Button _buttonNavigateC;
        private Button _buttonNavigateCNoBackstack;

        public TextView TextView => _textView ?? (_textView = FindViewById<TextView>(Utilities.Android.Resource.Id.TextView));

        public Button ButtonGoBack => _buttonGoBack ?? (_buttonGoBack = FindViewById<Button>(Utilities.Android.Resource.Id.ButtonGoBack));

        public Button ButtonNavigateC => _buttonNavigateC ?? (_buttonNavigateC = FindViewById<Button>(Utilities.Android.Resource.Id.ButtonNavigateC));

        public Button ButtonNavigateCNoBackstack => _buttonNavigateCNoBackstack ?? (_buttonNavigateCNoBackstack = FindViewById<Button>(Utilities.Android.Resource.Id.ButtonNavigateCNoBackstack));

        #endregion
    }
}