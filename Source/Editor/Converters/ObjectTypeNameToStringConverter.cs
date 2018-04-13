using System;
using System.Globalization;
using System.Windows.Data;

namespace Editor.Converters
{
    public class ObjectTypeNameToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.GetType().Name : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}