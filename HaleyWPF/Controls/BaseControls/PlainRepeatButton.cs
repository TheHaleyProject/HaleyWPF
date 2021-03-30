using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
{
    public class PlainRepeatButton : RepeatButton, ICornerRadius
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

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(PlainRepeatButton), new PropertyMetadata(null));
    }
}
