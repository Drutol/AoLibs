using System;
using System.Threading;
using System.Threading.Tasks;

namespace AoLibs.Dialogs.Core.Interfaces
{
    /// <summary>
    /// Base interface defining basic actions on cross-platform dialog.
    /// </summary>
    public interface ICustomDialog
    {
        /// <summary>
        /// Fired when dialog is shown.
        /// </summary>
        event EventHandler DialogShown;

        /// <summary>
        /// Fired when dialog is hidden.
        /// </summary>
        event EventHandler DialogHidden;

        /// <summary>
        /// Gets or sets a parameter with which the dialog was run.
        /// </summary>
        object Parameter { get; set; }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="parameter">Additional parameter that can be retrieved later within the dialog and ViewModel.</param>
        void Show(object parameter = null);

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        void Hide();

        /// <summary>
        /// Shows the dialog asynchronously.
        /// </summary>
        /// <param name="parameter">Additional parameter that can be retrieved later within the dialog and ViewModel.</param>
        Task ShowAsync(object parameter = null);

        /// <summary>
        /// Hides the dialog asynchronously.
        /// </summary>
        Task HideAsync();

        /// <summary>
        /// Awaits dialog result.
        /// </summary>
        /// <typeparam name="TResult">Desired return type.</typeparam>
        /// <param name="token">Cancellation token.</param>
        Task<TResult> AwaitResult<TResult>(CancellationToken token = default);
    }
}
