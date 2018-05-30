using System;
using System.Collections.Generic;

namespace AoLibs.Utilities.Shared
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static void IndexedForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static int FindIndex<T>(this IEnumerable<T> source, T obj)
        {
            var index = 0;
            foreach (T element in source)
            {
                if (element.Equals(obj))
                    return index;
                index++;
            }
            return -1;
        }

        public static int FindIndex<T>(this IEnumerable<T> source, Predicate<T> matchPredicate)
        {
            var index = 0;
            foreach (T element in source)
            {
                if (matchPredicate.Invoke(element))
                    return index;
                index++;
            }
            return -1;
        }

    }
}
