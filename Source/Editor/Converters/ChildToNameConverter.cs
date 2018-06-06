// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using Editor.Models;

namespace Editor.Converters
{
    public class ChildToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChildModel childModel)
            {
                if (childModel is AppenderModel appenderModel)
                {
                    return appenderModel.Name;
                }

                return childModel.ElementName;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
