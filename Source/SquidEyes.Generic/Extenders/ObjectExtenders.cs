using System;
using System.Diagnostics;

namespace SquidEyes.Generic
{
    public static partial class ObjectExtenders
    {
        [DebuggerHidden]
        public static bool IsDefault<T>(this T value)
        {
            return (Equals(value, default(T)));
        }

        [DebuggerHidden]
        public static string ToNewString<T>(this T value)
        {
            return string.Copy(value.ToString());
        }
    }
}
