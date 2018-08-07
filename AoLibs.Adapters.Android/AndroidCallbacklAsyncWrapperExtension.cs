using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Interfaces;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Provides utility to easily await any <see cref="IOnActivityEvent{T}"/> provider.
    /// </summary>
    public static class AndroidCallbacklAsyncWrapperExtension
    {
        /// <summary>
        /// Awaits for given callback to be fired.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="activityEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> Await<T>(
            this IOnActivityEvent<T> activityEvent,
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<T>();
            cancellationToken.Register(() => tcs.SetCanceled());

            activityEvent.Received += Handler;
            var result = await tcs.Task;
            activityEvent.Received -= Handler;

            return result;

            void Handler(object sender, T arg)
            {
                tcs.SetResult(arg);
            }
        }
    }
}