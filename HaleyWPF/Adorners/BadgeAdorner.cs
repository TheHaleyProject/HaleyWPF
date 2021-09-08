using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using Haley.WPF.Controls;
using System.Windows.Input;

namespace Haley.Models
{
    public class BadgeAdorner : Adorner
    {
        private VisualCollection _visuals;
        private ContentPresenter _contentPresenter;
        private Badge _badge;

        public object Content
        {
            get { return _contentPresenter.Content; }
            set { _contentPresenter.Content = value; }
        }

        public BadgeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _contentPresenter = new ContentPresenter();
            //AddVisualChild(_contentPresenter);
            _visuals = new VisualCollection(this);
            _visuals.Add(_contentPresenter); //This is to add to the target element.
            this.MouseLeftButtonDown += BadgeAdorner_MouseLeftButtonDown;
        }

        private void BadgeAdorner_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //If command is not empty, raise it with parameter.
        }

        public BadgeAdorner(UIElement adornedElement, Visual content)
          : this(adornedElement)
        { 
            Content = content; 
        }

        public BadgeAdorner(UIElement adornedElement, Badge badge)
         : this(adornedElement)
        {
            _badge = badge;
            if (_badge.Content != null)
            {
                //Give priority to the content.
                Content = _badge.Content;
            }
        }

        //Below method overrides the render. So DON'T USE.
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    _ContentPresenter.Measure(constraint);
        //    return _ContentPresenter.DesiredSize;
        //}

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Required. So that the content presenter gets arranged.
            _contentPresenter.Arrange(new Rect(0, 0,
                 finalSize.Width, finalSize.Height));
            return _contentPresenter.RenderSize;
        }
        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index]; 
        }
        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        private void _drawCircle(DrawingContext drawingContext)
        {

        }

        private void _drawRectangle(DrawingContext drawingContext)
        {

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Content != null) return; //This means, if we have set the content, we will not proceed further.

            //Below is for directly drawing on top of the element.
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            Point TopLeft = new Point(0, 0);
            Point TopRight = new Point(ActualWidth, 0);
            Point BottomLeft = new Point(0,ActualHeight);
            Point BottomRight = new Point(ActualWidth, ActualHeight);

            //Rectanlge always draws in RIGHT- AND THEN DOWN DIRECTION.
            double _size = 25.0;
            var _XOrigin = TopRight.X - (_size / 2);
            var _YOrigin = TopRight.Y - (_size / 2);
            var _newpoint = new Point(_XOrigin, _YOrigin);
            drawingContext.DrawRectangle(Brushes.Red, renderPen, new Rect(_newpoint, new Size(_size,_size)));

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            

            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, BottomRight, _size, _size);
        }
    }
}
