using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Haley.WPF.BaseControls
{
    public class PlainToggleButton : ToggleButton, IShadow, ICornerRadius
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

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainToggleButton), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public double SwitchWidth
        {
            get { return (double)GetValue(SwitchWidthProperty); }
            set { SetValue(SwitchWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SwitchWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SwitchWidthProperty =
            DependencyProperty.Register(nameof(SwitchWidth), typeof(double), typeof(PlainToggleButton), new PropertyMetadata(10.0));

        public Brush ColorON
        {
            get { return (Brush)GetValue(ColorONProperty); }
            set { SetValue(ColorONProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorON.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorONProperty =
            DependencyProperty.Register(nameof(ColorON), typeof(Brush), typeof(PlainToggleButton), new FrameworkPropertyMetadata(null));

        public Brush ColorOFF
        {
            get { return (Brush)GetValue(ColorOFFProperty); }
            set { SetValue(ColorOFFProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorOFF.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorOFFProperty =
            DependencyProperty.Register(nameof(ColorOFF), typeof(Brush), typeof(PlainToggleButton), new PropertyMetadata(null));

        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(PlainToggleButton), new FrameworkPropertyMetadata(false));

        public Brush ShadowColor
        {
            get { return (Brush)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Brush), typeof(PlainToggleButton), new FrameworkPropertyMetadata(null));

        public bool ShadowOnlyOnMouseOver
        {
            get { return (bool)GetValue(ShadowOnlyOnMouseOverProperty); }
            set { SetValue(ShadowOnlyOnMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowOnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowOnlyOnMouseOverProperty =
            DependencyProperty.Register(nameof(ShadowOnlyOnMouseOver), typeof(bool), typeof(PlainToggleButton), new FrameworkPropertyMetadata(true));

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
