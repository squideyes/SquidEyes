using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SquidEyes.Generic
{
    public static partial class CollectionExtenders
    {
        [DebuggerHidden]
        public static bool HasElements<T>(this List<T> list)
        {
            return list.HasElements(
                1, int.MaxValue, element => !element.IsDefault());
        }

        [DebuggerHidden]
        public static bool HasElements<T>(this List<T> list, Func<T, bool> isValid)
        {
            return list.HasElements(1, int.MaxValue, isValid);
        }

        [DebuggerHidden]
        public static bool HasElements<T>(this List<T> list, int minElements)
        {
            return list.HasElements(
                minElements, int.MaxValue, element => !element.IsDefault());
        }

        [DebuggerHidden]
        public static bool HasElements<T>(this List<T> list, int minElements,
            Func<T, bool> isValid)
        {
            return list.HasElements(minElements, int.MaxValue, isValid);
        }

        [DebuggerHidden]
        public static bool HasElements<T>(this List<T> list, int minElements,
            int maxElements)
        {
            return list.HasElements(minElements, maxElements, null);
        }

        [DebuggerHidden]
        public static bool HasElements<T>(this List<T> list, int minElements,
            int maxElements, Func<T, bool> isValid)
        {
            if (list == null)
                throw new ArgumentNullException("collection");

            if (minElements < 0)
                throw new ArgumentOutOfRangeException("minElements");

            if (maxElements < minElements)
                throw new ArgumentOutOfRangeException("maxElements");

            if (list.Count < minElements)
                return false;

            if (list.Count > maxElements)
                return false;

            if (isValid != null)
            {
                foreach (var item in list)
                {
                    if (item == null)
                        return false;

                    if (!isValid(item))
                        return false;
                }
            }

            return true;
        }

        [DebuggerHidden]
        public static bool HasElements<K, V>(this Dictionary<K, V> dictionary)
        {
            return dictionary.HasElements(1, int.MaxValue, value => true);
        }

        [DebuggerHidden]
        public static bool HasElements<K, V>(this Dictionary<K, V> dictionary,
            Func<V, bool> isValid)
        {
            return dictionary.HasElements(1, int.MaxValue, isValid);
        }

        [DebuggerHidden]
        public static bool HasElements<K, V>(this Dictionary<K, V> dictionary,
            int minElements)
        {
            return dictionary.HasElements(1, minElements, value => true);
        }

        [DebuggerHidden]
        public static bool HasElements<K, V>(this Dictionary<K, V> dictionary,
            int minElements, Func<V, bool> isValid)
        {
            return dictionary.HasElements(1, minElements, isValid);
        }

        [DebuggerHidden]
        public static bool HasElements<K, V>(this Dictionary<K, V> dictionary,
            int minElements, int maxElements)
        {
            return dictionary.HasElements(minElements, maxElements, value => true);
        }

        [DebuggerHidden]
        public static bool HasElements<K, V>(this Dictionary<K, V> dictionary,
            int minElements, int maxElements, Func<V, bool> isValid)
        {
            if (dictionary == null)
                throw new ArgumentNullException("collection");

            if (minElements < 0)
                throw new ArgumentOutOfRangeException("minElements");

            if (maxElements < minElements)
                throw new ArgumentOutOfRangeException("maxElements");

            if (dictionary.Count < minElements)
                return false;

            if (dictionary.Count > maxElements)
                return false;

            if (isValid != null)
            {
                foreach (var key in dictionary.Keys)
                    if (!isValid(dictionary[key]))
                        return false;
            }

            return true;
        }
    }
}
