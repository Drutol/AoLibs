using AoLibs.Dialogs.Core;

namespace AoLibs.Dialogs.Android
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
        /// <summary>
        /// Gets the parameter casted to TArgument.
        /// </summary>
        protected TArgument Argument { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomArgumentViewModelDialogBase{TViewModel, TArgument}"/> class.
        /// </summary>
        protected CustomArgumentViewModelDialogBase()
        {
            if (Parameter is TArgument param)
                Argument = param;
        }
    }
}