using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace SquidEyes.Generic
{
    public static partial class OperatorExtenders
    {
        [DebuggerHidden]
        public static bool InRange<T>(this T value, MinMax<T> minMax) 
            where T : struct, IComparable<T>
        {
            return value.InRange<T>(minMax.MinValue, minMax.MaxValue);
        }

        [DebuggerHidden]
        public static bool InRange<T>(this T value, T minValue, T maxValue)
            where T : struct, IComparable<T>
        {
            Contract.Requires(maxValue.CompareTo(minValue) >= 0);

            return (value.CompareTo(minValue) >= 0) &&
                (value.CompareTo(maxValue) <= 0);
        }

        [DebuggerHidden]
        public static bool IsGreaterThan<T>(this T value, T minValue)
            where T : struct, IComparable<T>
        {
            return (value.CompareTo(minValue) > 0);
        }

        [DebuggerHidden]
        public static bool IsGreaterThanOrEqualTo<T>(this T value, T minValue)
            where T : struct, IComparable<T>
        {
            return (value.CompareTo(minValue) >= 0);
        }

        [DebuggerHidden]
        public static bool IsLessThan<T>(this T value, T maxValue)
            where T : struct, IComparable<T>
        {
            return (value.CompareTo(maxValue) < 0);
        }

        [DebuggerHidden]
        public static bool IsLessThanOrEqualTo<T>(this T value, T maxValue)
            where T : struct, IComparable<T>
        {
            return (value.CompareTo(maxValue) <= 0);
        }
    }
}