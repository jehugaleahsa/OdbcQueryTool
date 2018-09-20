using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace OdbcQueryTool
{
    [ValueConversion(typeof(object), typeof(object))]
    [ContentProperty(nameof(Converters))]
    public class ConverterPipeline : DependencyObject, IValueConverter
    {
        private static readonly DependencyPropertyKey convertersPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Converters), 
            typeof(ObservableCollection<IValueConverter>), 
            typeof(ConverterPipeline), 
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ConvertersProperty = convertersPropertyKey.DependencyProperty;

        public ConverterPipeline()
        {
            SetValue(convertersPropertyKey, new ObservableCollection<IValueConverter>());
        }

        public ObservableCollection<IValueConverter> Converters => (ObservableCollection<IValueConverter>)GetValue(ConvertersProperty);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object nextValue = value;
            foreach (var converter in Converters)
            {
                var (nextSourceType, nextTargetType) = GetExpectedTypes(converter);
                nextValue = converter.Convert(nextValue, nextTargetType, parameter, culture);
                if (nextValue == Binding.DoNothing)
                {
                    return nextValue;
                }
            }
            return nextValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object nextValue = value;
            foreach (var converter in Converters.Reverse())
            {
                var (nextSourceType, nextTargetType) = GetExpectedTypes(converter);
                nextValue = converter.Convert(nextValue, nextTargetType, parameter, culture);
                if (nextValue == Binding.DoNothing)
                {
                    return nextValue;
                }
            }
            return nextValue;
        }

        private static (Type source, Type target) GetExpectedTypes(IValueConverter converter)
        {
            Type converterType = converter.GetType();
            var attribute = (ValueConversionAttribute)converterType.GetCustomAttributes(typeof(ValueConversionAttribute), true).FirstOrDefault();
            if (attribute == null)
            {
                throw new InvalidOperationException($"The target type could not be determined for the converter {converterType.FullName}");
            }
            return (attribute.SourceType, attribute.TargetType);
        }
    }
}
