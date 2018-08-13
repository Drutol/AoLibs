using Android.Content;

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Can be used to easily await new intent. See <see cref="AndroidCallbacklAsyncWrapperExtension.Await{T}"/> for handy usage.
    /// </summary>
    public interface IOnNewIntentProvider :
        IOnActivityEvent<Intent>
    {    
    }
}