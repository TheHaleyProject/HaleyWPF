using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using Haley.WPF.Controls;
using System.Globalization;
using System.Windows.Input;

namespace Haley.Models
{
    /// <summary>
    ///  This adorner needs a canvas or a rectanlge and it will move across the X or Y direction. Base upon the mouse position, the slider can be placed on vertical or horizontal across the whole canvas. (Shape can be customized).
    /// </summary>
    public class UniAxisPickerAdorner : PickerAdornerBase
    {
        private static readonly Pen _pen = new Pen(Brushes.Black, 1);
        private Brush _brush = Brushes.Transparent;
        public UniAxisPickerAdorner(UIElement adornedElement,Orientation orientation) : base(adornedElement,false)
        {
            Orientation = orientation;
        }

        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FillColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register(nameof(FillColor), typeof(Color), typeof(UniAxisPickerAdorner), new FrameworkPropertyMetadata(Colors.Transparent,FrameworkPropertyMetadataOptions.AffectsRender,propertyChangedCallback:FillColorPropertyChanged));

        public Orientation Orientation { get;}

        static void FillColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UniAxisPickerAdorner uniadorner && e.NewValue is Color newcolor)
            {
                uniadorner._brush = new SolidColorBrush(newcolor);
            }
        }
        public override void Draw(DrawingContext dwgContext)
        {
            //based upon the orientation get the X or Y value.
            var arrow_height = 10;
            var arrow_width = 14;
            var y = ActualHeight*PercentY;
            var x = -4;

            var triangleGeometry = new StreamGeometry();
            using (var context = triangleGeometry.Open())
            {
                context.BeginFigure(new Point(x, y + arrow_height / 2), true, true);
                context.LineTo(new Point(x + arrow_width, y), true, true);
                context.LineTo(new Point(x, y - arrow_height / 2), true, true);
            }

            var rightTri = triangleGeometry.Clone();
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(-1, 1));
            transformGroup.Children.Add(new TranslateTransform(ActualWidth, 0));
            rightTri.Transform = transformGroup;


            dwgContext.DrawGeometry(_brush, _pen, triangleGeometry);
            dwgContext.DrawGeometry(_brush, _pen, rightTri);
        }
    }
}