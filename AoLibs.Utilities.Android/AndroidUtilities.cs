using Android.Views;
using Java.Lang;

namespace AoLibs.Utilities.Android
{
    public static class AndroidUtilities
    {
        /// <summary>
        /// Reads the contants of <see cref="JavaObjectWrapper{TObj}"/>.
        /// </summary>
        /// <param name="obj">Wrapper to extract the data from.</param>
        /// <typeparam name="TObj">Object that is contained by the wrpper.</typeparam>
        public static TObj Unwrap<TObj>(this Java.Lang.Object obj) 
            where TObj : class
        {
            return (obj as JavaObjectWrapper<TObj>)?.Instance;
        }

        /// <summary>
        /// Wraps object with <see cref="JavaObjectWrapper{TObj}"/> so it can be assigned to <see cref="View.Tag"/> for example.
        /// </summary>
        /// <param name="obj">Object to wrap.</param>
        /// <typeparam name="TObj">Object that is contained by the wrpper.</typeparam>
        public static JavaObjectWrapper<TObj> Wrap<TObj>(this TObj obj) 
            where TObj : class
        {
            return new JavaObjectWrapper<TObj>(obj);
        }
    }
}