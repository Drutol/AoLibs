using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.Adapters.Core.Dialogs
{
    public class OneshotCustomDialogProvider<TDialog> : ICustomDialogProvider 
        where TDialog : ICustomDialog
    {
        protected Func<TDialog> Factory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneshotCustomDialogProvider{TPage}"/> class.
        /// </summary>
        /// <param name="factory">Factory used to build actual dialog.</param>
        public OneshotCustomDialogProvider(Func<TDialog> factory)
        {
            Factory = factory.Invoke;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneshotCustomDialogProvider{TPage}"/> class.
        /// <see cref="Activator.CreateInstance{T}"/> will be used for instantiation.
        /// </summary>
        public OneshotCustomDialogProvider()
        {
            Factory = Activator.CreateInstance<TDialog>;
        }

        /// <summary>
        /// Gets instance of held page.
        /// </summary>
        public ICustomDialog Dialog => Factory.Invoke();

    }
}
