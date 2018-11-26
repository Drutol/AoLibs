using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public interface ICustomDialogViewModelResolver
    {
        TViewModel Resolve<TViewModel>() 
            where TViewModel : CustomDialogViewModelBase;
    }
}
