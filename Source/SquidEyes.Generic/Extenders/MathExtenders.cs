using System;
using System.Diagnostics.Contracts;
using SquidEyes;

namespace SquidEyes.Generic
{
    public static partial class MathExtenders
    {
        public static double Truncate(this double number, int decimals)
        {
            Contract.Requires(decimals.InRange(2, 6));

            var factor = Math.Pow(10, decimals);

            return Math.Round(Math.Truncate(number * factor) / factor, decimals);
        }
    }
}
