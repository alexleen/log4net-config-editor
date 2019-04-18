// Copyright © 2019 Alex Leendertsen

using System;
using System.Globalization;
using System.Windows.Data;
using Editor.Enums;

namespace Editor.Converters
{
    public class SaveStateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = null;

            if (value is SaveState saveState)
            {
                switch (saveState)
                {
                    case SaveState.Unknown:
                        break;
                    case SaveState.Changed:
                        text = "Unsaved Changes";
                        break;
                    case SaveState.Saving:
                        text = "Saving...";
                        break;
                    case SaveState.Saved:
                        text = "All Changes Saved";
                        break;
                    default:
                        break;
                }
            }

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
