using System.Collections.Generic;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;

namespace AoLibs.Dialogs.Android
{
    /// <summary>
    /// Default Android implementation of <see cref="CustomDialogsManagerBase{TDialogIndex}"/>
    /// </summary>
    /// <typeparam name="TDialogIndex">Type of enum that is used to identify the dialogs.</typeparam>
    public class CustomDialogsManager<TDialogIndex> : CustomDialogsManagerBase<TDialogIndex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogsManager{TDialogIndex}"/> class.
        /// </summary>


        public CustomDialogsManager(
            Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary) : base(
            dialogsDictionary)
        {

        }
    }
}