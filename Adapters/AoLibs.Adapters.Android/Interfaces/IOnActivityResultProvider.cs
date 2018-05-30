using Android.App;
using Android.Content;

namespace AoLibs.Adapters.Android.Interfaces
{
    public interface IOnActivityResultProvider : IOnActivityEvent<(int RequestCode, Result ResultCode, Intent Data)>
    {
       
    }
}