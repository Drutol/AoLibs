using System;
using Android.App;

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Interface used as a base for activity based events like <see cref="Activity.OnActivityResult"/>
    /// </summary>
    /// <typeparam name="T">Event arguments type.</typeparam>
    public interface IOnActivityEvent<T>
    {
        event EventHandler<T> Received;
    }
}