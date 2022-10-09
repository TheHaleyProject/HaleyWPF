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
            Icon.SetIsHandler(this,true); //This is an handler for the icon
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Icon.InitiateImages(this);
        }

        public double RotateAngle {
            get { return (double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }

        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register(nameof(RotateAngle), typeof(double), typeof(PlainImage), new PropertyMetadata(0.0));
    }
}
