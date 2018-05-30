using System;

namespace AoLibs.Adapters.Android.Interfaces
{
    public interface IOnActivityEvent<T>
    {
        event EventHandler<T> Received;
    }
}