using Haley.Abstractions;
using System;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.Models
{
    public class FlexiMenuBar : DependencyObject
    {
        public static FontFamily GetFontFamily(DependencyObject obj) {
            return (FontFamily)obj.GetValue(FontFamilyProperty);
        }

        public static void SetFontFamily(DependencyObject obj, FontFamily value) {
            obj.SetValue(FontFamilyProperty, value);
        }

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.RegisterAttached("FontFamily", typeof(FontFamily), typeof(FlexiMenuBar), new PropertyMetadata(null));

        public static Brush GetForeground(DependencyObject obj) {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, Brush value) {
            obj.SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(FlexiMenuBar), new PropertyMetadata(Brushes.Black));

        public static FontStyle GetFontStyle(DependencyObject obj) {
            return (FontStyle)obj.GetValue(FontStyleProperty);
        }

        public static void SetFontStyle(DependencyObject obj, FontStyle value) {
            obj.SetValue(FontStyleProperty, value);
        }

        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.RegisterAttached("FontStyle", typeof(FontStyle), typeof(FlexiMenuBar), new PropertyMetadata(default(FontStyle)));

        public static FontStretch GetFontStretch(DependencyObject obj) {
            return (FontStretch)obj.GetValue(FontStretchProperty);
        }

        public static void SetFontStretch(DependencyObject obj, FontStretch value) {
            obj.SetValue(FontStretchProperty, value);
        }

        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.RegisterAttached("FontStretch", typeof(FontStretch), typeof(FlexiMenuBar), new PropertyMetadata(default(FontStretch)));



        public static FontWeight GetFontWeight(DependencyObject obj) {
            return (FontWeight)obj.GetValue(FontWeightProperty);
        }

        public static void SetFontWeight(DependencyObject obj, FontWeight value) {
            obj.SetValue(FontWeightProperty, value);
        }

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.RegisterAttached("FontWeight", typeof(FontWeight), typeof(FlexiMenuBar), new PropertyMetadata(default(FontWeight)));

        public static double GetFontSize(DependencyObject obj) {
            return (double)obj.GetValue(FontSizeProperty);
        }

        public static void SetFontSize(DependencyObject obj, double value) {
            obj.SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(FlexiMenuBar), new PropertyMetadata(13.0));

        public FlexiMenuBar() { }

    }
}
