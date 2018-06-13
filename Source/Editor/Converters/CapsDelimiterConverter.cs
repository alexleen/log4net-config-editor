// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Editor.Converters
{
    public class CapsDelimiterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Replace(value.ToString(), "(\\B[A-Z])", " $1");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
