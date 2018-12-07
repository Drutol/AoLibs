namespace AoLibs.Dialogs.Core.Interfaces
{
    /// <summary>
    /// Interface used to define component allowing to retrieve given ViewModel.
    /// </summary>
    public interface ICustomDialogViewModelResolver
    {
        /// <summary>
        /// Resolves ViewModel for given type.
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type to resolve.</typeparam>
        TViewModel Resolve<TViewModel>() 
            where TViewModel : CustomDialogViewModelBase;
    }
}
