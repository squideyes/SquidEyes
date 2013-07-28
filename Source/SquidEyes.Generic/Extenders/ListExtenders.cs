using System.Collections.Generic;

namespace SquidEyes.Generic
{
    public static partial class ListExtenders
    {
        public static List<T> Clone<T>(this List<T> list)
        {
            return new List<T>(list);
        }
    }
}
