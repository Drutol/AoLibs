using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.Android;

namespace AoLibs.Sample.Android.Net.Fragments
{
    [NavigationPage((int)PageIndex.PageC, NavigationPageAttribute.PageProvider.Cached)]
    public class TestPageCFragment : FragmentBase<TestViewModelC>
    {
        public override int LayoutResourceId { get; } = Utilities.Android.Resource.Layout.test_page_c;

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

        public Button ButtonGoBack => _buttonGoBack ?? (_buttonGoBack = FindViewById<Button>(Utilities.Android.Resource.Id.ButtonGoBack));

        public Button ButtonNavigateA => _buttonNavigateA ?? (_buttonNavigateA = FindViewById<Button>(Utilities.Android.Resource.Id.ButtonNavigateA));

        public Button ButtonNavigateB => _buttonNavigateB ?? (_buttonNavigateB = FindViewById<Button>(Utilities.Android.Resource.Id.ButtonNavigateB));

        #endregion
    }
}