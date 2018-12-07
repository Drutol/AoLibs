namespace AoLibs.Dialogs.Core
{
    /// <summary>
    /// Utility ViewModel for dialogs with typed <see cref="TParameter"/>
    /// </summary>
    /// <typeparam name="TParameter">The type of used parameter for given dialog.</typeparam>
    public class CustomDialogViewModelWithParameterBase<TParameter> : CustomDialogViewModelBase
    {
        /// <inheritdoc />
        protected internal override void OnDialogAppearedInternal()
        {
            this.OnDialogAppeared();
            OnDialogAppeared(Dialog.Parameter == null ? default : (TParameter) Dialog.Parameter);
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