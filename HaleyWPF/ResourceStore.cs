using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Haley.Models;

namespace Haley.WPF
{
    public static class ResourceHelper
    {
        public static CornerRadius cornerRadius = new CornerRadius(0.0);

        private static CommonDictionary colorDictionary;
        private static CommonDictionary iconDictionary;

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
            if (iconDictionary == null)
            {
                iconDictionary = new CommonDictionary();
                iconDictionary.Source = new Uri("pack://application:,,,/Haley.WPF;component/Dictionaries/haleyIcons.xaml",UriKind.RelativeOrAbsolute);
            }
            if (iconDictionary.Contains(resourceKey))
            {
                return (ImageSource)iconDictionary[resourceKey];
            }
            return null;
        }
    }
}
