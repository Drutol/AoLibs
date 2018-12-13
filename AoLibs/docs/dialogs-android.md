# Android Dialogs

The dialogs implemented with `CustomDialogBase` are working based on `DialogFragment`.

## Dialog Flavours

There total 3 classes building upon each other with additional functionalities

* CustomDialogBase
    * CustomViewModelDialogBase
        * CustomArgumentViewModelDialogBase

### CustomDialogBase

The base implementaton inheriting directly from `DialogFragment` and implementing methods of `ICustomDialog`.

#### Layout

There are no special requirements for layout that will be contained within the dialog. All you have to do is to provide its Id in `LayoutResourceId` property.

```cs
protected override int LayoutResourceId { get; } = Resource.Layout.test_dialog_a;
```

#### Bindings

This class provides flow for creating bindings between ViewModel and View controls. You will be required to implement `InitBindings()` method. For example:
```
protected override void InitBindings()
{
    Bindings.Add(this.SetBinding(() => ViewModel.Counter, () => TextView.Text)
        .ConvertSourceToTarget(i => i.ToString()));

    Button.SetOnClickCommand(ViewModel.IncrementCommand);
}
```

#### Awaiting Result

If the dialog was invoked using `AwaitResult()` method the awaited response type will be stored in `AwaitedResultType`. You can use `SetResult()` and `CancelResult()` to provide or cancel the result.

!!! note
    While you can pass anything as a result to `SetResult()` it will be checked against `AwaitedResultType` and `ArgumentException` will be thrown in case of mismatch.

#### Parameter

Passed parameter while invoking the dialog will be stored in `Parameter` property.

### CustomViewModelDialogBase

This inherited class will additionally take `TViewModel` generic parameter and will try to resolve given type using resolver passed to `CustomDialogsManager`.
The ViewModel's callbacks will be also invoked by this class.

### CustomArgumentViewModelDialogBase

This class in addition to requiring `TViewModel` will also require `TArgument` and will expose `Argument` property with casted parameter.

## CustomDialogsManager 

This class manages all the dialogs. During app start you will need to create its instance with passing your `FragmentManager` as well as dictionary of dialogs and providers. Additionally you can pass `ICustomDialogViewModelResolver` which will be used to obtain ViewModels from your IoC container or whatnot. For example:

```cs
protected override void OnCreate(Bundle savedInstanceState)
{
...
    var dialogDefinitions = new Dictionary<DialogIndex, ICustomDialogProvider>
    {
        {DialogIndex.TestDialogA, new OneshotCustomDialogProvider<TestDialogA>()},
        {DialogIndex.TestDialogB, new OneshotCustomDialogProvider<TestDialogB>()}
    };

    var dialogManager = new CustomDialogsManager<DialogIndex>(
        SupportFragmentManager,
        dialogDefinitions,
        new ViewModelResolver());
...
}

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