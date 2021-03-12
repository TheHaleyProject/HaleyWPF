using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
{
    public class ToggleButton : Control, IShadow, ICornerRadius
    {
        #region Click Event
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ToggleButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        #endregion

        static ToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButton), new FrameworkPropertyMetadata(typeof(ToggleButton)));
        }

        public ToggleButton() { }

        public string OnText
        {
            get { return (string)GetValue(OnTextProperty); }
            set { SetValue(OnTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnTextProperty =
            DependencyProperty.Register(nameof(OnText), typeof(string), typeof(ToggleButton), new PropertyMetadata("ON"));

        public string OffText
        {
            get { return (string)GetValue(OffTextProperty); }
            set { SetValue(OffTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffTextProperty =
            DependencyProperty.Register(nameof(OffText), typeof(string), typeof(ToggleButton), new PropertyMetadata("OFF"));

        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty.Register(nameof(ShowText), typeof(bool), typeof(ToggleButton), new PropertyMetadata(true));

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            //And also change the bool Status here.
            Status = !Status;
            RaiseEvent(new UIRoutedEventArgs<bool>(ClickEvent, this) { value = Status });
        }

        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(nameof(Status), typeof(bool), typeof(ToggleButton), new PropertyMetadata(false));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ToggleButton), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public double SwitchWidth
        {
            get { return (double)GetValue(SwitchWidthProperty); }
            set { SetValue(SwitchWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SwitchWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SwitchWidthProperty =
            DependencyProperty.Register(nameof(SwitchWidth), typeof(double), typeof(ToggleButton), new PropertyMetadata(10.0));

        public Brush ColorON
        {
            get { return (Brush)GetValue(ColorONProperty); }
            set { SetValue(ColorONProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorON.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorONProperty =
            DependencyProperty.Register(nameof(ColorON), typeof(Brush), typeof(ToggleButton), new FrameworkPropertyMetadata(null));

        public Brush ColorOFF
        {
            get { return (Brush)GetValue(ColorOFFProperty); }
            set { SetValue(ColorOFFProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorOFF.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorOFFProperty =
            DependencyProperty.Register(nameof(ColorOFF), typeof(Brush), typeof(ToggleButton), new PropertyMetadata(null));

        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(ToggleButton), new FrameworkPropertyMetadata(false));

        public Brush ShadowColor
        {
            get { return (Brush)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Brush), typeof(ToggleButton), new FrameworkPropertyMetadata(null));

        public bool ShadowOnlyOnMouseOver
        {
            get { return (bool)GetValue(ShadowOnlyOnMouseOverProperty); }
            set { SetValue(ShadowOnlyOnMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowOnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowOnlyOnMouseOverProperty =
            DependencyProperty.Register(nameof(ShadowOnlyOnMouseOver), typeof(bool), typeof(ToggleButton), new FrameworkPropertyMetadata(true));



        public bool EnlargeSwitchButton
        {
            get { return (bool)GetValue(EnlargeSwitchButtonProperty); }
            set { SetValue(EnlargeSwitchButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnlargeSwitchButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnlargeSwitchButtonProperty =
            DependencyProperty.Register(nameof(EnlargeSwitchButton), typeof(bool), typeof(ToggleButton), new PropertyMetadata(false));

    }
}
