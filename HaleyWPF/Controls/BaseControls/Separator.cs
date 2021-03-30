using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Haley.WPF.BaseControls
{
    public class Separator : Control
    {
        static Separator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(typeof(Separator)));
        }

        public Separator() { }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(double), typeof(Separator), new PropertyMetadata(1.0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(Separator), new PropertyMetadata(Orientation.Horizontal));
    }
}
