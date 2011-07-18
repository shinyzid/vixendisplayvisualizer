// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    ///   The color to color name converter.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToColorNameConverter : IValueConverter
    {
        /// <summary>
        ///   The convert.
        /// </summary>
        /// <param name = "value">
        ///   The value.
        /// </param>
        /// <param name = "targetType">
        ///   The target type.
        /// </param>
        /// <param name = "parameter">
        ///   The parameter.
        /// </param>
        /// <param name = "culture">
        ///   The culture.
        /// </param>
        /// <returns>
        ///   The convert.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color))
            {
                return "White";
            }

            var color = (Color)value;
            if (color == Colors.Red)
            {
                return "Red";
            }

            if (color == Colors.Orange)
            {
                return "Orange";
            }

            if (color == Colors.Yellow)
            {
                return "Yellow";
            }

            if (color == Colors.Green)
            {
                return "Green";
            }

            if (color == Colors.Blue)
            {
                return "Blue";
            }

            if (color == Colors.Purple)
            {
                return "Purple";
            }

            return Colors.White;
        }

        /// <summary>
        ///   The convert back.
        /// </summary>
        /// <param name = "value">
        ///   The value.
        /// </param>
        /// <param name = "targetType">
        ///   The target type.
        /// </param>
        /// <param name = "parameter">
        ///   The parameter.
        /// </param>
        /// <param name = "culture">
        ///   The culture.
        /// </param>
        /// <returns>
        ///   The convert back.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorName = value as string;
            if (string.IsNullOrEmpty(colorName))
            {
                return Colors.White;
            }

            switch (colorName)
            {
                case "Red":
                    return Colors.Red;
                case "Orange":
                    return Colors.Orange;
                case "Yellow":
                    return Colors.Yellow;
                case "Green":
                    return Colors.Green;
                case "Blue":
                    return Colors.Blue;
                case "Purple":
                    return Colors.Purple;
                default:
                    return Colors.White;
            }
        }
    }
}