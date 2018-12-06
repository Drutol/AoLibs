using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core.Dialogs;
using AoLibs.Sample.Shared.NavArgs;

namespace AoLibs.Sample.Shared.DialogViewModels
{
    public class TestDialogViewModelB : CustomDialogViewModelWithParameterBase<DialogBNavArgs>
    {
        private string _message;

        protected override void OnDialogAppeared(DialogBNavArgs parameter)
        {
            Message = parameter.Message;
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }
    }
}
