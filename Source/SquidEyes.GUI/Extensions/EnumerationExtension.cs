using SquidEyes.Generic;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Markup;

namespace SquidEyes.GUI
{
    public class EnumerationExtension : MarkupExtension
    {
        public class ProvidedValue
        {
            public object Value { get; internal set; }
            public string Description { get; internal set; }
        }

        public EnumerationExtension(Type enumType)
        {
            Contract.Requires(enumType != null);
            Contract.Requires(enumType.IsEnum());

            EnumType = enumType;
        }

        public Type EnumType { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Contract.Requires(serviceProvider != null);

            return (from object enumValue in Enum.GetValues(EnumType)
                    select new ProvidedValue
                    {
                        Value = enumValue,
                        Description = GetDescription(enumValue)
                    }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType.
                GetField(enumValue.ToString()).
                GetCustomAttributes(typeof(DescriptionAttribute), false).
                FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute != null ?
                descriptionAttribute.Description : enumValue.ToString();
        }
    }
}
