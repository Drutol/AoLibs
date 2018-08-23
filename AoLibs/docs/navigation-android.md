## Introduction

The idea behind this navigation mechanism is similar to how WPF/UWP's `<Frame>` element works. Namely we are have one root page that hosts all other pages. These pages are managed by library within this frame.

## Setup

All pages are hosted as `Fragments`, I'm assuming using single activity pattern.
Given a single android activity with layout:
```xml
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
              android:layout_width="match_parent"
              android:layout_height="match_parent"
              android:orientation="vertical">

  <FrameLayout android:layout_width="match_parent"
               android:layout_height="match_parent"
               android:id="@+id/RootView" />
</LinearLayout>
```
Now we will want to initialize library from code.
The _RootView_ `FrameLayout` will be hosting all our pages within itself.

```cs
protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    SetContentView(Resource.Layout.main);

    var pageDefinitions = new Dictionary<PageIndex, IPageProvider<NavigationFragmentBase>>
    {
        // cached
        {PageIndex.WelcomePage, new CachedPageProvider<WelcomePageFragment>()},
        // oneshots
        {PageIndex.SignInPage, new OneshotPageProvider<SignInPageFragment>()},
    };

    var manager = new NavigationManager<PageIndex>(
        fragmentManager: SupportFragmentManager,
        rootFrame: RootView,
        pageDefinitions: pageDefinitions)
}
```
Now we can happily use our `INavigationManager<TPageIdentifier>` in our ViewModels!

## Additional configuration

## Adding new pages

As we can see there are two pages defined `WelcomePageFragment` and `SignInPageFragment`. I'm providing base clases for fragments so it's very easy to add new content.

Type hierarchy:

* `INavigationPage`

    * `NavigationFragmentBase`

        * `FragmentBase<TViewModel>`


`NavigationFragmentBase` inherits from Android's `Fragment` class and wraps its functionality. Let's say we want to create new page _SplashPage_.
1. Create new class called for example `SplashPageFragment`
2. Make it inherit from `FragmentBase<TViewModel>`
```cs
public class SplashPageFragment : FragmentBase<SplashViewModel>
{
    public override int LayoutResourceId { get; } = Resource.Layout.splash_page;

    protected override void InitBindings()
    {

    }
}
```
3. There are 2 required elements:
  * `LayoutResourceId` which indicates which layout the fragment is associated with.
  * `InitBinings` method in which we will define our bindings to ViewModel.
4. Add new entry in `pageDefinitions`

## Transition animations

If you want to include transition animations when navigating you will want to use `Action<FragmentTransaction> interceptTransaction` parameter of `NavigationManager`'s constructor. It will expose `FragmentTransaction` so you change whatever you want before actually committing new page. For example:
```cs
private void InterceptTransaction(FragmentTransaction fragmentTransaction)
{
    fragmentTransaction.SetCustomAnimations(
        Resource.Animator.animation_slide_bottom,
        Resource.Animator.animation_fade_out);
}
```

## ViewModel injection

The generic parameter of `FragmentBase<TViewModel>` allows you to specify which ViewModel to pull for given fragment. ViewModel will be available in `ViewModel` property in your fragment.
This requires providing implementation of `IViewModelResolver` interface which will pull appropriate ViewModels from your IoC for example.
```cs
private class ViewModelResolver : IViewModelResolver
{
    public TViewModel Resolve<TViewModel>()
    {
        using (var scope = ViewModelLocator.ObtainScope())
        {
            return scope.Resolve<TViewModel>();
        }
    }
}
```

This `ViewModelResolver` can be assigned to static property found on `NavigationFragmentBase`.
```cs
NavigationFragmentBase.ViewModelResolver = new ViewModelResolver();
```

## Bindings

When we are talking navigation we are talking bindings lifecycle too. `FragmentBase` class handles them too. Since the library is based on MVVMLight library we are using its bindings. 

You will want to add all of them to `Bindings` which is of type `List<Binding>`, they will be properly attached and reattached when needed.

You are supposed to add all your bindings in `InitBindings` method which is called once per fragment instance or when bindigs need to be recreated.

If you don't have any bindings added to `Bindings`, yet you don't want the method to be fired again you can call this constructor:
```cs
public NavigationFragmentBase(bool hasNonTrackableBindings = false);
```
Example:
```cs
protected override void InitBindings()
{
    Bindings.Add(this.SetBinding(() => ViewModel.Toggle).WhenSourceChanges(() =>
    {
        ToggleValue.Text = ViewModel.Toggle ? "ON" : "OFF";
    }));

    Bindings.Add(this.SetBinding(() => ViewModel.Value, () => Value.Text));
}
```
Disclaimer: `Value` and `ToggleValue` are `TextViews`.

## Notes

* The _MainActivity_ will have to inherit `AppCompatActivity`.
