using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Haley.Abstractions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Haley.MVVM;
using Haley.Enums;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace Haley.Models
{
    public class BorderClipper : Behavior<Border>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            activateClip();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
        }

        public bool EnableClip
        {
            get { return (bool)GetValue(EnableClipProperty); }
            set { SetValue(EnableClipProperty, value); }
        }

        public static readonly DependencyProperty EnableClipProperty =
            DependencyProperty.Register(nameof(EnableClip), typeof(bool), typeof(BorderClipper), new FrameworkPropertyMetadata(false,propertyChangedCallback:OnEnableClipChanged));

        private static void OnEnableClipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BorderClipper clipper)
            {
                clipper.activateClip();
            }
        }

        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(CornerRadius), typeof(BorderClipper), new FrameworkPropertyMetadata(default(CornerRadius),propertyChangedCallback:OnRadiusChanged));

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BorderClipper clipper)
            {
                clipper.activateClip();
            }
        }

        private void AssociatedObject_Loaded(object sender, EventArgs e)
        {
            
            if (!EnableClip) return;
            activateClip();
        }

        void activateClip()
        {
            try
            {
                Border brdr = AssociatedObject as Border;
                if (brdr == null) return;

                if (!EnableClip)
                {
                    brdr.SetCurrentValue(Border.ClipProperty, null);
                    return;
                }

                var _xradius = brdr.CornerRadius.TopLeft + (brdr.BorderThickness.Left);
                var _yradius = brdr.CornerRadius.BottomRight + (brdr.BorderThickness.Bottom);
                if (_xradius < double.Epsilon) _xradius = 0.0;
                if (_yradius < double.Epsilon) _yradius = 0.0;
                var clip = new RectangleGeometry(new Rect(0, 0, brdr.ActualWidth, brdr.ActualHeight), _xradius, _yradius);
                
                clip.Freeze();
                brdr.SetCurrentValue(Border.ClipProperty, clip);
            }
            catch (Exception)
            {
            }
        }
    }
}
