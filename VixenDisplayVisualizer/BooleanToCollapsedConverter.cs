// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    ///   The boolean to collapsed converter.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToCollapsedConverter : IValueConverter
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
            if (value != null && value is bool
                && (targetType == typeof(Visibility) || typeof(Visibility).IsSubclassOf(targetType)))
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }

            return DependencyProperty.UnsetValue;
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
            if (value != null && value is Visibility
                && ((Visibility)value == Visibility.Collapsed || (Visibility)value == Visibility.Visible)
                && targetType == typeof(bool))
            {
                return (Visibility)value == Visibility.Collapsed;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}