using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Reflection;
using System.ComponentModel;

//namespace Haley.WPF.AppResources.Converters
//{
//    #region Methods
//    internal static class ConverterFunctions
//    {
//        public static string getEnumDesctiption__(Enum input)
//        {
//            FieldInfo fi = input.GetType().GetField(input.ToString());
//            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

//            return attributes.Length == 0 ? input.ToString() : ((DescriptionAttribute)attributes[0]).Description;
//        }
//        public static string getString(object value)
//        {
//            Type InputType = value.GetType();
//            //Convert Input to String.
//            if (InputType == typeof(string)) return (string)value;
//            if (InputType == typeof(int)) return ((int)value).ToString();
//            if (InputType == typeof(double)) return ((double)value).ToString();
//            if (InputType == typeof(bool)) return ((bool)value).ToString();
//            if (InputType.BaseType == typeof(Enum)) return ((Enum)value).ToString();
//            return null;
//        }

//        public static object getObject(object value, Type targetType)
//        {
//            if (targetType == typeof(string)) return (string)value;
//            if (targetType == typeof(int)) return int.Parse((string)value);
//            if (targetType == typeof(double)) return double.Parse((string)value);
//            if (targetType == typeof(bool)) return bool.Parse((string)value);
//            if (targetType.BaseType == typeof(Enum)) return value;
//            return null;
//        }
//    }

//    #endregion



//    /// <summary>
//    /// Dictionary should be of format Dictionary<object bool> 
//    /// /// </summary>
//    public class DictionaryFilter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            Dictionary<object, bool> input = (Dictionary<object, bool>)value;
//            int param = 1;
//            if (parameter != null) int.Parse((string)parameter);
//            if (input != null || input.Count != 0)
//            {
//                Dictionary<object, bool> newSource = new Dictionary<object, bool>();
//                if (param == 1) newSource =  input.ToList().Where(p => p.Value == true).ToDictionary(a => a.Key, b => b.Value);
//                if (param == 2) newSource = input.ToList().Where(p => p.Value == false).ToDictionary(a => a.Key, b => b.Value);
//                return newSource; //In case this fails, return the initial list after the conditional block
//            }
//            return input;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class EquateInputAndParameter : IValueConverter //Can be used to check if the input parameter and the binded value are similar. Both should match integer.
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (parameter == null || value == null) return false;
//            string inputparameter = ConverterFunctions.getString(parameter); // Parameter is always going to be string.
//            return ConverterFunctions.getString(value) == inputparameter;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            bool ActualResult = (bool)value; //If actual result is true, then return the parameter, else, return null.

//            if (ActualResult == true) return ConverterFunctions.getObject(parameter, targetType); //Idea that the bounded source value is checked against the parameter. We try to send the parameter back as integer.
//            if (targetType.IsValueType) return Activator.CreateInstance(targetType); //Which means that the target type is a nonnullable one (it has a default value)
//            if (targetType == typeof(string)) return "Error";
//            return null;
//        }
//    }

//    public class EquateInputAndParameterToVisbility : IValueConverter //Can be used to check if the input parameter and the binded value are similar. Both should match integer.
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (parameter == null || value == null) return false;
//            string inputparameter = ConverterFunctions.getString(parameter); // Parameter is always going to be string.
//            bool result = ConverterFunctions.getString(value) == inputparameter;
//            if (result == true) return Visibility.Visible;
//            return Visibility.Collapsed;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            Visibility visiblestatus = (Visibility)value;
//            if (visiblestatus == Visibility.Visible) return ConverterFunctions.getObject(parameter, targetType); //Idea that the bounded source value is checked against the parameter. We try to send the parameter back as integer.
//            return null;
//        }
//    }

//    public class GetEnumDescription : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return ConverterFunctions.getEnumDesctiption__((Enum)value);
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class ConvertEnumListToStringList : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            List<Enum> input = (List<Enum>)value;
//            List<string> result = new List<string>();
//            foreach (Enum e in input)
//            {
//                result.Add(ConverterFunctions.getEnumDesctiption__(e));
//            }
//            return result;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//    #endregion
//}
