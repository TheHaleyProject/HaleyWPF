using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Haley.Models;
using System.Collections.Generic;

namespace Haley.WPF
{
    public static class ResourceHelper
    {
        public static CornerRadius cornerRadius = new CornerRadius(0.0);

        private static CommonDictionary colorDictionary;
        private static List<CommonDictionary> iconDictionaries;

        public static SolidColorBrush getBrush(object resourceKey)
        {
            if (colorDictionary == null)
            {
                //in case we decide to implement a Theme in Haley.WPF, we need to figure a way to change this theme at run time. (just like how we change theme using ThemeLoader)
                colorDictionary = new CommonDictionary(); //Even though we use 'new' key word, common dictionary has a static dictionary store where it checks and returns only distinct value.
                colorDictionary.Source = new Uri("pack://application:,,,/Haley.WPF;component/Dictionaries/ThemeColors/ThemeNormal.xaml",
                            UriKind.Absolute);
            }
            if (colorDictionary.Contains(resourceKey))
            {
                return (SolidColorBrush)colorDictionary[resourceKey];
            }
            return null;
        }
        public static ImageSource getIcon(object resourceKey)
        {
            if (iconDictionaries == null)
            {
                iconDictionaries = new List<CommonDictionary>();

                var dic_01 = new CommonDictionary();
                dic_01.Source = new Uri("pack://application:,,,/Haley.WPF;component/Dictionaries/Icons/haleyIcons01.xaml",UriKind.RelativeOrAbsolute);
                var dic_02 = new CommonDictionary();
                dic_02.Source = new Uri("pack://application:,,,/Haley.WPF;component/Dictionaries/Icons/haleyIcons02.xaml", UriKind.RelativeOrAbsolute);

                iconDictionaries.Add(dic_01);
                iconDictionaries.Add(dic_02);
            }
            foreach (var dic in iconDictionaries)
            {
                if (dic.Contains(resourceKey))
                {
                    return (ImageSource)dic[resourceKey];
                }
            }
            
            return null;
        }
    }
}
