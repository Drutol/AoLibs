using Android.App;
using Android.Content;

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Can be used to easily await activity result event. See <see cref="AndroidCallbacklAsyncWrapperExtension.Await{T}"/> for handy usage.
    /// </summary>
    public interface IOnActivityResultProvider 
        : IOnActivityEvent<(int RequestCode, Result ResultCode, Intent Data)>
    {       
    }
}