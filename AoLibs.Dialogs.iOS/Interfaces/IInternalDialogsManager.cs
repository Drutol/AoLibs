using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoLibs.Dialogs.Core.Interfaces;
using Foundation;
using UIKit;

namespace AoLibs.Dialogs.iOS.Interfaces
{
    internal interface IInternalDialogsManager : ICustomDialogsManager
    {
        new ICustomDialog CurrentlyDisplayedDialog { get; set; }
    }
}