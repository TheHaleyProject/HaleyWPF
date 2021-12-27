using System;
using System.Linq;
using System.Windows;
using Haley.Models;
using System.Windows.Controls;

namespace Haley.WPF.Controls
{
    public class PlainImage : Control
    {
        #region Constructors
        static PlainImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainImage), new FrameworkPropertyMetadata(typeof(PlainImage)));
        }

        public PlainImage()
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
