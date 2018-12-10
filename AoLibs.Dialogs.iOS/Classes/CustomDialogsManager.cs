using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Dialogs.iOS.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    public class CustomDialogsManager<TDialogIndex> : CustomDialogsManagerBase<TDialogIndex>, IInternalDialogsManager
    {
        public CustomDialogsManager(
            UINavigationController rootNavigationController,
            Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary,
            ICustomDialogViewModelResolver viewModelResolver) 
            : base(dialogsDictionary)
        {
            CustomDialogBase.RootViewController = rootNavigationController;
            CustomDialogBase.CustomDialogViewModelResolver = viewModelResolver;
            CustomDialogBase.DialogsManager = this;
        }
    }
}