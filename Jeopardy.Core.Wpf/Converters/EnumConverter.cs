using System;
using System.Globalization;
using System.Windows.Data;
using static Jeopardy.Core.Wpf.MarkupExtensions.EnumerationExtension;

namespace Jeopardy.Core.Wpf.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            if (value is EnumerationMember underlyingValue)
            {
                if (targetType.IsEnum)
                {
                    // convert int to enum
                    return Enum.ToObject(targetType, underlyingValue.Value);
                }
            }

            if (value.GetType().IsEnum)
            {
                // convert enum to int
                return System.Convert.ChangeType(
                    value,
                    Enum.GetUnderlyingType(value.GetType()));
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
