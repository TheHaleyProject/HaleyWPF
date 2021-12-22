using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Haley.Enums;

namespace Haley.WPF.Controls
{
    //Apart from whatever is in the ToggleButtonBase, we also add an icon
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
            if (Icon == null) Icon = ResourceHelper.getIcon(IconEnums.empty_image.ToString());
            base.OnApplyTemplate();
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ImageToggleButton), new PropertyMetadata(null));
    }
}
