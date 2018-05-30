using Android.App;

namespace AoLibs.Adapters.Android.Interfaces
{
    public interface IContextProvider
    {
        Activity CurrentContext { get; }
    }
}