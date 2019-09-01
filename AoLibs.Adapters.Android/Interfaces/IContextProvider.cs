using Android.App;
using Android.Content;

namespace AoLibs.Adapters.Android.Interfaces
{
    /// <summary>
    /// Interface used by library components to resolve current application context.
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Gets current context.
        /// </summary>
        Context CurrentContext { get; }     

        /// <summary>
        /// Gets current activity.
        /// </summary>
        Activity CurrentActivity { get; }
    }
}