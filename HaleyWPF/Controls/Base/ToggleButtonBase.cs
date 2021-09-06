using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    public class ToggleButtonBase : ToggleButton, ICornerRadius
    {
        public ToggleButtonBase() { }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ToggleButtonBase), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public SolidColorBrush ColorON
        {
            get { return (SolidColorBrush)GetValue(ColorONProperty); }
            set { SetValue(ColorONProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorON.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorONProperty =
            DependencyProperty.Register(nameof(ColorON), typeof(SolidColorBrush), typeof(ToggleButtonBase), new FrameworkPropertyMetadata(null));

        public SolidColorBrush ColorOFF
        {
            get { return (SolidColorBrush)GetValue(ColorOFFProperty); }
            set { SetValue(ColorOFFProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorOFF.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorOFFProperty =
            DependencyProperty.Register(nameof(ColorOFF), typeof(SolidColorBrush), typeof(ToggleButtonBase), new PropertyMetadata(null));
    }
}
