using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public class CustomDialogsManagerBase<TDialogIndex> : ICustomDialogsManager<TDialogIndex>
    {
        private readonly Dictionary<TDialogIndex, ICustomDialogProvider> _dialogsDictionary;

        public CustomDialogsManagerBase(Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary)
        {
            _dialogsDictionary = dialogsDictionary;
        }

        public ICustomDialog this[TDialogIndex dialog] => _dialogsDictionary[dialog].Dialog;

        public void Show(TDialogIndex index, object parameter = null)
        {
            var dialog = this[index];
            dialog.Parameter = parameter;
            dialog.Show();
        }
    }
}
