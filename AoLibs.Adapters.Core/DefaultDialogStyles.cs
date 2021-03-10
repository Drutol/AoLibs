using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Core
{
    /// <summary>
    /// Static class where the default values for <see cref="IMessageBoxProvider"/> methods that will be used unless
    /// custom one aren't provided.
    /// </summary>
    public static class DefaultDialogStyles
    {
        public static INativeDialogStyle PasswordInputDialogStyle { get; set; }
        public static INativeDialogStyle DialogStyle { get; set; }
        public static INativeDialogStyle LoadingDialogStyle { get; set; }
    }
}
