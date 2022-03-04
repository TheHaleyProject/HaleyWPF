using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Haley.Abstractions;
using Haley.Enums;
using System.Windows;
using System.Windows.Controls;
using Haley.WPF.Internal;
using Haley.Models;
using Haley.Services;

namespace Haley.Utils
{
    public static class HaleyThemeProvider 
    {
        private static Dictionary<InternalThemeMode, Uri> GenerateInternalData()
        {
            //pack://application:,,,/Haley.WPF;component/Dictionaries/ThemeColors/ThemeNormal.xaml
            //This should get all the available internal themes.
            Dictionary<InternalThemeMode, Uri> result = new Dictionary<InternalThemeMode, Uri>();

            string themeURIPathBase = $@"pack://application:,,,/Haley.WPF;component/Dictionaries/ThemeColors/Theme";
            result.Add(InternalThemeMode.Normal, new Uri(themeURIPathBase + "Normal.xaml", UriKind.RelativeOrAbsolute));
            result.Add(InternalThemeMode.Mild, new Uri(themeURIPathBase + "Mild.xaml", UriKind.RelativeOrAbsolute));
            result.Add(InternalThemeMode.Dark, new Uri(themeURIPathBase + "Dark.xaml", UriKind.RelativeOrAbsolute));
            return result;
        }

        public static InternalThemeProvider Prepare()
        {
            InternalThemeProvider _newData = new InternalThemeProvider();
            _newData.SetInfoDic(GenerateInternalData());
            return _newData;
        }
    }
}
