using System;
using System.Windows.Data;
using System.Globalization;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps.Platform;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Converts Microsoft.Phone.Controls.Maps.Platform.Location to System.Device.Location.GeoCoordinate
    /// </summary>
    public class LocationGeoCoordinateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var location = (Location)value;

            // There is an implicit case in the Location type.
            return (GeoCoordinate)location;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
