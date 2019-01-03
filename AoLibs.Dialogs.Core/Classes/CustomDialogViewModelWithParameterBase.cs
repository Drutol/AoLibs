namespace AoLibs.Dialogs.Core
{
    /// <summary>
    /// Utility ViewModel for dialogs with typed TParameter.
    /// </summary>
    /// <typeparam name="TParameter">The type of used parameter for given dialog.</typeparam>
    public class CustomDialogViewModelWithParameterBase<TParameter> : CustomDialogViewModelBase
    {
        /// <inheritdoc />
        protected internal override void OnDialogAppearedInternal()
        {
#pragma warning disable SA1000 // Keywords must be spaced correctly
            OnDialogAppeared(Dialog.Parameter == null ? default : (TParameter) Dialog.Parameter);
#pragma warning restore SA1000 // Keywords must be spaced correctly
        }

        /// <summary>
        /// Callback for when the dialog appears.
        /// </summary>
        /// <param name="parameter">Passed parameter when the dialog was invoked.</param>
        protected virtual void OnDialogAppeared(TParameter parameter)
        {
        }
    }
}