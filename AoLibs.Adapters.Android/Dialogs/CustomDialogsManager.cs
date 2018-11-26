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
    public class CustomDialogsManager<TDialogIndex> : CustomDialogsManagerBase<TDialogIndex>
    {
        private readonly FragmentManager _fragmentManager;
        private readonly ICustomDialogViewModelResolver _viewModelResolver;

        public CustomDialogsManager(
            FragmentManager fragmentManager,
            Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary,
            ICustomDialogViewModelResolver viewModelResolver = null)
            : base(dialogsDictionary)
        {
            _fragmentManager = fragmentManager;
            _viewModelResolver = viewModelResolver;

            CustomDialogBase.CustomDialogViewModelResolver = _viewModelResolver;
            CustomDialogBase.FragmentManager = _fragmentManager;
        }
    }
}