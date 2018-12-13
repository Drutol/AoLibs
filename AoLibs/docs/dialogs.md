# Dialogs

This set of libraries is meant to provide easy management of dialogs from ViewModels in your Xamarin apps. It provides means of invoking dialogs, awaiting their results and streamlinining dialog creation in respective platform specific projects.

!!! question
    The dialogs here are not stock message dialogs provided by the platforms. The dialogs here are modal windows with completly custom UI required for more specific scenarios.

!!! hint
    In the essence the mechanism here is very simillar to AoLibs' naviagtion solution.

I tried to find the best balance between passing instances as `object` arguments and using generics in order not to make the code overly complex.

## Setup

The dialog consists of its `ViewModel` paired with implementation of `ICustomDialog` interface in platform projects. All registered dialogs are handled by `ICustomDialogsManager` which can be injected into your page ViewModels. `ICustomDialog` defines methods to manipulate the dialog.

Additional caveats will be described in subpages for each platfrom.

### Quick Start

Steps:

* Define an enum which will contain identifiers for all your dialogs, `DialogIndex` for example.
* Create dialog's ViewModel inheriting `CustomDialogViewModelBase` in your shared project.
* Create implementation of `ICustomDialog` in your platform specific project by inheriting `CustomDialogBase`.
* During app startup create instance of `CustomDialogsManagerBase` passing dictionary matching dialog identifiers with providers of your's dialog definition.
* Inject `ICustomDialogsManager` to your page ViewModel and access the dialog.

## Dialog

Dialog is defined by `ICustomDialog` interface and is implemented separately on each platorm. Aside from basic funtions like Show&Hide it allows to await result:
```cs
Task<TResult> AwaitResult<TResult>(CancellationToken token = default);
```
The dialog also has its parameter to give a context for its invocation, you can pass it to `Show()` method. You can pass anything there as it's an argument of `object` type.

## ViewModel

The ViewModel for our dialog must inherit from `CustomDialogViewModelBase` which provides callbacks for dialog's lifetime as well as utility methods to access the dialog itself and manage the possibly awaited result of dialog's execution. Additionally from ViewModel you can define simple dialog config shared by both platforms.

### Features

* Dialog config
    
    You can override `CustomDialogConfig` property in order to provide custom config defining basic characteristics of a dialog such as:
    
    *  `Gravity` - defining side of screen where dialog should be shown.
    *  `IsCancellable` - can dialog be cancelled by tapping outside or using back button.
    *  `StretchHorizontally` - whether to stretch container horizontally
    *  `StretchVertically`- whether to stretch container vertically

* Access to dialog instance
    
    You can access the dialog itself with additional methods to manipulate the dialog's result.

* Lifecycle callbacks

    You can override methods that indicate that dialog has been shown or hidden.

### Flavours

With `CustomDialogViewModelBase` being the very base dialog ViewModel there are others that provide additional functionality.

* `CustomDialogViewModelWithParameterBase` allows to define generic parameter of the argument that will be passed when showing the dialog. Additional method to override is provided `OnDialogAppeared` which will pass given type instead of an `object`