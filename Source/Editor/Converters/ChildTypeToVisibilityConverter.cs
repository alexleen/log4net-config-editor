// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Editor.Models.ConfigChildren;

namespace Editor.Converters
{
    internal class ChildTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is AppenderModel ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
