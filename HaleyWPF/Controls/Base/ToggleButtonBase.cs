using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Haley.Utils;

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

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ToggleButtonBase), new FrameworkPropertyMetadata(default(CornerRadius)));

        public SolidColorBrush ColorON
        {
            get { return (SolidColorBrush)GetValue(ColorONProperty); }
            set { SetValue(ColorONProperty, value); }
        }

        public static readonly DependencyProperty ColorONProperty =
            DependencyProperty.Register(nameof(ColorON), typeof(SolidColorBrush), typeof(ToggleButtonBase), new FrameworkPropertyMetadata(null));

        public SolidColorBrush ColorOFF
        {
            get { return (SolidColorBrush)GetValue(ColorOFFProperty); }
            set { SetValue(ColorOFFProperty, value); }
        }

        public static readonly DependencyProperty ColorOFFProperty =
            DependencyProperty.Register(nameof(ColorOFF), typeof(SolidColorBrush), typeof(ToggleButtonBase), new PropertyMetadata(null));
    }
}
