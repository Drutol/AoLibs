# Android Layout to CSharp

I've built little application that is able to convert android layout defined in xml to C# code referencing controls with given ID.

!!! info
    You can find repo here: [https://github.com/Drutol/AndroidLayoutToCSharp](https://github.com/Drutol/AndroidLayoutToCSharp)

Finally you receive code to copy&paste:

```cs
#region Views

private FrameLayout _rootView;

public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));

#endregion
```

It's built as an UWP app for Windows 10. Manual compilation and deployment required.

## Features

It's capable of

* [x] Generating general "code-behind" for all controls marked with `android:id`
* [x] Recursively going through all files in specified folder in order to find controls added with `<include>` directive.
* [x] Generating code for ViewHolders.
* [x] Filtering IDs based on whether the ID starts with upper or lower character.
    * Useful for IDs that are only used by layout in `ConstraitLayout` scenarios for example.
