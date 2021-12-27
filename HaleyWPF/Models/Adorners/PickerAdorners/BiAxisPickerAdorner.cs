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
    /// This adorner needs a canvas or a rectanlge and it will move across the X & Y direction. Base upon the mouse position, the slider can be placed anywhere on the canvas/rectangle.
    /// </summary>
    public class BiAxisPickerAdorner : PickerAdornerBase
    {
        private static readonly Brush FillBrush = Brushes.Transparent;
        private static readonly Pen InnerRingPen = new Pen(Brushes.White, 2);
        private static readonly Pen OuterRingPen = new Pen(Brushes.Black, 2);

        public BiAxisPickerAdorner(UIElement adornedElement) : base(adornedElement,true)
        {
        }

        public override void Draw(DrawingContext dwgContext)
        {
            //Draw the adorner
            //We have required properties from the base.
            //remember, the size might have changed. So, the position might also have changed. We should focus on the percent of the initial selection.
            var newposition = new Point((ActualWidth * PercentX), (ActualHeight * PercentY));
            dwgContext.DrawEllipse(FillBrush, InnerRingPen, newposition, 4, 4);
            dwgContext.DrawEllipse(FillBrush, OuterRingPen, newposition, 6, 6);

            if (ActualHeight < 2 || ActualWidth < 2)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }
    }
}