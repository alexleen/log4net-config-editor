// Copyright © 2018 Alex Leendertsen

using Editor.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Editor.Converters
{
    public class SaveStateToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage image = new BitmapImage();

            if (value is SaveState saveState)
            {
                switch (saveState)
                {
                    case SaveState.Unknown:
                        break;
                    case SaveState.Changed:
                        image = new BitmapImage(new Uri("pack://application:,,,/Editor;component/Images/dialog-warning.png"));
                        break;
                    case SaveState.Saving:
                        image = new BitmapImage(new Uri("pack://application:,,,/Editor;component/Images/view-refresh.png"));
                        break;
                    case SaveState.Saved:
                        image = new BitmapImage(new Uri("pack://application:,,,/Editor;component/Images/dialog-ok-apply.png"));
                        break;
                    default:
                        break;
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
