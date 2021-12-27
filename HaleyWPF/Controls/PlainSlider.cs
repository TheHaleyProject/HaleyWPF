using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Haley.WPF.Controls
{
    public class PlainSlider : Slider
    {
        static PlainSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainSlider), new FrameworkPropertyMetadata(typeof(PlainSlider)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public PlainSlider() { }
    }
}
