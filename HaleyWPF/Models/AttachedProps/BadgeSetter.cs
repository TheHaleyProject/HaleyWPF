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
                //Get the badge.
                var _badge = element.GetValue(BadgeProperty) as Badge;

                //Get adorner layer
                var _layer = AdornerLayer.GetAdornerLayer(element);

                //Create new adorner and add to the layer.
                var _badgeAdorner = new BadgeAdorner(element, _badge);
                _layer.Add(_badgeAdorner);

                //Store all objects as reference in the Badge.
                _badge.Adorner = _badgeAdorner;
                _badge.AdornerLayer = _layer;
                _badge.Parent = element;

                //Subscribe to changes in badge.
                _badge.ValueChanged += _badge_ValueChanged;

                //Unsubscribe to the element load
                element.Loaded -= Element_Loaded;
            }
        }

        private static void _badge_ValueChanged(object sender, EventArgs e)
        {
            //Each time value changes, we remove the old adorner and add new one.
          if (sender is Badge _badge)
            {
                _badge.AdornerLayer.Remove(_badge.Adorner);
                //Create a new adorner.
                var _badgeAdorner = new BadgeAdorner(_badge.Parent, _badge);
                _badge.AdornerLayer.Add(_badgeAdorner);

                //Store the new adorner back to the badge.
                _badge.Adorner = _badgeAdorner;
            }
        }

        static void BadgePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                //Adorner layer is created only after the element is loaded. So we wait until it is loaded.
                element.Loaded += Element_Loaded;
                element.Unloaded += Element_Unloaded;
            }
        }

        private static void Element_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.Loaded -= Element_Loaded;
                element.Unloaded -= Element_Unloaded;
                var _badge = element.GetValue(BadgeProperty) as Badge;
                _badge.ValueChanged -= _badge_ValueChanged;
            }
        }
    }
}
