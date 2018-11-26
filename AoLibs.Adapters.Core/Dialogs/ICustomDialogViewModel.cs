using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace AoLibs.Adapters.Core.Dialogs
{
    public abstract class CustomDialogViewModelBase : ViewModelBase
    {
        public virtual CustomDialogConfig CustomDialogConfig { get; }

        public ICustomDialogForViewModel Dialog { get; set; }

        public virtual void OnDialogAppeared()
        {

        }

        public virtual void OnDialogDismissed()
        {

        }
    }
}
