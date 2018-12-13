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
        protected TArgument Argument { get; set; }

        protected CustomArgumentViewModelDialogBase(IntPtr handle)
            : base(handle)
        {
        }

        protected CustomArgumentViewModelDialogBase(string name, NSBundle p)
            : base(name, p)
        {
        }

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