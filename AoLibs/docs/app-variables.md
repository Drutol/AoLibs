# AppVariables

It's a higher level mechanism that uses `ISettingsProvider` and `IFileStorageProvider` to provide easy typed access to stored data.

> Note: It's only meant for reference types, don't use simple types.

```cs
public class AppVariables : AppVariablesBase
{
    public AppVariables(ISettingsProvider settingsProvider, IDataCache dataCache = null) 
    		: base(settingsProvider, dataCache)
    {

    }

    public AppVariables(ISyncStorage syncStorage, IAsyncStorage asyncStorage = null) 
    		: base(syncStorage, asyncStorage)
    {

    }

    [Variable]
    public Holder<TokenModel> TokenModel { get; set; }

    [Variable]
    public Holder<VideoLibrary> VideoLibrary { get; set; }
}
```

Single property of type `Holder<T>` marked with `VariableAttribute` takes care of most of your storage needs.

## Holder

Wrapper that allows the library to interface with your data in "managed" way, it allows you to write the values to files or app settings via underlying implementations of `ISyncStorage` and `IAsyncStorage`. Out of the box it can handle `ISettingsProvider` as `ISyncStorage` and `IDataCache` as `IAsyncStorage`.

Your data is cached in memory and not read every time you request it.

## VariableAttribute

Allows to define additional behaviour for your data.

* `MemoryOnly`
	* Stores the data only in memory always skipping both storage options.
* `CustomKey`
	* By default property's name is used as *key* (file name or settings key), you can specify your own here.
* `ExpirationTime`
	* Defines for how long the data is valid. By default only supported in `IDataCache`, you will have to implement own `ISyncStorage` in order to consume this setting there.