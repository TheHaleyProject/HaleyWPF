using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Haley.Models;
using System.Windows.Controls.Primitives;

namespace Haley.WPF.BaseControls
{
    public class ImageButton : ButtonBase
    {
        #region Constructors
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Icon.InitiateImages(this);
        }
    }
}
