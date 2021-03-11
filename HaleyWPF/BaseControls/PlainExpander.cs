using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
{
    public class PlainExpander : Expander, IShadow, ICornerRadius
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

        // Using a DependencyProperty as the backing store for ShowIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register(nameof(ShowIcon), typeof(bool), typeof(PlainExpander), new PropertyMetadata(true));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(PlainExpander), new FrameworkPropertyMetadata(null));

        public SolidColorBrush IconDefColor
        {
            get { return (SolidColorBrush)GetValue(IconDefColorProperty); }
            set { SetValue(IconDefColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconDefColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconDefColorProperty =
            DependencyProperty.Register(nameof(IconDefColor), typeof(SolidColorBrush), typeof(PlainExpander), new FrameworkPropertyMetadata(null));

        public ImageSource Arrow
        {
            get { return (ImageSource)GetValue(ArrowProperty); }
            set { SetValue(ArrowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Arrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowProperty =
            DependencyProperty.Register(nameof(Arrow), typeof(ImageSource), typeof(PlainExpander), new FrameworkPropertyMetadata(ResourceHelper.getIcon(IconEnums.arrow_down_rounded.ToString())));

        public SolidColorBrush ArrowDefColor
        {
            get { return (SolidColorBrush)GetValue(ArrowDefColorProperty); }
            set { SetValue(ArrowDefColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowDefColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowDefColorProperty =
            DependencyProperty.Register(nameof(ArrowDefColor), typeof(SolidColorBrush), typeof(PlainExpander), new PropertyMetadata(null));

        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(PlainExpander), new FrameworkPropertyMetadata(null));

        #region Shadow
        public bool ShadowOnlyOnMouseOver
        {
            get { return (bool)GetValue(ShadowOnlyOnMouseOverProperty); }
            set { SetValue(ShadowOnlyOnMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowOnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowOnlyOnMouseOverProperty =
            DependencyProperty.Register(nameof(ShadowOnlyOnMouseOver), typeof(bool), typeof(PlainExpander), new PropertyMetadata(true));

        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(PlainExpander), new FrameworkPropertyMetadata(false));

        public Brush ShadowColor
        {
            get { return (Brush)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Brush), typeof(PlainExpander), new FrameworkPropertyMetadata(ResourceHelper.getBrush(ColorEnums.defShadow.ToString())));
        #endregion

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainExpander), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(PlainExpander), new FrameworkPropertyMetadata(ResourceHelper.getBrush(ColorEnums.defHoverBackground.ToString())));
    }
}
