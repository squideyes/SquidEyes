using System;
using System.Diagnostics;

namespace SquidEyes.Generic
{
    public static partial class DoubleExtenders
    {
        [DebuggerHidden]
        public static bool Approximates(this double a, double b)
        {
            return Approximates(a, b, 1.0E+15);
        }

        [DebuggerHidden]
        public static bool Approximates(this double a, double b,
            int decimals)
        {
            if ((decimals < 1) || (decimals > 15))
                throw new ArgumentOutOfRangeException("decimals");

            return Approximates(a, b, Math.Pow(10, decimals - 1));
        }

        [DebuggerHidden]
        public static bool Approximates(this double a, double b,
            double fraction)
        {
            if (fraction <= 0)
                throw new ArgumentOutOfRangeException("fraction");

            if (a == b)
                return true;

            return Math.Abs(a - b) < Math.Abs(a / fraction);
        }
    }
}
