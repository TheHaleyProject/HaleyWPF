using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
{
    [TemplatePart(Name = UIEHeaderControl, Type = typeof(Border))]
    public class PlainCard : ContentControl
    {
        public const string UIEHeaderControl = "PART_HeaderHolder";

        private Border _headerborder;
        static PlainCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainCard), new FrameworkPropertyMetadata(typeof(PlainCard)));
        }


        public PlainCard() { }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerborder = GetTemplateChild(UIEHeaderControl) as Border;
            _setHeaderCornerRadius();
        }

        private void _setHeaderCornerRadius()
        {
            if (CornerRadius != null && _headerborder != null)
            {
                _headerborder.CornerRadius = new CornerRadius(CornerRadius.TopLeft, CornerRadius.TopRight, 0.0, 0.0);
            }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(PlainCard), new FrameworkPropertyMetadata("Plain Card"));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainCard), new FrameworkPropertyMetadata(defaultValue: ResourceHelper.cornerRadius,propertyChangedCallback:cornerRadiusPropertyChanged));

        static void cornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainCard pcard)
            {
                pcard._setHeaderCornerRadius();
            }
        }

        public bool ShowHeader
        {
            get { return (bool)GetValue(ShowHeaderProperty); }
            set { SetValue(ShowHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowHeaderProperty =
            DependencyProperty.Register("ShowHeader", typeof(bool), typeof(PlainCard), new PropertyMetadata(true));

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(double), typeof(PlainCard), new PropertyMetadata(25.0));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(PlainCard), new PropertyMetadata(null));

        public SolidColorBrush HeaderForeground
        {
            get { return (SolidColorBrush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(nameof(HeaderForeground), typeof(SolidColorBrush), typeof(PlainCard), new PropertyMetadata(null));
    }
}
