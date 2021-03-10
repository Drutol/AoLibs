using Android.Views;

namespace AoLibs.Utilities.Android
{
    /// <summary>
    /// Allows to wrap object with Java's one. Handy for assigning <see cref="View.Tag"/>.
    /// </summary>
    /// <typeparam name="TObj">Any type to be wrapped.</typeparam>
    public class JavaObjectWrapper<TObj> : Java.Lang.Object
    {
        public TObj Instance { get; }

        public JavaObjectWrapper(TObj obj)
        {
            Instance = obj;
        }
    }
}