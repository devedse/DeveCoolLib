using System;
using System.Collections.Generic;

namespace DeveCoolLib.Collections
{
    public static class ListSynchronizerV3_WithUpdate
    {
        public static void SynchronizeLists<T, TKey>(IList<T> data, IList<T> desiredData, Func<T, TKey> selector, Action<T, T> updateAction) where T : class
        {
            var compareResult = EnumerableComparer.CompareEnumerables(data, desiredData, selector);

            foreach (var item in compareResult.Removed)
            {
                data.Remove(item);
            }

            foreach (var item in compareResult.Updated)
            {
                updateAction(item.Old, item.New);
            }

            foreach (var item in compareResult.Added)
            {
                data.Add(item);
            }
        }
    }
}
