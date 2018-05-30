namespace AoLibs.Utilities.Android
{
    public static class AndroidUtilities
    {
        public static TObj Unwrap<TObj>(this Java.Lang.Object obj) where TObj : class
        {
            return (obj as JavaObjectWrapper<TObj>)?.Instance;
        }

        public static JavaObjectWrapper<TObj> Wrap<TObj>(this TObj obj) where TObj : class
        {
            return new JavaObjectWrapper<TObj>(obj);
        }

        public static TEnum UnwrapEnum<TEnum>(this Java.Lang.Object obj)
        {
            return (TEnum) (object) (int) obj;
        }
    }
}