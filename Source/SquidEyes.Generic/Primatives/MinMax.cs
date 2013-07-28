using System;
using System.Diagnostics.Contracts;

namespace SquidEyes.Generic
{
    public struct MinMax<T> where T : IComparable<T>
    {
        private T minValue;
        private T maxValue;

        public MinMax(T minValue, T maxValue)
        {
            Contract.Requires(maxValue. CompareTo(minValue) >= 0);

            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public T MinValue
        {
            get
            {
                return minValue;
            }
        }

        public T MaxValue
        {
            get
            {
                return maxValue;
            }
        }
    }
}
