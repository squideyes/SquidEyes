using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace SquidEyes.Generic
{
    public static partial class NumericExtenders
    {
        private const int SIGN_MASK = ~int.MinValue;

        private const int KB = 1024;
        private const int MB = KB * 1024;
        private const int GB = MB * 1024;

        public static int GetDecimals(this decimal value)
        {
            return (decimal.GetBits(value)[3] & SIGN_MASK) >> 16;
        }

        public static int GetDecimals(this double value)
        {
            return GetDecimals(checked((decimal)value));
        }

        public static int GetDecimals(this float value)
        {
            return GetDecimals(checked((decimal)value));
        }

        public static string ToGB(this int size, int decimals)
        {
            Contract.Requires(size.InRange(0, int.MaxValue));
            Contract.Requires(decimals.InRange(0, 2));

            return Math.Round((double)size / GB, decimals) + "GB";
        }

        public static string ToMB(this int size, int decimals)
        {
            Contract.Requires(size.InRange(0, int.MaxValue));
            Contract.Requires(decimals.InRange(0, 2));

            return Math.Round((double)size / MB, decimals) + "MB";
        }

        public static string ToKB(this int size, int decimals)
        {
            Contract.Requires(size.InRange(0, int.MaxValue));
            Contract.Requires(decimals.InRange(0, 2));

            return Math.Round((double)size / KB, decimals) + "KB";
        }
    }
}
