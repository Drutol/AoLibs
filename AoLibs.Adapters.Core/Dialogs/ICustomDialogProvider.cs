using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public interface ICustomDialogProvider
    {
        ICustomDialog Dialog { get; }
    }
}
