using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.Utils;
using Haley.Enums;

namespace Haley.WPF
{
    public static class ResourceStore
    {
        public static CornerRadius cornerRadius = new CornerRadius(0.0);

        private static ResourceDictionary colorDictionary;
        private static ResourceDictionary iconDictionary;

        public static SolidColorBrush getBrush(object resourceKey)
        {
            if (colorDictionary == null)
            {
                //in case we decide to implement a Theme in Haley.WPF, we need to figure a way to change this theme at run time. (just like how we change theme using ThemeLoader)
                colorDictionary = new ResourceDictionary();
                colorDictionary.Source = new Uri("pack://application:,,,/Haley.WPF;component/Dictionaries/ThemeNormal.xaml",
                            UriKind.RelativeOrAbsolute);
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
                iconDictionary = new ResourceDictionary();
                iconDictionary.Source = new Uri("pack://application:,,,/Haley.WPF;component/Dictionaries/haleyIcons.xaml",
                            UriKind.RelativeOrAbsolute);
            }
            if (iconDictionary.Contains(resourceKey))
            {
                return (ImageSource)iconDictionary[resourceKey];
            }
            return null;
        }
    }
}
