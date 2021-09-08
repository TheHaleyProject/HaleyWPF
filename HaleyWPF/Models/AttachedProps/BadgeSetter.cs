using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using Haley.WPF.Controls;

namespace Haley.Models
{
    public static class BadgeSetter 
    {
        public static Badge GetBadge(DependencyObject obj)
        {
            return (Badge)obj.GetValue(BadgeProperty);
        }

        public static void SetBadge(DependencyObject obj, Badge value)
        {
            obj.SetValue(BadgeProperty, value);
        }

        // Using a DependencyProperty as the backing store for Badge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.RegisterAttached("Badge", typeof(Badge), typeof(BadgeSetter), new PropertyMetadata(null,BadgePropertyChanged));

        private static void Element_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                var _badge = element.GetValue(BadgeProperty) as Badge;
                var _layer = AdornerLayer.GetAdornerLayer(element);

                _layer.Add(new BadgeAdorner(element,_badge));
            }
        }
        static void BadgePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                //Adorner layer is created only after the element is loaded. So we wait until it is loaded.
                element.Loaded += Element_Loaded;
            }
        }
    }
}
