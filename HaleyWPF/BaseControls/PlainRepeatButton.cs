using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Haley.WPF.BaseControls
{
    public class PlainRepeatButton : RepeatButton, IShadow, ICornerRadius, IHoverBase
    {
        static PlainRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainRepeatButton), new FrameworkPropertyMetadata(typeof(PlainRepeatButton)));
        }

        public PlainRepeatButton() { }

        #region Corner Radius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));
        #endregion

        #region Hover
        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public Brush HoverBorderBrush
        {
            get { return (Brush)GetValue(HoverBorderBrushProperty); }
            set { SetValue(HoverBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBorderBrushProperty =
            DependencyProperty.Register(nameof(HoverBorderBrush), typeof(Brush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public Thickness HoverBorderThickness
        {
            get { return (Thickness)GetValue(HoverBorderThicknessProperty); }
            set { SetValue(HoverBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBorderThicknessProperty =
            DependencyProperty.Register(nameof(HoverBorderThickness), typeof(Thickness), typeof(PlainRepeatButton), new PropertyMetadata(ResourceHelper.borderThickness));

        public bool DisableHover
        {
            get { return (bool)GetValue(DisableHoverProperty); }
            set { SetValue(DisableHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisableHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisableHoverProperty =
            DependencyProperty.Register(nameof(DisableHover), typeof(bool), typeof(PlainRepeatButton), new PropertyMetadata(false));
        #endregion

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(PlainRepeatButton), new PropertyMetadata(null));

        #region SHADOW
        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(false));

        public Brush ShadowColor
        {
            get { return (Brush)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Brush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public bool ShadowOnlyOnMouseOver
        {
            get { return (bool)GetValue(ShadowOnlyOnMouseOverProperty); }
            set { SetValue(ShadowOnlyOnMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowOnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowOnlyOnMouseOverProperty =
            DependencyProperty.Register(nameof(ShadowOnlyOnMouseOver), typeof(bool), typeof(PlainRepeatButton), new PropertyMetadata(true));
        #endregion
    }
}
