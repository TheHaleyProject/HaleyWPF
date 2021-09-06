using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Haley.WPF.Controls
{
    public class PlainTextBox : TextBox, ICornerRadius
    {
        static PlainTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainTextBox), new FrameworkPropertyMetadata(typeof(PlainTextBox)));
        }

        public PlainTextBox() { }

        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register(nameof(WaterMark), typeof(string), typeof(PlainTextBox), new PropertyMetadata("Enter Value"));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainTextBox), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));
    }
}
