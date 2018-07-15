# Android Utilities

## Views

* `ScrollableSwipeToRefreshLayout`
    * Extension to `SwipeRefreshLayout`, allows to specify View the has scrolling capabilities in `ScrollingView` property. It will be later used to determine whether to or not allow refresh gesture. This gesture is only allowed while on the very top of the scrolling view.
    * `CanRefresh` property will be used if `SwipeRefreshLayout` equals `null`

## Helpers

* `JavaObjectWrapper<T>`
    * Simple wrapper so that we can easily place purely C# classes withing `Tag` property of `View`.
    * Goes well with with provided extension methods:
        * `static T Unwrap<T>(this Java.Lang.Object obj)`
        * `static JavaObjectWrapper<T> Wrap<T>(this T obj)`
* `DimensionsHelper`
    * Converts _dp_ to _px_ or _px_ to _dp_

### Commands

Set of 4 extension methods that help setting `ICommand` for `View`:

* `void SetOnClickCommand(this View view, ICommand command)`
* `void SetOnClickCommand(this View view, ICommand command, Action<View,bool> onCanExecuteChanged)`
* `void SetOnClickCommand(this View view, ICommand command, object arg)`
* `void SetOnClickCommand(this View view, ICommand command, object arg, Action<View, bool> onCanExecuteChanged)`

These commands will subscribe to `CanExecuteChanged` event. By default `Enabled` property will be changed but you can also use overload with `Action<View,bool> onCanExecuteChanged` argument to customize behaviour. The other 2 variants allow passing `object arg` argument to `ICommand`'s `Execute()` method.

### Memory watcher

Memory on android is precious. `MemoryWatcher` singleton allows to alert when the free memory drops below given percentage.