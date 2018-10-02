# iOS Navigation

## Introduction

It was certainly a challenge to bend native iOS navigation to match with overall concept and Android's capabilities. Finally I have achieved the freedom of managing the navigation stack however I see fit from shared code.

## Setup

As it's currently advised to use Storyboards I'll focus on them. What you essentially need is a navigation controller and a view controllers.

### NavigationViewController

This is how the most basic NavigationController may look like:

```cs
public partial class RootNavigationViewController : UINavigationController
    {
        public RootNavigationViewController (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // using attribute based navigation definition
            var manager = new NavigationManager<PageIndex>(
                navigationController: this,
                viewModelResolver: new ViewModelResolver());
            // or manually providing navigation definition
            var manager = new NavigationManager<PageIndex>(
                navigationController: this,
                pageDefinitions: new Dictionary<PageIndex, IPageProvider<INavigationPage>>
                {
                    {PageIndex.PageA, new StoryboardCachedPageProvider<TestPageAViewController>(
                        storyboardName: "Main",
                        viewControllerIdentifier: nameof(TestPageAViewController))},
                    ...
                },
                viewModelResolver: new ViewModelResolver());

            // store the manager somewhere to your liking   
            AppDelegate.Instance.NavigationManager = manager;

            ...
        }
    }
```

!!! hint
    For more information on attribute based navigation go [here](/navigation/#attribute-based-navigation).

    For more on `ViewModelResolver` you can go [here](/navigation/#viewmodel-injection)

### ViewController

Now we just need our ViewController to display the UI. For example:

```cs
    // used for attribute based definition, not needed when defining manually
    [NavigationPage(
        (int) PageIndex.PageA,
        NavigationPageAttribute.PageProvider.Cached,
        StoryboardName = "Main",
        ViewControllerIdentifier = nameof(TestPageAViewController))]
    public partial class TestPageAViewController : ViewControllerBase<TestViewModelA>
    {
        public TestPageAViewController(IntPtr handle) : base(handle)
        {

        }

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo();
        }

        public override void InitBindings()
        {
            ...
        }
    }
```

### Usage

Now your `INavigationManager` can be passed around to invoke navigation whenever you want to.

## Additional configuration

### TabBarViewControllers

Due to how UI is contructed on iOS it's not possible to abstract it enough, there are separate classes included such as:

* `ArgumentNavigationTabBarViewController` : `UITabBarController`
* `TabBarViewControllerBase<TViewModel>`

### Bindings

When we are talking navigation we are talking bindings lifecycle too. `ArgumentNavigationViewControler` class handles them too. Since the library is based on MVVMLight library we are using its bindings. 

You will want to add all of them to `Bindings` which is of type `List<Binding>`, they will be properly attached and reattached when needed.

You are supposed to add all your bindings in `InitBindings` method which is called once per controller instance or when bindings need to be recreated.

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