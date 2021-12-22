using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Haley.Enums;

namespace Haley.WPF.Controls
{
    [TemplatePart(Name = UIEHeaderControl, Type = typeof(Border))]
    [TemplatePart(Name = UIEFlyerControl, Type = typeof(Border))]
    public class PlainCard : ContentControl
    {
        public const string UIEHeaderControl = "PART_HeaderHolder";
        public const string UIEFlyerControl = "PART_FlyerHolder";

        private Border _headerborder;
        private Border _flyerBorder;
        static PlainCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainCard), new FrameworkPropertyMetadata(typeof(PlainCard)));
            
        }


        public PlainCard() { }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerborder = GetTemplateChild(UIEHeaderControl) as Border;
            _flyerBorder = GetTemplateChild(UIEFlyerControl) as Border;
            _setHeaderCornerRadius();
        }

        private void _setHeaderCornerRadius()
        {
            try
            {
                if (CornerRadius != null)
                {
                    switch (Mode)
                    {
                        case CardMode.Simple:
                            //Change the header border
                            if (_headerborder == null) return;
                            _headerborder.CornerRadius = new CornerRadius(CornerRadius.TopLeft, CornerRadius.TopRight, 0.0, 0.0);
                            break;
                        case CardMode.Flyer:
                            if (_flyerBorder == null) return;
                            //Change the flyer border.
                            _flyerBorder.CornerRadius = new CornerRadius(0.0, 0.0, CornerRadius.BottomRight, CornerRadius.BottomLeft);
                            break;
                        case CardMode.Professional:
                            if (_headerborder == null) return;
                            _headerborder.CornerRadius = new CornerRadius(0.0, 0.0, CornerRadius.BottomRight, CornerRadius.BottomLeft);
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(PlainCard), new FrameworkPropertyMetadata("Header"));

        public string SubHeader
        {
            get { return (string)GetValue(SubHeaderProperty); }
            set { SetValue(SubHeaderProperty, value); }
        }

        public static readonly DependencyProperty SubHeaderProperty =
            DependencyProperty.Register(nameof(SubHeader), typeof(string), typeof(PlainCard), new PropertyMetadata("Sub Header"));

        public HorizontalAlignment HeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HeaderAlignmentProperty); }
            set { SetValue(HeaderAlignmentProperty, value); }
        }

        public static readonly DependencyProperty HeaderAlignmentProperty =
            DependencyProperty.Register(nameof(HeaderAlignment), typeof(HorizontalAlignment), typeof(PlainCard), new PropertyMetadata(HorizontalAlignment.Center));

        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register(nameof(ShowIcon), typeof(bool), typeof(PlainCard), new PropertyMetadata(true));



        public double FlyerHeight
        {
            get { return (double)GetValue(FlyerHeightProperty); }
            set { SetValue(FlyerHeightProperty, value); }
        }

        public static readonly DependencyProperty FlyerHeightProperty =
            DependencyProperty.Register(nameof(FlyerHeight), typeof(double), typeof(PlainCard), new PropertyMetadata(60.0));

        public double FlyerWidth
        {
            get { return (double)GetValue(FlyerWidthProperty); }
            set { SetValue(FlyerWidthProperty, value); }
        }

        public static readonly DependencyProperty FlyerWidthProperty =
            DependencyProperty.Register(nameof(FlyerWidth), typeof(double), typeof(PlainCard), new PropertyMetadata(60.0));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

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

        public static readonly DependencyProperty ShowHeaderProperty =
            DependencyProperty.Register(nameof(ShowHeader), typeof(bool), typeof(PlainCard), new PropertyMetadata(true));

        public bool ShowSubHeader
        {
            get { return (bool)GetValue(ShowSubHeaderProperty); }
            set { SetValue(ShowSubHeaderProperty, value); }
        }

        public static readonly DependencyProperty ShowSubHeaderProperty =
            DependencyProperty.Register(nameof(ShowSubHeader), typeof(bool), typeof(PlainCard), new PropertyMetadata(true));



        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(PlainCard), new PropertyMetadata(20.0));

        public double SubHeaderFontSize
        {
            get { return (double)GetValue(SubHeaderFontSizeProperty); }
            set { SetValue(SubHeaderFontSizeProperty, value); }
        }

        public static readonly DependencyProperty SubHeaderFontSizeProperty =
            DependencyProperty.Register(nameof(SubHeaderFontSize), typeof(double), typeof(PlainCard), new PropertyMetadata(14.0));



        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(double), typeof(PlainCard), new FrameworkPropertyMetadata(30.0,null,coerceValueCallback:coerceHeight));



        static object coerceHeight(DependencyObject d, object baseValue)
        {
            double _actual = (double)baseValue;
            var _pcard = d as PlainCard;
            if (_actual < 30.0) return 30.0; 
            if ((2 * _actual) > _pcard.Height) return (_pcard.Height/2);
            return baseValue;
        }
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush), typeof(PlainCard), new PropertyMetadata(null));

        public SolidColorBrush HeaderForeground
        {
            get { return (SolidColorBrush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(nameof(HeaderForeground), typeof(SolidColorBrush), typeof(PlainCard), new PropertyMetadata(null));

        public CardMode Mode
        {
            get { return (CardMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(CardMode), typeof(PlainCard), new PropertyMetadata(CardMode.Simple));
    }
}
