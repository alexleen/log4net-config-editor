// Copyright © 2018 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Editor.Converters
{
    public class ReferencesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage image = new BitmapImage();

            if (value is int incomingReferences)
            {
                if (incomingReferences > 0)
                {
                    image = new BitmapImage(new Uri("pack://application:,,,/Editor;component/Images/dialog-ok-apply.png"));
                }
                else
                {
                    image = new BitmapImage(new Uri("pack://application:,,,/Editor;component/Images/dialog-warning.png"));
                }
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
