using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SquidEyes.Generic
{
    public static class EnumHelper
    {
        public static List<ComboItem<T>> ToComboItems<T>(bool sort = false)
            where T : struct
        {
            var comboItems = new List<ComboItem<T>>();

            foreach (var enumeration in
                Enum.GetValues(typeof(T)).Cast<T>())
            {
                comboItems.Add(new ComboItem<T>(enumeration));
            }

            if (sort)
                comboItems.Sort();

            return comboItems;
        }

        public static T Toggle<T>(this T value)
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            var index = values.IndexOf(value);

            index++;

            if (index == values.Count)
                index = 0;

            return values[index];
        }

        public static string GetDescription<T>(T value)
        {
            Contract.Requires(value.IsDefined());

            return GetDescription(typeof(T), value);
        }

        public static string GetDescription(Type enumType, object value)
        {
            Contract.Requires(enumType != null);

            if (value == null)
                return string.Empty;

            var fi = enumType.GetField(value.ToString());

            var attributes = (DescriptionAttribute[])
                fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static List<T> ToList<T>() where T: struct
        {
            Contract.Requires(typeof(T).BaseType == typeof(Enum));

            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
