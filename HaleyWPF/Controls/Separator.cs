using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Haley.WPF.Controls
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

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(double), typeof(Separator), new PropertyMetadata(1.0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(Separator), new PropertyMetadata(Orientation.Horizontal));
    }
}
