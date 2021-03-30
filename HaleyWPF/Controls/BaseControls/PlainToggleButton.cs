using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;

namespace Haley.WPF.BaseControls
{
    public class PlainToggleButton : ToggleButtonBase, ICornerRadius
    {
        static PlainToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainToggleButton), new FrameworkPropertyMetadata(typeof(PlainToggleButton)));
        }

        public PlainToggleButton() { }

        public string OnText
        {
            get { return (string)GetValue(OnTextProperty); }
            set { SetValue(OnTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnTextProperty =
            DependencyProperty.Register(nameof(OnText), typeof(string), typeof(PlainToggleButton), new PropertyMetadata("ON"));

        public string OffText
        {
            get { return (string)GetValue(OffTextProperty); }
            set { SetValue(OffTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffTextProperty =
            DependencyProperty.Register(nameof(OffText), typeof(string), typeof(PlainToggleButton), new PropertyMetadata("OFF"));

        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty.Register(nameof(ShowText), typeof(bool), typeof(PlainToggleButton), new PropertyMetadata(true));

        public double SwitchWidth
        {
            get { return (double)GetValue(SwitchWidthProperty); }
            set { SetValue(SwitchWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SwitchWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SwitchWidthProperty =
            DependencyProperty.Register(nameof(SwitchWidth), typeof(double), typeof(PlainToggleButton), new PropertyMetadata(10.0));

        public bool EnlargeSwitchButton
        {
            get { return (bool)GetValue(EnlargeSwitchButtonProperty); }
            set { SetValue(EnlargeSwitchButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnlargeSwitchButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnlargeSwitchButtonProperty =
            DependencyProperty.Register(nameof(EnlargeSwitchButton), typeof(bool), typeof(PlainToggleButton), new PropertyMetadata(false));

    }
}
