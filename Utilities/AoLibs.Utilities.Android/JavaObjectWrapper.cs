namespace AoLibs.Utilities.Android
{
    public class JavaObjectWrapper<TObj> : Java.Lang.Object
    {
        public TObj Instance { get; }

        public JavaObjectWrapper(TObj obj)
        {
            Instance = obj;
        }
    }
}