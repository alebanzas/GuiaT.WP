﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace GuiaTBAWP.Converters
{
    /// <summary>
    /// Data binding converter for converting an itinerary text to plain text.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;

            bool result;

            if(!bool.TryParse(value.ToString(), out result))
            {
                return Visibility.Collapsed;
            }

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
