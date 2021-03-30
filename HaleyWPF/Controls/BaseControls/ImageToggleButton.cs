using System;
using System.Linq;
using System.Windows;

namespace Haley.WPF.BaseControls
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
            base.OnApplyTemplate();
        }
    }
}
