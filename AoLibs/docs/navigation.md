# Navigation

The goal is to provide lightweight solution for cross-platform navigation.

The main interface you will be using in your shared codebase is `INavigationManager<TPageIdentifier>` where `TPageIdentifier` is usually a simple enum with all your pages.

!!! example
    Below I'll describe shared concepts between Android and iOS. Setup and platform specific things are contained in separate pages.

## Page providers

The page definitions dictionary takes in pair of `TPageIdentifier` which is simple `enum` in most cases and instance of class that implements `IPageProvider<INavigationPage>`. There are two implemented as of now:

* `CachedPageProvider`
    * This provider preserves the state of its page so that when we navigate there it will look the same.
* `OneshotPageProvider`
    * Here we are providing the page instance just once, each navigation will create new uninitialized page. The use-case of it is to avoid manually cleaning pages like _RegisterPage_. I suggest disabling cache in your IoC container for associated ViewModel.

## Navigation lifecycle events

`INavigationPage` provides following events for you to override from platform specific class:
```cs
public virtual void NavigatedTo();
public virtual void NavigatedBack();
public virtual void NavigatedFrom();
```
* NavigatedTo
    * Is called when we navigate forward to this fragment.
        * eg. SignInPage -> DashboardPage
        * DashboardPage invokes `NavigatedTo`
* NavigatedBack
    * Called when we navigate backwards to the page we have already been on.
        * eg. SignInPage <- DashboardPage
        * SignInPage invokes `NavigatedBack`
* NavigatedFrom
    * Is called whenever we leave the page.

## Navigation with arguments

It's possible to invoke navigation with arguments. It allows us to separate ViewModels and avoid communication spaghetti code. 

`INavigationManager<TPageIdentifier>` allows to pass `args` parameter which is plain `System.Object`. These arguments will appear in `NavigationArguments` property within fragment class.

## Navigation backstack options

There are defined following navigation options:
```cs
public enum NavigationBackstackOption
{
    AddToBackstack,
    SetAsRootPage,
    ClearBackstackToFirstOccurence,
    NoBackstack,
    ForceNewPageInstance
}
```
* `AddToBackstack`
    * Basic default option, current page will be added to backstack when navigating.
* `SetAsRootPage`
    * Clears backstack and then navigates to given page essentially making it a root page.
* `ClearBackstackToFirstOccurence`
    * Allows to "jump" to page that is already on backstack, let's say you have registration process consisting of multiple pages and once you are done you need to get back to "Sign in" page, with this option you can go back multiple pages.
* `NoBackstack`
    * Current page won't be added to backstack on navigation.
* `ForceNewPageInstance`
    * Requests `IPageProvider` to recreate page instance before navigating.

## Attribute based navigation

When creating instance of `INavigationManager` there's a constructor overload that doesn't require you to pass dictionary mapping pages to providers.
This constructor will search for all types from calling assembly via `Assembly.GetCallingAssembly()` and search for classes marked with `NavigationPageAttribute`.
This attribute differs in Android vs iOS implementation but in its shared part it specifies:

* `PageProviderType`
    * Specifies whether to use `OneshotPageProvider` or `CachedPageProvider`
* `Page`
    * Specifies the integer value of an enum specified as *PageIndex*

```cs

[NavigationPage((int) PageIndex.PageA, NavigationPageAttribute.PageProvider.Cached)]
public class TestPageAFragment : FragmentBase<TestViewModelA>

```