﻿using System;
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
        /// Fired when dialog is about to be shown.
        /// </summary>
        event EventHandler DialogWillShow;

        /// <summary>
        /// Fired when dialog is about to be hidden.
        /// </summary>
        event EventHandler DialogWillHide;

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
        /// <param name="parameter">Parameter passed to the dialog.</param>
        /// <param name="token">Cancellation token.</param>
        Task<TResult> ShowAndAwaitResult<TResult>(object parameter = null, CancellationToken token = default);
    }
}
