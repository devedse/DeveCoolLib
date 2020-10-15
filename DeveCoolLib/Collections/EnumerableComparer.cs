using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveCoolLib.Collections
{
    public static class EnumerableComparer
    {
        /// <summary>
        /// This method compares 2 enumerables. Please ensure that there's only 1 item with the same ID per enumerable
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="oldList">The old enumerable</param>
        /// <param name="newList">The new enumerable</param>
        /// <param name="selector">The selector to compare between old and new items. This should point to a property that's unique per enumerable</param>
        /// <returns>The compare results</returns>
        public static IEnumerableCompareResults<T> CompareEnumerables<T, TKey>(IEnumerable<T> oldList, IEnumerable<T> newList, Func<T, TKey> selector)
        {
            var addedItems = newList.Where(newItem => !oldList.Any(oldItem => selector(newItem).Equals(selector(oldItem)))).ToList();
            var removedItems = oldList.Where(oldItem => !newList.Any(newItem => selector(oldItem).Equals(selector(newItem)))).ToList();

            var updatedItems = new List<IEnumerableCompareItemResult<T>>();

            foreach (var oldItem in oldList)
            {
                var oldItemSelector = selector(oldItem);

                var found = newList.FirstOrDefault(newItem => oldItemSelector.Equals(selector(newItem)));
                if (found != null)
                {
                    updatedItems.Add(new IEnumerableCompareItemResult<T>(oldItem, found));
                }
            }
            return new IEnumerableCompareResults<T>(addedItems, removedItems, updatedItems);
        }

        public static bool SequenceEqual<T>(IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> equalizer)
        {
            return Enumerable.SequenceEqual(first, second, EqualityComparerFactory.Create<T>((t) => t.GetHashCode(), equalizer));
        }

        public static bool SequenceEqual<T>(IEnumerable<T> first, IEnumerable<T> second, params Func<T, object>[] equalizer)
        {
            if (equalizer == null || equalizer.Length == 0)
            {
                throw new ArgumentException("equalizer should contain at least one equals property", nameof(equalizer));
            }

            return SequenceEqual(first, second, (first, second) => EqualsFunc(first, second, equalizer));
        }

        private static bool EqualsFunc<T>(T first, T second, Func<T, object>[] equalizer)
        {
            foreach (var equalSelector in equalizer)
            {
                if (!equalSelector(first).Equals(equalSelector(second)))
                {
                    return false;
                }
            }
            return true;
        }

        //public static bool SequenceEqualUnordered<T>(IList<T> first, IList<T> second, Func<T, T, bool> equalizer)
        //{
        //    return Enumerable.SequenceEqual(first, second, EqualityComparerFactory.Create<T>((t) => t.GetHashCode(), equalizer));
        //}
    }
}
