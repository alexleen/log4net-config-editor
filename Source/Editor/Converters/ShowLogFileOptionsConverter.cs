using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Editor.Descriptors;
using Editor.Models.ConfigChildren;

namespace Editor.Converters
{
    public class ShowLogFileOptionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AppenderModel appender && appender.Descriptor == AppenderDescriptor.File)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}