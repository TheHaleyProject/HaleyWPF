using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using Haley.WPF.Controls;
using System.Globalization;
using Haley.Enums;
using System.Windows.Input;
using Haley.Events;
using Haley.Utils;

namespace Haley.Models
{
    //A slider can be in vertical direction or horizontal direction. Sometimes, it can also move across a canvas in both direction.

    /// <summary>
    /// Slider Picker Adorner.
    /// </summary>
    public abstract class PickerAdornerBase : Adorner
    {

        public event EventHandler<ValueChangedArgs<(Point position,Point percent)>> PositionChangedEvent;
        
        //Add a dependency property which will trigger onrender.
        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(Point), typeof(PickerAdornerBase), new FrameworkPropertyMetadata(new Point(0,0),FrameworkPropertyMetadataOptions.AffectsRender)); //This affects the render .  So, on render is triggered.

        protected double PercentX; //Positions percentage with respect to the element size.
        protected double PercentY;

        public PickerAdornerShape AdornerShape { get; set; }
        public string Id { get; }
        public bool IsBiAxis { get; set; }
        public AdornerLayer Layer { get; set; }
        //Let us not override the OnRender here. Instead we will override in the concrete classes..
        public PickerAdornerBase(UIElement adornedElement,bool is_biaxis) : base(adornedElement)
        {
            IsHitTestVisible = false;
            Id = Guid.NewGuid().ToString();
            IsBiAxis = is_biaxis;
        }

        public void SetPosition(Point position)
        {
            //Instead of using the actualwidth/ actual height, we can also do AdornedElement.DesiredSize.Width & AdornedElement.DesiredSize.Height

            //check if current position and new position are same.
            if (Position.X == position.X && Position.Y == position.Y) return; //Do not update same values.

            //TODO: Clamp the position to set within the adorned element size.

            PercentX = (ActualWidth != 0.0) ? (position.X / ActualWidth) : 0;
            PercentY = (ActualHeight != 0.0) ? (position.Y / ActualHeight) : 0;
            this.SetCurrentValue(PositionProperty, position);
            var args = new ValueChangedArgs<(Point position, Point percent)>((position, new Point(PercentX, PercentY)), null);
            PositionChangedEvent?.Invoke(this, args); //Send this new position.
        }

        public void SetPosition(double x_percent, double y_percent)
        {
            //Now, based upon the new percentage value, we change the position.
            var _newX = x_percent * ActualWidth;
            var _newY = y_percent * ActualHeight;
            SetPosition(new Point(_newX, _newY));
        }

        public abstract void Draw(DrawingContext dwgContext);
        public virtual void UnSubscribe()
        {
            //In case we have some subcriptions made, we unsubscribe them here.
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Draw(drawingContext); //Call draw.
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //Checkout this stackoverflow answer : https://stackoverflow.com/a/2521222
            var result = base.MeasureOverride(constraint);
            InvalidateVisual(); //remember that the position has changed with respect to the new size. So, update the position as well. Since we are using the percentage method, we donot need to worry about updating the position of adorner.
            return result;
        }
    }
}