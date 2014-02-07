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
    public class ItineraryTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBuilder = new StringBuilder();

            // Since value represents not well formatted XML which has no root element,
            // and the VirtualEarth prefix is not mapped to any namespace, we're
            // adding a fictitious root element which also maps the VirtualEarth prefix.
            string validXmlText = string.Format("<Directives xmlns:VirtualEarth=\"http://BingMaps\">{0}</Directives>", value);
            XDocument.Parse(validXmlText)
                     .Elements()
                     .Select(e => e.Value)
                     .ToList()
                     .ForEach(v => textBuilder.Append(v));

            string s = textBuilder.ToString();
            s = s.Replace("Depart", "Partir de ");
            s = s.Replace("toward", "hacia");
            s = s.Replace("Turnright onto", "Gire a la derecha en");
            s = s.Replace("Turnleft onto", "Gire a la izquierda en");
            s = s.Replace("Road name changes to", "Cambia de nombre a");
            if (s.Contains("Arrive at"))
                s = "Llega a destino";
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
