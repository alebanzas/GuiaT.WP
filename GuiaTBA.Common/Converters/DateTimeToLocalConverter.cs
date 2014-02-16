using System;
using System.Windows.Data;
using System.Globalization;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting an itinerary text to plain text.
    /// </summary>
    public class DateTimeToLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            DateTime result;

            if(!DateTime.TryParse(value.ToString(), out result))
            {
                return string.Empty;
            }

            return "Ultima actualización: " + string.Format("{0} {1}", result.ToLocalTime().ToShortDateString(), result.ToLocalTime().ToShortTimeString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
