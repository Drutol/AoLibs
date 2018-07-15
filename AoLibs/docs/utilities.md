# Utilities

Set of various little helpers that I found useful.

## DateTimeExtensions

`DateTime DateTimeFromUnixTimestamp(this int timestamp)`

Takes epoch timestamp and converts it to `DateTime` object.

`int ToUnixTimestamp(this DateTime date)`

Converts `DateTime` to epoch integer.

## DiffUtility

Helps diffing 2 collections. There are two extension methods for this utility:

* `DiffResult<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> other, CompareDelegate<T> referenceComparer, IEqualityComparer<T> equalityComparer)`
    * You can provide `IEqualityComparer<T>` that will be used to compare the items.
* `DiffResult<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> other, CompareDelegate<T> referenceComparer, CompareDelegate<T> equalityComparer = null)`
    * If you don't `equalityComparer` default `.Equals()` method will be utilised.

Arguments:

* first
    * Original collection
* other
    * Altered collection
* referenceComparer
    * Delegate that allows to differentiate same entities, you want to compare IDs here for example.
* equalityComparer
    * Indicates whether the item differs in its contents from other.

On output you will receive `DiffResult<T>` object:

```cs
public class DiffResult<T>
{
    public IEnumerable<T> Added { get; internal set; }
    public IEnumerable<T> Removed { get; internal set; }
    public IEnumerable<T> Unmodified { get; internal set; }
    public IEnumerable<T> Modified { get; internal set; }
}   
```

All these operations defined in `IEnumerable<T>` are held back until they are enumerated.

## FileSizeUtility

`string GetHumanReadableBytesLength(long value)`

Takes byte count and outputs proper size string.
Possible suffixes:
```cs
SizeSuffixes = {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
```

## SmartObservableCollection

Extension over `ObservableCollection<T>`. The _smart_ part comes from addition of  `void AddRange(IEnumerable<T> range)` method. Proper collection changed events will be issued for newly added items only in a batch.

## StringExtensions

* `string FirstCharToUpper(this string input)`
* `string FirstCharToLower(this string input)`
* `string Wrap(this string s, string start, string end)`
* `string TrimWhitespaceInside(this string str, bool allWhitespce = true)`
    * Removes whitespace in the middle of the string, `allWhitespace` specifies if regex will be working with `\s` or just `" "`.

## StringUtilities

`static int LevenshteinDistance(string s, string t)`

Calculates simple Levenshtein distance between given strings.