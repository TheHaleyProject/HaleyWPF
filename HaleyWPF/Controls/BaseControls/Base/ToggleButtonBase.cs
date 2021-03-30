using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
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

        public Brush ColorON
        {
            get { return (Brush)GetValue(ColorONProperty); }
            set { SetValue(ColorONProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorON.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorONProperty =
            DependencyProperty.Register(nameof(ColorON), typeof(Brush), typeof(ToggleButtonBase), new FrameworkPropertyMetadata(null));

        public Brush ColorOFF
        {
            get { return (Brush)GetValue(ColorOFFProperty); }
            set { SetValue(ColorOFFProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorOFF.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorOFFProperty =
            DependencyProperty.Register(nameof(ColorOFF), typeof(Brush), typeof(ToggleButtonBase), new PropertyMetadata(null));
    }
}
