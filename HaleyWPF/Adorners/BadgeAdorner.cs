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
    public class BadgeAdorner : Adorner
    {
        Point TopLeft = new Point(0, 0);
        Point TopRight = new Point(0, 0);
        Point BottomLeft = new Point(0, 0);
        Point BottomRight = new Point(0, 0);
        Point TopCenter = new Point(0, 0);
        Point BottomCenter = new Point(0, 0);
        Point LeftCenter = new Point(0, 0);
        Point RightCenter = new Point(0, 0);
        Point Center = new Point(0, 0);

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

        private void BadgeAdorner_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //If command is not empty, raise it with parameter.
            if (_badge.Command != null)
            {
                _badge.Command.Execute(_badge.CommandParameter);
            }
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
            if (_badge.CustomShape != null)
            {
                //Give priority to the content.
                Content = _badge.CustomShape;
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
            var renderPen = _getPen();
            var renderBrush = _getBackground();
            var _size = _getSize();
            var _target = _getTargetPoint();
            var _anchor = _getAnchor(_target, _size.X, _size.Y);
            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, _anchor, _size.X/2, _size.Y/2);

            if (!string.IsNullOrWhiteSpace(_badge.Label))
            {
                _drawLabel(drawingContext,_anchor,_size.X);
            }
        }

        private void _drawLabel(DrawingContext drawingContext,Point anchor,double maxWidth)
        {
            var foreground = _getForeground();
            FormattedText _text = new FormattedText(_badge.Label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), _badge.FontSize, foreground);
            _text.TextAlignment = TextAlignment.Center;
            //_text.MaxTextWidth = maxWidth;
            var _neworigin = new Point(anchor.X, anchor.Y - _text.Height / 2); //To make it vertically centered.
            drawingContext.DrawText(_text, _neworigin);
        }

        private void _drawRectangle(DrawingContext drawingContext)
        {
            var renderPen = _getPen();
            var renderBrush = _getBackground();
            var _size = _getSize();
            var _target = _getTargetPoint();
            var _anchor = _getAnchor(_target, _size.X, _size.Y);
            //Rectanlge always draws in RIGHT- AND THEN DOWN DIRECTION. So try to change the center of the rectangle.
            var _XOrigin = _anchor.X - (_size.X / 2);
            var _YOrigin = _anchor.Y - (_size.Y / 2);
            var _startPoint = new Point(_XOrigin, _YOrigin);

            //If we have a value for cornerradius, then we draw rounded rectangle.
            var _crnrrad = _badge.CornerRadius;
            if (_crnrrad > 0)
            {
                drawingContext.DrawRoundedRectangle(renderBrush, renderPen, new Rect(_startPoint, new Size(_size.X, _size.Y)),_crnrrad/2,  _crnrrad/2);
            }
            else
            {
                drawingContext.DrawRectangle(renderBrush, renderPen, new Rect(_startPoint, new Size(_size.X, _size.Y)));
            }

            if (!string.IsNullOrWhiteSpace(_badge.Label))
            {
                _drawLabel(drawingContext, _anchor,_size.X);
            }
        }

        private (double X,double Y) _getSize()
        {
            //Get X value
            double _x = 30.0; 
            if (_badge.Size.Width > 1)
            {
                _x = _badge.Size.Width;
            }

            double _y = 0.0;
            if (_badge.Size.Height > 1)
            {
                _y = _badge.Size.Height;
            }
            else
            {
                //If it equals X. Unless user specifically defines Y, we take it symmetric
                _y = _x;
            }
            return (_x, _y);
        }

        private void _getCorners()
        {
            TopLeft = new Point(0, 0);
            TopRight = new Point(ActualWidth, 0);
            BottomLeft = new Point(0, ActualHeight);
            BottomRight = new Point(ActualWidth, ActualHeight);
            TopCenter = new Point(ActualWidth/2, 0);
            BottomCenter = new Point(ActualWidth/2, ActualHeight);
            LeftCenter = new Point(0, ActualHeight/2);
            RightCenter = new Point(ActualWidth , ActualHeight/2);
            Center = new Point(ActualWidth/2, ActualHeight / 2);
        }

        private Pen _getPen()
        {
            Pen renderPen = null;
            //Pen is basically for the border brush purpose.
            if (_badge.BorderBrush != null )
            {
                //renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
                renderPen = new Pen(_badge.BorderBrush,_badge.BorderThickness);
            }

            return renderPen;
        }

        private Point _getAnchor(Point Origin, double SizeX, double SizeY)
        {
            Point _newOrigin = Origin;
            //This is to get the corners of the shpae object. But instead, we are going to change the origin(center point). So that the shape adjusts to the desired location.
            switch (_badge.Anchor)
            {
                case Enums.BadgeAnchor.TopLeft:
                    //Shape has to attach on its top left to the actual origin. 
                    //We just move the actual origin to right side, so that the shape looks like it is attached on the top left.
                    _newOrigin = new Point((Origin.X + SizeX/2), (Origin.Y + SizeY/2)); //Moves right and moves down.
                    break;
                case Enums.BadgeAnchor.TopRight:
                    _newOrigin = new Point((Origin.X - SizeX/2), (Origin.Y + SizeY/2)); //Moves left and moves down.
                    break;
                case Enums.BadgeAnchor.BottomLeft:
                    _newOrigin = new Point((Origin.X + SizeX/2), (Origin.Y - SizeY/2)); //Moves right and moves up.
                    break;
                case Enums.BadgeAnchor.BottomRight:
                    _newOrigin = new Point((Origin.X - SizeX/2), (Origin.Y - SizeY/2)); //Moves left and moves up.
                    break;
                default:
                case Enums.BadgeAnchor.Center:
                    _newOrigin = Origin; //The actual point is considered the center.
                    break;
            }

            //Add margin to the neworigin
                _newOrigin = new Point(_newOrigin.X + _badge.MarginX, _newOrigin.Y + _badge.MarginY);
            return _newOrigin;
        }

        private Point _getTargetPoint()
        {
            switch (_badge.Alignment)
            {
                case Enums.BadgeAlignment.TopRight:
                    return TopRight;
                case Enums.BadgeAlignment.TopLeft:
                    return TopLeft;
                case Enums.BadgeAlignment.BottomLeft:
                    return BottomLeft;
                case Enums.BadgeAlignment.BottomRight:
                    return BottomRight;
                case Enums.BadgeAlignment.TopCenter:
                    return TopCenter;
                case Enums.BadgeAlignment.BottomCenter:
                    return BottomCenter;
                case Enums.BadgeAlignment.LeftCenter:
                    return LeftCenter;
                case Enums.BadgeAlignment.RightCenter:
                    return RightCenter;
                case Enums.BadgeAlignment.Center:
                    return Center;
            }
            return TopRight;
        }

        private Brush _getForeground()
        {
            if (_badge.Foreground != null)
            {
                return _badge.Foreground;
            }
            SolidColorBrush _brush = new SolidColorBrush(Colors.White);
            return _brush;
        }

        private Brush _getBackground()
        {
            if (_badge.Background != null)
            {
                return _badge.Background;
            }
            SolidColorBrush _brush = null;
            //Else check the kind and then return a specific color.
            switch (_badge.Kind)
            {
                case Enums.BadgeType.Success:
                    _brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF29A65C");
                    break;
                case Enums.BadgeType.Info:
                    _brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2F6088");
                    break;
                case Enums.BadgeType.Warning:
                    _brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFBF8A29");
                    break;
                case Enums.BadgeType.Error:
                    _brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFEB630A");
                    break;
            }
            return _brush;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //if not visible then don't proceed at all.
            if (!_badge.IsVisible)
            {
                //Reason why we set this below property is that for content, the adorner is already prepared via constructor.
                this.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                return;
            }
            if (Content != null) return; //This means, if we have set the content, we will not proceed further.

            _getCorners();
            switch (_badge.Shape)
            {
                case Enums.BadgeShape.Ellipse:
                    _drawCircle(drawingContext);
                    break;
                case Enums.BadgeShape.Rectangle:
                    _drawRectangle(drawingContext);
                    break;
            }
        }
    }
}
