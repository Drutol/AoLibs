using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public interface ICustomDialogsManager<TDialogIndex>
    {
        ICustomDialog this[TDialogIndex dialog] { get; }

        void Show(TDialogIndex dialog, object parameter = null);
    }
}
