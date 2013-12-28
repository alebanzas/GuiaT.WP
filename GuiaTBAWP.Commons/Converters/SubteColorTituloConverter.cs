using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Shapes;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting a line number to image source
    /// </summary>
    public class SubteColorTituloConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "White";

            try
            {
                var linestatus = value.ToString().ToLowerInvariant();

                return linestatus.Contains("normal") ? "White" : "Yellow";
            }
            catch (Exception)
            {
                return "White";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
