// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using System.Xml;

namespace Editor.Converters
{
    public class RefSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XmlNode node)
            {
                if (node.Attributes?["type"]?.Value == "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async")
                {
                    string name = node.Attributes?["name"]?.Value;
                    return name ?? "Async Forwarder";
                }

                return node.Name;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
