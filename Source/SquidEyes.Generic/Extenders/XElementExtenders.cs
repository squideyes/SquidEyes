using System;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public static partial class XElementExtenders
    {
        public static string NullOrValue(this XElement element)
        {
            if (element == null)
                return (string)null;
            else
                return element.Value;
        }

        public static T ToEnum<T>(this XElement element) 
        {
            if (element == null)
                return default(T);

            if (string.IsNullOrWhiteSpace(element.Value))
                return default(T);

            return (T)Enum.Parse(typeof(T), element.Value, true);
        }
    }
}
