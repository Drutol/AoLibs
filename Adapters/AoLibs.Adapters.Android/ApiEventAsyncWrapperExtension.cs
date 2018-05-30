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
    public static class ApiEventAsyncWrapperExtension
    {
        public static async Task<T> Await<T>(this IOnActivityEvent<T> activityEvent)
        {
            var tcs = new TaskCompletionSource<T>();

            void Handler(object sender, T arg)
            {
                tcs.SetResult(arg);
            }

            activityEvent.Received += Handler;
            var result = await tcs.Task;
            activityEvent.Received -= Handler;

            return result;
        }
    }
}