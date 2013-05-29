using System.Device.Location;
using System.Globalization;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Text converters
    /// </summary>
    public class TextConverter
    {
        public static string ToLocationString(GeoCoordinate coordinate)
        {
            return coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + ", " + coordinate.Longitude.ToString(CultureInfo.InvariantCulture);
        }
    }
}
