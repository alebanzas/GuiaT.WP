using System;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Text converters
    /// </summary>
    public class NumberConverter
    {
        public static double SetSigFigs(double d, int digits)
        {
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

            return scale * Math.Round(d / scale, digits);
        }
    }
}
