using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Jeopardy.Core.Wpf.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool v && bool.TryParse((string)parameter, out var collapsed)
                ? v ? Visibility.Visible : collapsed ? Visibility.Collapsed : Visibility.Hidden
                : (object)true;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
