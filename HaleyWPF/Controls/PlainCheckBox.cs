using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    public class PlainCheckBox : CheckBox
    {
        private const double min_TickBoxSize = 15.0;
        private const double max_TickBoxSize = 25.0;
        static PlainCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainCheckBox), new FrameworkPropertyMetadata(typeof(PlainCheckBox)));
        }

        public PlainCheckBox() { }

        public SolidColorBrush TickColor
        {
            get { return (SolidColorBrush)GetValue(TickColorProperty); }
            set { SetValue(TickColorProperty, value); }
        }

        public static readonly DependencyProperty TickColorProperty =
            DependencyProperty.Register(nameof(TickColor), typeof(SolidColorBrush), typeof(PlainCheckBox), new PropertyMetadata(null));

        public double TickBoxSize
        {
            get { return (double)GetValue(TickBoxSizeProperty); }
            set { SetValue(TickBoxSizeProperty, value); }
        }

        public static readonly DependencyProperty TickBoxSizeProperty =
            DependencyProperty.Register(nameof(TickBoxSize), typeof(double), typeof(PlainCheckBox), new FrameworkPropertyMetadata(16.0, new PropertyChangedCallback(TickBoxSizePropertyChanged), ValidatTickBoxSize));
        static void TickBoxSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //
        }
        static object ValidatTickBoxSize(DependencyObject d, object baseValue)
        {
            var current_value = (double)baseValue;
            if (current_value < min_TickBoxSize) return min_TickBoxSize;
            if (current_value > max_TickBoxSize) return max_TickBoxSize;
            return baseValue;
        }
    }
}
