// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using Editor.ConfigProperties;

namespace Editor.Converters
{
    internal class PresetToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string preset)
            {
                return preset == Fix.CustomPreset;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
