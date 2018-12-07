using System;
using AoLibs.Dialogs.Core.Interfaces;

namespace AoLibs.Dialogs.Core
{
    // private for now as there are caching issues on Android
    class CachedCustomDialogProvider<TDialog> : ICustomDialogProvider 
        where TDialog : ICustomDialog
    {
        protected Func<TDialog> Factory { get; set; }

        private ICustomDialog _page;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedCustomDialogProvider{TDialog}"/> class.
        /// Creates new instance, the page will be created using <see cref="Activator.CreateInstance{T}"/>, be sure it can be instantinated this way.
        /// </summary>
        public CachedCustomDialogProvider()
        {
            Factory = Activator.CreateInstance<TDialog>;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedCustomDialogProvider{TDialog}"/> class.
        /// Creates new instance setting up the provider with provided page.
        /// </summary>
        /// <param name="instance">Dialog to be used by provider.</param>
        /// <param name="factory">Optional factory to reinstantinate the page if need arises. <see cref="Activator.CreateInstance{T}"/> will be used if null.</param>
        public CachedCustomDialogProvider(TDialog instance, Func<TDialog> factory = null)
        {
            Factory = factory ?? Activator.CreateInstance<TDialog>;
            Dialog = instance;
        }

        /// <summary>
        /// Gets or sets instance of held page.
        /// </summary>
        public ICustomDialog Dialog
        {
            get { return _page ?? (_page = Factory()); }

            private set => _page = value;
        }
    }
}
