using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
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
