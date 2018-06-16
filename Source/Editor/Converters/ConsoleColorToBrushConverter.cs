// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Editor.Converters
{
    public class ConsoleColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = Brushes.Transparent;

            if (value is ConsoleColor consoleColor)
            {
                if (consoleColor == ConsoleColor.DarkYellow)
                {
                    //DarkYellow is "special". See https://stackoverflow.com/a/12340136/7355697
                    brush = Brushes.DarkGoldenrod;
                }
                else
                {
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString(Enum.GetName(typeof(ConsoleColor), consoleColor));
                }
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
