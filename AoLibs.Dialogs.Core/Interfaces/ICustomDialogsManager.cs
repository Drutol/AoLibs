namespace AoLibs.Dialogs.Core.Interfaces
{
    /// <summary>
    /// Interface for defining dialogs manager.
    /// </summary>
    /// <typeparam name="TDialogIndex">Enum defining dialog types.</typeparam>
    public interface ICustomDialogsManager<in TDialogIndex>
    {
        /// <summary>
        /// Gets dialog associated with given argument.
        /// </summary>
        /// <param name="dialog">The dialog type to retrieve.</param>
        ICustomDialog this[TDialogIndex dialog] { get; }
    }
}
