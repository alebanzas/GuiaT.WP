using System;
using System.Windows.Data;
using System.Globalization;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting a line number to image source
    /// </summary>
    public class SubteNombreImagenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            try
            {
                var lineletter = value.ToString().Split(' ')[1];

                return string.Format("/Images/Subtes/linea{0}.png", lineletter.ToUpperInvariant());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
