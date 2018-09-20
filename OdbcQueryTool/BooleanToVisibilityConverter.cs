using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OdbcQueryTool
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(bool) || targetType != typeof(Visibility))
            {
                throw new InvalidOperationException();
            }
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Visibility) || targetType != typeof(bool))
            {
                throw new InvalidOperationException();
            }
            return (Visibility)value == Visibility.Visible;
        }
    }
}
