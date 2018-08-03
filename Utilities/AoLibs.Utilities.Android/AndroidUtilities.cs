using Android.Views;
using Java.Lang;

namespace AoLibs.Utilities.Android
{
    public static class AndroidUtilities
    {

        /// <summary>
        /// Reads the contants of <see cref="JavaObjectWrapper{TObj}"/>
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TObj Unwrap<TObj>(this Java.Lang.Object obj) where TObj : class
        {
            return (obj as JavaObjectWrapper<TObj>)?.Instance;
        }

        /// <summary>
        /// Wraps object with <see cref="JavaObjectWrapper{TObj}"/> so it can be assigned to <see cref="View.Tag"/> for example.
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static JavaObjectWrapper<TObj> Wrap<TObj>(this TObj obj) where TObj : class
        {
            return new JavaObjectWrapper<TObj>(obj);
        }

        /// <summary>
        /// Converts <see cref="Object"/> to given <see cref="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TEnum UnwrapEnum<TEnum>(this Java.Lang.Object obj)
        {
            return (TEnum) (object) (int) obj;
        }
    }
}