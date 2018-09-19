using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OdbcQueryTool
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException();
            }
            var visible = (bool)value;
            if (Invert)
            {
                visible = !visible;
            }
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException();
            }
            var visibility = (Visibility)value;
            var isVisible = visibility == Visibility.Visible;
            if (Invert)
            {
                isVisible = !isVisible;
            }
            return isVisible;
        }
    }
}
