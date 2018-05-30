using System;
using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface IMessageBoxProvider
    {
        Task<bool> ShowMessageBoxWithInputAsync(string title, string content, string positiveText, string negativeText);
        Task ShowMessageBoxOkAsync(string title, string content, string neutralText);

        IDisposable LoaderLifetime { get; }
        void ShowLoadingPopup(string title = null, string content = null);
        void HideLoadingDialog();
    }
}
