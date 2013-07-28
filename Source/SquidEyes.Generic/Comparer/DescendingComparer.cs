using System;
using System.Collections.Generic;

namespace SquidEyes.Generic
{
    public class DescendingComparer<T> : IComparer<T> 
        where T : IComparable<T>
    {
        public int Compare(T a, T b)
        {
            return b.CompareTo(a);
        }
    }
}
