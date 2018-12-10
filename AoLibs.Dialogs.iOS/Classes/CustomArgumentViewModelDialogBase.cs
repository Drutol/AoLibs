using System;
using AoLibs.Dialogs.Core;
using Foundation;

namespace AoLibs.Dialogs.iOS
{
    /// <summary>
    /// Dialog base with possibility of providing the type of ViewModel associated with given dialog, and parameter it will be using.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel.</typeparam>
    /// <typeparam name="TArgument">The Argument.</typeparam>
    public abstract class CustomArgumentViewModelDialogBase<TViewModel,TArgument> 
        : CustomViewModelDialogBase<TViewModel>
        where TViewModel : CustomDialogViewModelBase
    {
        protected TArgument Argument { get; private set; }

        protected CustomArgumentViewModelDialogBase(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        protected CustomArgumentViewModelDialogBase(string name, NSBundle p)
            : base(name, p)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomArgumentViewModelDialogBase{TViewModel, TArgument}"/> class.
        /// </summary>
        protected void Initialize()
        {
            if (Parameter is TArgument param)
                Argument = param;
        }
    }
}