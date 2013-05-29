using System;
using System.Text;
using System.Linq;
using System.Windows.Data;
using System.Xml.Linq;
using System.Globalization;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting an itinerary text to plain text.
    /// </summary>
    public class DistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            float result;

            if(!float.TryParse(value.ToString(), out result))
            {
                return string.Empty;
            }

            if(float.IsNaN(result))
            {
                return string.Empty;
            }

            if(result < 1)
            {
                return (result*1000).ToString(CultureInfo.InvariantCulture) + "m";
            }

            return result + "Km";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
