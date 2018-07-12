# AoLibs.Navigation

## Concept

The idea behind this navigation mechanism is simillar to how WPF/UWP's `<Frame>` element works. Namely we are have one root page that hosts all other pages. These pages are managed by libary within this frame.

## Android

### Setup
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

### Adding new pages

As we can see there are two pages defined `WelcomePageFragment` and `SignInPageFragment`. I'm providing base clases for fragments so it's very easy to add new content.

Type hierarchy:
* `INavigationPage`
  * `NavigationFragmentBase`
    * `FragmentBase<TViewModel>`

`NavigationFragmentBase` inherits from Andoroid's `Fragment` class and wraps its functionality. Let's say we want to create new page _SplashPage_.
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

### Some details

#### Page providers

The page definitions dictionary takes in pair of `TPageIdentifier` which is simple `enum` in most cases and instance of class that implements `IPageProvider<NavigationFragmentBase>`. There are two implemented as of now:
* `CachedPageProvider`
  * This provider preserves the state of its page so that when we navigate there it will look the same.
* `OneshotPageProvider`
  * Here we are providing the page instance just once, each navigation will create new uninitilized page. The use-case of it is to avoid manually cleaning pages like _RegisterPage_. I suggest disabling cache in your IoC container for associated ViewModel.

#### Transition animations

If you want to include transition animations when navigating you will want to use `Action<FragmentTransaction> interceptTransaction` parameter of `NavigationManager`'s constructor. It will expose `FragmentTransaction` so you change whatever you want before actually commiting new page. For example:
```cs
private void InterceptTransaction(FragmentTransaction fragmentTransaction)
{
    fragmentTransaction.SetCustomAnimations(
        Resource.Animator.animation_slide_bottom,
        Resource.Animator.animation_fade_out);
}
```

#### ViewModel injection

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

#### Navigation lifecycle events

`NavigationFragmentBase` provides following events for you to override:
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
  * Called when we navigate backwards to the page we have alraedy been on.
    * eg. SignInPage <- DashboardPage
    * SignInPage invokes `NavigatedBack`
* NavigatedFrom
  * Is called whenever we leave the page

#### Navigation with arguments

#### Bindings

### Notes
* The _MainActivity_ will have to inherit `AppCompatActivity`.
