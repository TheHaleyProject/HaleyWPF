using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.Models
{
    public class Shadow : DependencyObject
    {
        public Shadow() { }

        #region SHADOW

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(Shadow), new PropertyMetadata(false));


        public static Brush GetColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ColorProperty);
        }

        public static void SetColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(ColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.RegisterAttached("Color", typeof(Brush), typeof(Shadow), new PropertyMetadata(null));

        public static bool GetOnlyOnMouseOver(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnlyOnMouseOverProperty);
        }

        public static void SetOnlyOnMouseOver(DependencyObject obj, bool value)
        {
            obj.SetValue(OnlyOnMouseOverProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnlyOnMouseOverProperty =
            DependencyProperty.RegisterAttached("OnlyOnMouseOver", typeof(bool), typeof(Shadow), new PropertyMetadata(true));

        public static double GetBlurRadius(DependencyObject obj)
        {
            return (double)obj.GetValue(BlurRadiusProperty);
        }

        public static void SetBlurRadius(DependencyObject obj, double value)
        {
            obj.SetValue(BlurRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for BlurRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlurRadiusProperty =
            DependencyProperty.RegisterAttached("BlurRadius", typeof(double), typeof(Shadow), new PropertyMetadata(4.0));

        #endregion
    }
}
