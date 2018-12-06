using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    /// <summary>
    /// Base implementation of <see cref="ICustomDialogsManager{TDialogIndex}"/>
    /// </summary>
    /// <typeparam name="TDialogIndex">Enum defining dialog pages.</typeparam>
    public abstract class CustomDialogsManagerBase<TDialogIndex> : ICustomDialogsManager<TDialogIndex>
    {
        private readonly Dictionary<TDialogIndex, ICustomDialogProvider> _dialogsDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogsManagerBase{TDialogIndex}"/> class.
        /// </summary>
        /// <param name="dialogsDictionary">Dictionary defining all dialogs that will be used in the application.</param>
        protected CustomDialogsManagerBase(Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary)
        {
            _dialogsDictionary = dialogsDictionary;
        }

        /// <summary>
        /// Gets the dialog associated with given <see cref="dialog"/>
        /// </summary>
        /// <param name="dialog">The dialog type to retrieve.</param>
        public ICustomDialog this[TDialogIndex dialog] => _dialogsDictionary[dialog].Dialog;
    }
}
