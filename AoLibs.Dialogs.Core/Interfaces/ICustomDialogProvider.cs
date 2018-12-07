namespace AoLibs.Dialogs.Core.Interfaces
{
    /// <summary>
    /// Interface used for defining dialog providers.
    /// </summary>
    public interface ICustomDialogProvider
    {
        /// <summary>
        /// Gets the dialog instance.
        /// </summary>
        ICustomDialog Dialog { get; }
    }
}
