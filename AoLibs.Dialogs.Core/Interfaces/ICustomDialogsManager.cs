namespace AoLibs.Dialogs.Core.Interfaces
{ 
    /// <summary>
    /// Base non-generic interface for custom dialogs manager.
    /// </summary>
    public interface ICustomDialogsManager
    {
        /// <summary>
        /// Gets currently displayed dialog, null if none is presented.
        /// </summary>
        ICustomDialog CurrentlyDisplayedDialog { get; }
    }

    /// <summary>
    /// Interface for defining dialogs manager.
    /// </summary>
    /// <typeparam name="TDialogIndex">Enum defining dialog types.</typeparam>
    public interface ICustomDialogsManager<in TDialogIndex> : ICustomDialogsManager
    {
        /// <summary>
        /// Gets dialog associated with given argument.
        /// </summary>
        /// <param name="dialog">The dialog type to retrieve.</param>
        ICustomDialog this[TDialogIndex dialog] { get; }
    }
}
