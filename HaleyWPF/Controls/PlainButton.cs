using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Haley.Utils;

namespace Haley.WPF.Controls
{
    public class PlainButton : Button, ICornerRadius
    {
        static PlainButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainButton), new FrameworkPropertyMetadata(typeof(PlainButton)));
        }

        public PlainButton() { }

        #region Corner Radius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainButton), new FrameworkPropertyMetadata(default(CornerRadius)));
        #endregion

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(PlainButton), new PropertyMetadata(null));
    }
}
