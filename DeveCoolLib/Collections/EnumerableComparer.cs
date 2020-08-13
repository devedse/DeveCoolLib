using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveCoolLib.Collections
{
    public static class EnumerableComparer
    {
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
            foreach(var equalSelector in equalizer)
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
