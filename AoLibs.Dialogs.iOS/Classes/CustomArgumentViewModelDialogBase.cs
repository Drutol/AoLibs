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
        private object _parameter;

        /// <summary>
        /// Gets the parameter that was passed to the dialog while invoking it, casted to given TArgument.
        /// </summary>
        protected TArgument Argument { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomArgumentViewModelDialogBase{TViewModel, TArgument}"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected CustomArgumentViewModelDialogBase(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomArgumentViewModelDialogBase{TViewModel, TArgument}"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="p">Bundle.</param>
        protected CustomArgumentViewModelDialogBase(string name, NSBundle p)
            : base(name, p)
        {
        }

        /// <inheritdoc />
        public override object Parameter
        {
            get => _parameter;
            set
            {
                _parameter = value;
                if (value is TArgument argument)
                    Argument = argument;
            }
        }
    }
}