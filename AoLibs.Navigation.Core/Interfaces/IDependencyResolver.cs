namespace AoLibs.Navigation.Core.Interfaces
{
    /// <summary>
    /// Interface used to provide ViewModels for injection to actual <see cref="INavigationPage"/> instances.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Resolves for instance of <see cref="TDependency"/>.
        /// </summary>
        /// <typeparam name="TDependency">ViewModel.</typeparam>
        TDependency Resolve<TDependency>();
    }
}
