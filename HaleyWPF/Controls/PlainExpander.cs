using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(PlainExpander), new FrameworkPropertyMetadata(null));

        public SolidColorBrush IconDefColor
        {
            get { return (SolidColorBrush)GetValue(IconDefColorProperty); }
            set { SetValue(IconDefColorProperty, value); }
        }

        public static readonly DependencyProperty IconDefColorProperty =
            DependencyProperty.Register(nameof(IconDefColor), typeof(SolidColorBrush), typeof(PlainExpander), new FrameworkPropertyMetadata(null));

        public ImageSource Arrow
        {
            get { return (ImageSource)GetValue(ArrowProperty); }
            set { SetValue(ArrowProperty, value); }
        }

        public static readonly DependencyProperty ArrowProperty =
            DependencyProperty.Register(nameof(Arrow), typeof(ImageSource), typeof(PlainExpander), new FrameworkPropertyMetadata(ResourceHelper.getIcon(IconKind.arrow_down_rounded.ToString())));

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

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainExpander), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));
    }
}
