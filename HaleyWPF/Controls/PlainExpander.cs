using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Haley.Utils;


namespace Haley.WPF.Controls
{
    public class PlainExpander : Expander, ICornerRadius
    {
        static PlainExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainExpander), new FrameworkPropertyMetadata(typeof(PlainExpander)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register(nameof(ShowIcon), typeof(bool), typeof(PlainExpander), new PropertyMetadata(true));

        public ImageSource Arrow
        {
            get { return (ImageSource)GetValue(ArrowProperty); }
            set { SetValue(ArrowProperty, value); }
        }

        public static readonly DependencyProperty ArrowProperty =
            DependencyProperty.Register(nameof(Arrow), typeof(ImageSource), typeof(PlainExpander), new FrameworkPropertyMetadata(IconFinder.GetIcon(IconKind.arrow_down_rounded.ToString(),IconSourceKey.Default)));

        public SolidColorBrush ArrowDefColor
        {
            get { return (SolidColorBrush)GetValue(ArrowDefColorProperty); }
            set { SetValue(ArrowDefColorProperty, value); }
        }

        public static readonly DependencyProperty ArrowDefColorProperty =
            DependencyProperty.Register(nameof(ArrowDefColor), typeof(SolidColorBrush), typeof(PlainExpander), new PropertyMetadata(null));

        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(PlainExpander), new FrameworkPropertyMetadata(null));

        public Thickness HeaderBorderThickness {
            get { return (Thickness)GetValue(HeaderBorderThicknessProperty); }
            set { SetValue(HeaderBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty HeaderBorderThicknessProperty =
            DependencyProperty.Register(nameof(HeaderBorderThickness), typeof(Thickness), typeof(PlainExpander), new FrameworkPropertyMetadata());

        public double HeaderSize {
            get { return (double)GetValue(HeaderSizeProperty); }
            set { SetValue(HeaderSizeProperty, value); }
        }

        public static readonly DependencyProperty HeaderSizeProperty =
            DependencyProperty.Register(nameof(HeaderSize), typeof(double), typeof(PlainExpander), new FrameworkPropertyMetadata(25.0));

        public Thickness HeaderPadding {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }

        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.Register(nameof(HeaderPadding), typeof(Thickness), typeof(PlainExpander), new FrameworkPropertyMetadata());

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainExpander), new FrameworkPropertyMetadata(default(CornerRadius)));
    }
}
