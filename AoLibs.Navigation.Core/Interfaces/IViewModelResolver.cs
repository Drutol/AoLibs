namespace AoLibs.Navigation.Core.Interfaces
{
    /// <summary>
    /// Interface used to provide ViewModels for injection to actual <see cref="INavigationPage"/> instances.
    /// </summary>
    public interface IViewModelResolver
    {
        /// <summary>
        /// Resolves for instance of <see cref="TViewModel"/>
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel</typeparam>
        TViewModel Resolve<TViewModel>();
    }
}
