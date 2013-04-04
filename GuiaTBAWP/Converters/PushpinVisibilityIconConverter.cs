using System;
using System.Windows;
using System.Windows.Data;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting a pushpin type to brush.
    /// </summary>
    public class PushpinVisibilityIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val;
            if (int.TryParse(value.ToString(), out val))
            {
                if(val != 0)
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
