// Copyright © 2019 Alex Leendertsen

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
            if (value is AppenderModel appender && parameter is string param)
            {
                if (param == "file" && appender.Descriptor == AppenderDescriptor.File ||
                    param == "dir" && (appender.Descriptor == AppenderDescriptor.File ||
                                       appender.Descriptor == AppenderDescriptor.RollingFile))
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
