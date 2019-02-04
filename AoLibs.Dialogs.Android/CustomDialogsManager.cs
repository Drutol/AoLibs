using System.Collections.Generic;
using AoLibs.Dialogs.Android.Interfaces;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace AoLibs.Dialogs.Android
{
    /// <summary>
    /// Default Android implementation of <see cref="CustomDialogsManagerBase{TDialogIndex}"/>
    /// </summary>
    /// <typeparam name="TDialogIndex">Type of enum that is used to identify the dialogs.</typeparam>
    public class CustomDialogsManager<TDialogIndex> : CustomDialogsManagerBase<TDialogIndex>, IInternalDialogsManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogsManager{TDialogIndex}"/> class.
        /// </summary>
        /// <param name="fragmentManager">Application's fragment manager.</param>
        /// <param name="dialogsDictionary">Definitions matching TDialogIndex values with actual dialog providers.</param>
        /// <param name="viewModelResolver">Optional argument for allowing automatic resolution of ViewModels in dialogs.</param>
        public CustomDialogsManager(
            FragmentManager fragmentManager,
            Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary,
            ICustomDialogViewModelResolver viewModelResolver = null)
            : base(dialogsDictionary)
        {
            CustomDialogBase.CustomDialogViewModelResolver = viewModelResolver;
            CustomDialogBase.ConfiguredFragmentManager = fragmentManager;
            CustomDialogBase.DialogsManager = this;
        }
    }
}