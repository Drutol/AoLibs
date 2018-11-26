using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public interface ICustomViewModelDialog<TViewModel> : ICustomDialog
        where TViewModel : CustomDialogViewModelBase
    {
        TViewModel ViewModel { get; }
    }
}
