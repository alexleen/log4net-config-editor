// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using Editor.Descriptors;

namespace Editor.Converters
{
    public class LayoutToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LayoutDescriptor descriptor)
            {
                return descriptor != LayoutDescriptor.Simple;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
