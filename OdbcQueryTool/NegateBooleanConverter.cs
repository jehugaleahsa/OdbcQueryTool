using System;
using System.Globalization;
using System.Windows.Data;

namespace OdbcQueryTool
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Negate(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Negate(value, targetType);
        }

        private static object Negate(object value, Type targetType)
        {
            if (value == null || value.GetType() != typeof(bool) || targetType != typeof(bool))
            {
                throw new InvalidOperationException();
            }
            return !(bool)value;
        }
    }
}
