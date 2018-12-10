using AoLibs.Dialogs.Core.Interfaces;

namespace AoLibs.Dialogs.Android.Interfaces
{
    internal interface IInternalDialogsManager : ICustomDialogsManager
    {
        new ICustomDialog CurrentlyDisplayedDialog { get; set; }
    }
}