namespace AoLibs.Dialogs.Core.Interfaces
{
    /// <summary>
    /// Interface used to define component allowing to retrieve given ViewModel.
    /// </summary>
    public interface ICustomDialogDependencyResolver
    {
        /// <summary>
        /// Resolves TDependency for given type.
        /// </summary>
        /// <typeparam name="TDependency">TDependency to resolve.</typeparam>
        TDependency Resolve<TDependency>();
    }
}
