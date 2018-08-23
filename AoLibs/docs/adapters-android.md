# Android Adapters

## IContextProvider

Some adapters require context to work, that's why they are accepting `IContextProvider` argument. 
You should provide your own implementation depending on your architecture.

For example:

```cs
private class ContextProvider : IContextProvider
{
    public Activity CurrentContext => MainActivity.Instance;
}
```

## IOnActivityEvent

Is a mechanism that helps wrapping activity callbacks and delegating them in a more streamlined way.

Let's consider:

```cs
public class MainActivity : AppCompatActivity, IOnNewIntentProvider
{

    ...

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        _activityNewIntentEventHandler.Invoke(this, intent);
    }

    event EventHandler<Intent> IOnActivityEvent<Intent>.Received
    {
        add => _activityNewIntentEventHandler += value;
        remove => _activityNewIntentEventHandler -= value;
    }
}
```
Where `IOnNewIntentProvider` derives from `IOnActivityEvent<T>`.

You are required to manually provide delegation of given event like above. Then you can register them like so for example:

```cs
containerBuilder.Register(ctx => MainActivity.Instance)
                .As<IOnActivityResultProvider>()
                ...
                .As<IOnNewIntentProvider>();
```
### AsyncWrapperExtension

While the whole thing with `IOnActivityEvent<T>` may look cumbersome at first, it for sure saved me quite a bit of time later on.
There's `AndroidCallbacklAsyncWrapperExtension` class which contains extension method:

```cs
public static async Task<T> Await<T>(
    this IOnActivityEvent<T> activityEvent,
    CancellationToken cancellationToken = default)
```

It lets you *await* callbacks without writing devious logic every time.
