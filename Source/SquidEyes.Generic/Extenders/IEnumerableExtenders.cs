using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace SquidEyes.Generic
{
    public static partial class IEnumerableExtenders
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return (items == null) || (!items.Any());
        }

        public static string ToConcatenatedString<T>(
            this IEnumerable<T> source, Func<T, string> stringSelector)
        {
            return source.ToConcatenatedString(stringSelector, string.Empty);
        }

        public static string ToConcatenatedString<T>(this IEnumerable<T> source,
            Func<T, string> stringSelector, string separator)
        {
            var sb = new StringBuilder();

            bool needsSeparator = false;

            foreach (var item in source)
            {
                if (needsSeparator)
                    sb.Append(separator);

                sb.Append(stringSelector(item));

                needsSeparator = true;
            }

            return sb.ToString();
        }
    }
}
