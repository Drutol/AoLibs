using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Dialogs
{
    public interface ICustomDialog
    {
        object Parameter { get; set; }

        void Show();
        void Hide();
        Task ShowAsync();
        Task HideAsync();

        Task<TResult> AwaitResult<TResult>(CancellationToken token = default);
    }
}
