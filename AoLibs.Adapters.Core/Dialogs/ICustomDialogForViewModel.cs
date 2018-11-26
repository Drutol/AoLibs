using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public interface ICustomDialogForViewModel : ICustomDialog
    {
        void SetResult(object result);
        void CancelResult();
    }
}
