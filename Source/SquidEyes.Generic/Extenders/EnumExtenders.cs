using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SquidEyes.Generic
{
    public static partial class EnumExtenders
    {
        [DebuggerHidden]
        public static bool IsEnum(this Type type, bool flags = false)
        {
            var isEnum = (Nullable.GetUnderlyingType(type) ?? type).IsEnum;

            if (!flags)
                return isEnum;

            return isEnum && (type.GetCustomAttributes(
                typeof(FlagsAttribute), false).Length != 0);
        }

        [DebuggerHidden]
        public static T ToEnum<T>(this string value) where T : struct
        {
            Contract.Requires(typeof(T).IsEnum());

            return (T)Enum.Parse(typeof(T), value, true);
        }

        [DebuggerHidden]
        public static T ToEnumOrDefault<T>(this string value, T defaultEnum) where T : struct
        {
            Contract.Requires(typeof(T).IsEnum());

            T enumeration;

            if (Enum.TryParse<T>(value, true, out enumeration))
                return enumeration;
            else
                return defaultEnum;
        }

        [DebuggerHidden]
        public static List<T> ToFlagList<T>(this T flags) where T : struct
        {
            Contract.Requires(typeof(T).IsEnum(true));

            var result = new List<T>();

            foreach (T flag in Enum.GetValues(typeof(T)))
                if (InFlags<T>(flag, flags))
                    result.Add(flag);

            return result;
        }

        [DebuggerHidden]
        public static bool InFlags<T>(this T flag, T flags) where T : struct
        {
            Contract.Requires(typeof(T).IsEnum(true));

            int bits = (int)Enum.ToObject(typeof(T), flag);
            int mask = (int)Enum.ToObject(typeof(T), flags);

            return ((bits & mask) == bits);
        }

        [DebuggerHidden]
        public static bool IsDefined<T>(this T value)
        {
            Contract.Requires(typeof(T).IsEnum());

            dynamic dyn = value;

            var max = Enum.GetValues(value.GetType()).
                Cast<dynamic>().Aggregate((e1, e2) => e1 | e2);

            return (max & dyn) == dyn;
        }
    }
}
