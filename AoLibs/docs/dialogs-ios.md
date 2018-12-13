#iOS Dialogs

The iOS dialogs are presented as a modal ViewControllers with separate container view to which custom dialog layout will be injected.

## Dialog Flavours

There total 3 classes building upon each other with additional functionalities

* CustomDialogBase
    * CustomViewModelDialogBase
        * CustomArgumentViewModelDialogBase

### CustomDialogBase

The base implementaton inheriting directly from `DialogFragment` and implementing methods of `ICustomDialog`.

This dialog needs to have attached `CustomDialogAttribute` defining which dialog it is and its storyboard properties.

#### Layout

The layout of the dialog is taken from ViewController found in storyboard defined by `CustomDialogAttribute`.

#### Bindings

This class provides flow for creating bindings between ViewModel and View controls. You will be required to implement `InitBindings()` method. For example:
```
public override void InitBindings()
{
    Bindings.Add(this.SetBinding(() => ViewModel.Counter, () => CounterLabel.Text));
    DoStuffButton.SetOnClickCommand(ViewModel.IncrementCommand);
}
```

#### Awaiting Result

If the dialog was invoked using `AwaitResult()` method the awaited response type will be stored in `AwaitedResultType`. You can use `SetResult()` and `CancelResult()` to provide or cancel the result.

!!! note
    While you can pass anything as a result to `SetResult()` it will be checked against `AwaitedResultType` and `ArgumentException` will be thrown in case of mismatch.

#### Parameter

Passed parameter while invoking the dialog will be stored in `Parameter` property.

#### Additional customization

You can customize whether the dialog will show/hide with animation or not using thse two properties:
```cs
public virtual bool ShouldAnimateOnShow { get; } = true;
public virtual bool ShouldAnimateOnDismiss { get; } = true;
```

### CustomViewModelDialogBase

This inherited class will additionally take `TViewModel` generic parameter and will try to resolve given type using resolver passed to `CustomDialogsManager`.
The ViewModel's callbacks will be also invoked by this class.

### CustomArgumentViewModelDialogBase

This class in addition to requiring `TViewModel` will also require `TArgument` and will expose `Argument` property with casted parameter.

## CustomDialogsManager 

This class manages all the dialogs. During app start you will need to create its instance with passing your parent `UINavigationController` as well as dictionary of dialogs and providers. Additionally you can pass `ICustomDialogViewModelResolver` which will be used to obtain ViewModels from your IoC container or whatnot. For example:

```cs

var dialogDefinitions = new Dictionary<DialogIndex, ICustomDialogProvider>
{
    {DialogIndex.TestDialogA, new StoryboardOneshotCustomDialogProvider<TestDialogAViewController>()},
    {DialogIndex.TestDialogB, new StoryboardOneshotCustomDialogProvider<TestDialogBViewController>()},
};

var dialogManager = new CustomDialogsManager<DialogIndex>(
    this,
    dialogDefinitions,
    new ViewModelResolver());

private class ViewModelResolver : ICustomDialogViewModelResolver
{
    TViewModel ICustomDialogViewModelResolver.Resolve<TViewModel>() 
    {
        using (var scope = ResourceLocator.ObtainScope())
        {
            return scope.Resolve<TViewModel>();
        }
    }
}
```

!!! warning
    Take note of `StoryboardOneshotCustomDialogProvider` which is a subclass of `OneshotDialogProvider`. Because base implementation of `OneshotDialogProvider` uses `Activator` to create the dialog instance it's not vaiable in iOS scenario where we need to use `UIStoryboard.FromName()` in order instantiate the dialog's ViewController.