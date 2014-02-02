using System.Globalization;
using System.Windows.Media;

namespace GuiaTBAWP.Commons.Helpers
{
    public class ColorTranslator
    {
        public static Color FromHtml(string hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;

            var start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            var r = byte.Parse(hex.Substring(start, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(start + 2, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(start + 4, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}