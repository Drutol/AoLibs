# Adapters

The library comes packed with quite a few adapters that will help you avoid writing boilerplate code for platfrom dependednt functionalities.

> Adapter is a class which provides same functionality that has to be implemented differently on different platforms.

## Provided adapters

Main interfaces which can be used in shared codebase are found in `AoLibs.Adapters.Core`.

* IClipboardProvider
* IDataCache
* IDispatcherAdapter
* IFileStorageProvider
* ILifecycleInfoProvider
* IMessageBoxProvider
* IPhoneCallAdapter
* IPhotoPickerAdapter
* IPickerAdapter
* ISettingsProvider
* IUriLauncherAdapter
* IVersionProvider

I'll provide more details on the ones that may be not obvious.

## DataCache

It depends on `IFileStorageProvider`. Uses Json.NET to serialize given data and save it to file for storage. Object are wrapped in object with timestamp so it's possible to specify for how long this piece of data stays valid.

## DispatcherAdapter

Runs action on UI thread... that's it.

## LifecycleInfoProvider

Requires manual invocation of methods.

## SettingsProvider

Provides set of methods allowing to storage data in application's settings construct.