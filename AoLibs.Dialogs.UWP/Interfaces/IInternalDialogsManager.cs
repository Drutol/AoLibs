using AoLibs.Dialogs.Core.Interfaces;

namespace AoLibs.Dialogs.UWP.Interfaces
{
    internal interface IInternalDialogsManager : ICustomDialogsManager
    {
        new ICustomDialog CurrentlyDisplayedDialog { get; set; }
    }
}