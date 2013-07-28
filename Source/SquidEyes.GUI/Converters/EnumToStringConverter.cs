using System;
using System.Globalization;
using System.Windows.Data;
using SquidEyes.Generic;

namespace SquidEyes.GUI
{
    public class EnumToStringConverter : AbstractConverter, IValueConverter
    {
        private Type enumType;

        public EnumToStringConverter()
        {
        }

        public EnumToStringConverter(Type enumType)
        {
            this.EnumType = enumType;
        }

        public Type EnumType
        {
            get
            {
                return this.enumType;
            }
            set
            {
                if (this.enumType != value)
                {
                    if (value != null)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException(
                                "The \"EnumType\" property must be set to an Enum type!");
                    }

                    this.enumType = value;
                }
            }
        }

        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return EnumHelper.GetDescription(EnumType, value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException(
                "The \"ConvertBack\" method is inoperative!");
        }
    }
}
