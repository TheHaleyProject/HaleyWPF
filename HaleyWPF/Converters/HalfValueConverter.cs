using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Reflection;
using System.ComponentModel;

namespace Haley.Converters
{
    /// <summary>
    /// This converter should divide whatever value that you provide by 2 and return the result
    /// </summary>
    public class HalfValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //What if input is integer???
                double _actual_length = (double)value;
                return (_actual_length /2);
            }
            catch (Exception) //In case of any exception return the actual input value
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double _converted_length = (double)value;
                return (_converted_length * 2);
            }
            catch (Exception) //In case of any exception return the actual input value
            {
                return value;
            }
        }
    }
}
