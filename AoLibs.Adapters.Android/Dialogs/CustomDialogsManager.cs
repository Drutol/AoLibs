using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Core.Dialogs;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace AoLibs.Adapters.Android.Dialogs
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
        /// <param name="fragmentManager">Appliaction's fragment manager.</param>
        /// <param name="dialogsDictionary">Definitions matching <see cref="TDialogIndex"/> values with actual dialog providers.</param>
        /// <param name="viewModelResolver">Optional argument for allowing automatic resolution of ViewModels in dialogs.</param>
        public CustomDialogsManager(
            FragmentManager fragmentManager,
            Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary,
            ICustomDialogViewModelResolver viewModelResolver = null)
            : base(dialogsDictionary)
        {
            CustomDialogBase.CustomDialogViewModelResolver = viewModelResolver;
            CustomDialogBase.ConfiguredFragmentManager = fragmentManager;
        }
    }
}