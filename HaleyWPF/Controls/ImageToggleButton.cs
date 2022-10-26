using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Haley.Enums;

namespace Haley.WPF.Controls
{
    /// <summary>
    /// Toggle button to change image.
    /// </summary>
    public class ImageToggleButton : ToggleButtonBase
    {
        static ImageToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageToggleButton), new FrameworkPropertyMetadata(typeof(ImageToggleButton)));
        }
        public ImageToggleButton()
        {

        }

        public override void OnApplyTemplate()
        {
            if (Icon == null) Icon = ResourceHelper.getIcon(IconKind.empty_image.ToString());
            if (OffIcon == null) SetCurrentValue(OffIconProperty, Icon);
            base.OnApplyTemplate();
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ImageToggleButton), new PropertyMetadata(null));

        public ImageSource OffIcon
        {
            get { return (ImageSource)GetValue(OffIconProperty); }
            set { SetValue(OffIconProperty, value); }
        }

        public static readonly DependencyProperty OffIconProperty =
            DependencyProperty.Register(nameof(OffIcon), typeof(ImageSource), typeof(ImageToggleButton), new PropertyMetadata(null));

    }
}
