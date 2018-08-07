using System.Collections.Generic;
using System.Linq;

namespace AoLibs.Utilities.Shared
{
    public static class DiffUtility
    {
        public delegate bool CompareDelegate<in T>(T x, T y);

        public class DiffResult<T>
        {
            internal DiffResult()
            {

            }

            public IEnumerable<T> Added { get; internal set; }
            public IEnumerable<T> Removed { get; internal set; }
            public IEnumerable<T> Unmodified { get; internal set; }
            public IEnumerable<T> Modified { get; internal set; }
        }

        /// <summary>
        /// Performs diff on two <see cref="IEnumerable{T}"/> Provides Additions, Removals, Specifies which items were modified and which stayed as is.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">Original collection.</param>
        /// <param name="other">Updated collection.</param>
        /// <param name="referenceComparer">Delegate allowing to match items representing same entities, by Ids for example.</param>
        /// <param name="equalityComparer">Comparer that will be used to determine item's equality.</param>
        public static DiffResult<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> other,
            CompareDelegate<T> referenceComparer, IEqualityComparer<T> equalityComparer)
        {
            return Diff(first, other, referenceComparer, equalityComparer.Equals);
        }

        /// <summary>
        /// Performs diff on two <see cref="IEnumerable{T}"/> Provides Additions, Removals, Specifies which items were modified and which stayed as is.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">Original collection.</param>
        /// <param name="other">Updated collection.</param>
        /// <param name="referenceComparer">Delegate allowing to match items representing same entities, by Ids for example.</param>
        /// <param name="equalityComparer">Delegate that specifies custom equality logic, <see cref="object.Equals(object)"/> will be used otherwise.</param>
        public static DiffResult<T> Diff<T>(this IEnumerable<T> first, IEnumerable<T> other,  CompareDelegate<T> referenceComparer, CompareDelegate<T> equalityComparer = null)
        {
     
            
            //if comparer wasn't provided we use standard Equals
            if (equalityComparer is null)
                equalityComparer = (x, y) => x.Equals(y);

            //evaluate enumerables
            var firstList = first.ToList();
            var otherList = other.ToList();

            var result = new DiffResult<T>
            {
                Added = ProcessAdditions(firstList, otherList, referenceComparer),
                Removed = ProcessRemovals(firstList, otherList, referenceComparer),
                Modified = ProcessModifications(firstList, otherList, referenceComparer, equalityComparer),
                Unmodified = ProcessUnmodifiedItems(firstList, otherList, referenceComparer, equalityComparer),
            };
            
            
            return result;
        }

        private static IEnumerable<T> ProcessAdditions<T>(IEnumerable<T> first, IEnumerable<T> other, CompareDelegate<T> comparer)
        {
            return other.Where(item => !first.Any(otherItem => comparer(item, otherItem)));
        }

        private static IEnumerable<T> ProcessRemovals<T>(IEnumerable<T> first, IEnumerable<T> other, CompareDelegate<T> comparer)
        {
            return first.Where(item => !other.Any(otherItem => comparer(item, otherItem)));
        }

        private static IEnumerable<T> ProcessModifications<T>(IEnumerable<T> first, IEnumerable<T> other, CompareDelegate<T> referenceComparer, CompareDelegate<T> equalityComparer)
        {
            foreach (var item in first)
            {
                var matchingItem = other.FirstOrDefault(arg => referenceComparer(arg, item));
                if (matchingItem == null) continue;

                if (!equalityComparer(item, matchingItem))
                    yield return matchingItem;
            }
        }

        private static IEnumerable<T> ProcessUnmodifiedItems<T>(IEnumerable<T> first, IEnumerable<T> other, CompareDelegate<T> referenceComparer, CompareDelegate<T> equalityComparer)
        {
            foreach (var item in first)
            {
                var matchingItem = other.FirstOrDefault(arg => referenceComparer(arg, item));
                if (matchingItem == null) continue;

                if (equalityComparer(item, matchingItem))
                    yield return matchingItem;
            }
        }
    }
}
