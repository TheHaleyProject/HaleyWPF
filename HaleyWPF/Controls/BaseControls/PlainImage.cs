using System;
using System.Linq;
using System.Windows;
using Haley.Models;

namespace Haley.WPF.BaseControls
{
    public class PlainImage : Icon
    {
        #region Constructors
        static PlainImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainImage), new FrameworkPropertyMetadata(typeof(PlainImage)));
        }

        public PlainImage()
        {
            base.IsPressEnabled = false;
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            base.IsPressEnabled = false;
        }
    }
}
