using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting a pushpin type to brush.
    /// </summary>
    public class PushpinTypeBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var brushKey = string.Format("{0}Brush", value);
            var brush = Application.Current.Resources[brushKey] ?? DefaultBrush;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        static PushpinTypeBrushConverter()
        {
            DefaultBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x37, 0x84, 0xDF));
        }

        public static Brush DefaultBrush;
    }
}
