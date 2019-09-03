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
    /// <summary>
    /// Default dialogs manager for iOS.
    /// </summary>
    /// <typeparam name="TDialogIndex">Enum defining the dialog identifiers.</typeparam>
    public class CustomDialogsManager<TDialogIndex> : CustomDialogsManagerBase<TDialogIndex>, IInternalDialogsManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDialogsManager{TDialogIndex}"/> class.
        /// </summary>
        /// <param name="rootNavigationController">Main navigation controller.</param>
        /// <param name="dialogsDictionary">Dialogs dictionary matching identifiers with their providers.</param>
        /// <param name="dependencyResolver">Component capable of providing instances for given ViewModel type.</param>
        public CustomDialogsManager(
            UINavigationController rootNavigationController,
            Dictionary<TDialogIndex, ICustomDialogProvider> dialogsDictionary,
            ICustomDialogDependencyResolver dependencyResolver) 
            : base(dialogsDictionary)
        {
            CustomDialogBase.RootViewController = rootNavigationController;
            CustomDialogBase.CustomDialogDependencyResolver = dependencyResolver;
            CustomDialogBase.DialogsManager = this;
        }
    }
}