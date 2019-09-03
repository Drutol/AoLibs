namespace AoLibs.Navigation.Android.Navigation
{
    /// <summary>
    /// Base fragment class which fetches given ViewModel automatically.
    /// </summary>
    /// <typeparam name="TViewModel">The type of ViewModel bound with this fragment.</typeparam>
    public abstract class FragmentBase<TViewModel> : NavigationFragmentBase 
        where TViewModel : class
    {
        public FragmentBase(bool hasNonTrackableBindings = false) 
            : base(hasNonTrackableBindings)
        {
            ViewModel = DependencyResolver?.Resolve<TViewModel>();
        }

        protected TViewModel ViewModel { get; }
    }
}