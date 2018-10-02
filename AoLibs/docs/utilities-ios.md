#iOS Utilities

### Commands

Set of 4 extension methods that help setting `ICommand` for `View`:

* `void SetOnClickCommand(this UIView view, ICommand command)`
* `void SetOnClickCommand(this UIView view, ICommand command, Action<UIView,bool> onCanExecuteChanged)`
* `void SetOnClickCommand(this UIView view, ICommand command, object arg)`
* `void SetOnClickCommand(this UIView view, ICommand command, object arg, Action<UIView, bool> onCanExecuteChanged)`

Depending on whether passed `view` is instance of `UIControl` these methods will work slightly different in the backstage.

* UIView
    * `UITapGestureRecognizer` will be added to the view for triggering command.
    * `UIView.UserInteractionEnabled` will be manipulated to apply command's `CanExecute()` state.
* UIControl
    * Subscription will be made to `UIControl.TouchUpInside` for triggering command.
    * `UIControl.Enabled` will be changed to reflect the `CanExecute()` state of the command.
    
!!! info
    Repeated calls will not cause multiple subsriptions.

These commands will subscribe to `CanExecuteChanged` event. By default `Enabled` or `UserInteractionEnabled` property will be changed but you can also use overload with `Action<View,bool> onCanExecuteChanged` argument to customize behaviour. The other 2 variants allow passing `object arg` argument to `ICommand`'s `Execute()` and `CanExecute()` methods.

