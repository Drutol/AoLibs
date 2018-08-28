using System;
using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Provides functionality of presenting message boxes,
    /// additionally allows to streamline display of any loaders you may happen to have.
    /// </summary>
    public interface IMessageBoxProvider
    {
        /// <summary>
        /// Shows dialog allowing user to make an action.
        /// </summary>
        /// <param name="title">Title of the dialog.</param>
        /// <param name="content">Content of the dialog.</param>
        /// <param name="positiveText">Content on the button representing YES response.</param>
        /// <param name="negativeText">Content on the button representing NO response.</param>
        /// <returns>
        /// If user pressed YES then true else false.
        /// </returns>
        Task<bool> ShowMessageBoxWithInputAsync(string title, string content, string positiveText, string negativeText);
        
        /// <summary>
        /// Shows message box with only action being OK action.
        /// </summary>
        /// <param name="title">Title of the dialog.</param>
        /// <param name="content">Content of the dialog.</param>
        /// <param name="neutralText">Content on the button representing YES response.</param>
        Task ShowMessageBoxOkAsync(string title, string content, string neutralText);

        /// <summary>
        /// Gets disposable wrapper on the loader lifetime so you can use it in using block convenintly.
        /// Invokes <see cref="ShowLoadingPopup"/> and <see cref="HideLoadingDialog"/>
        /// </summary>
        IDisposable LoaderLifetime { get; }
       
        /// <summary>
        /// Provides disposable wrapper on the loader lifetime so you can use it in using block convenintly.
        /// Invokes <see cref="ShowLoadingPopup"/> and <see cref="HideLoadingDialog"/>. Customizatble version of <see cref="LoaderLifetime"/>
        /// </summary>
        /// <param name="title">Title of the loader.</param>
        /// <param name="content">Content of the loader.</param>
        IDisposable ObtainLoaderLifetime(string title, string content);

        /// <summary>
        /// Informs the provider to send signal to present your custom loading dialog.
        /// </summary>
        /// <param name="title">Title of the loader.</param>
        /// <param name="content">Content of the loader.</param>
        void ShowLoadingPopup(string title = null, string content = null);
        
        /// <summary>
        /// Informs the provider to send signal to hide your custom loading dialog.
        /// </summary>
        void HideLoadingDialog();
    }
}
