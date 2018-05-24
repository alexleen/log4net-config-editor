// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Editor.Converters
{
    public class ConsoleColorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ConsoleColor consoleColor)
            {
                string name = Enum.GetName(typeof(ConsoleColor), consoleColor);
                if (name != null)
                {
                    return string.Concat(name.Select(x => char.IsUpper(x) ? " " + x : x.ToString()));
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
