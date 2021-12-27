using System;
using System.Linq;
using System.Windows;
using Haley.Models;

namespace Haley.WPF.Controls
{
    public class ImageRepeatButton : PlainRepeatButton
    {
        #region Constructors
        static ImageRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageRepeatButton), new FrameworkPropertyMetadata(typeof(ImageRepeatButton)));
        }

        public ImageRepeatButton()
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
