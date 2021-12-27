using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Haley.Models
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private Canvas _getCanvas(DependencyObject input,int parentlevel)
        {
            try
            {
                if (parentlevel == 0 || input == null) return null;

                var _parent = VisualTreeHelper.GetParent(input);
                //if parent is canvas, return it.
                if (_parent is Canvas _canvasParent) return _canvasParent;

                //If not, loop again
                return _getCanvas(_parent, parentlevel - 1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control _ctrl = this.DataContext as Control;

            if (_ctrl != null)
            {
                double left = Canvas.GetLeft(_ctrl);
                double top = Canvas.GetTop(_ctrl);
                double right = Canvas.GetRight(_ctrl);
                double bottom = Canvas.GetBottom(_ctrl);
                var _parent = _getCanvas(this, 3);
                Canvas _targetCanvas = _parent as Canvas;
                if (!double.IsNaN(left))
                {
                    var _newvalue = left + e.HorizontalChange;
                    if (_newvalue < 0) _newvalue = 0;
                    if (_targetCanvas != null && _newvalue > (_targetCanvas.ActualWidth - this.ActualWidth))
                    {
                        _newvalue = (_targetCanvas.ActualWidth - this.ActualWidth);
                    }
                   
                    Canvas.SetLeft(_ctrl, _newvalue);
                }

                if (!double.IsNaN(right))
                {
                    var _newvalue = right - e.HorizontalChange;
                    if (_newvalue < 0) _newvalue = 0;
                    if (_targetCanvas != null && _newvalue > (_targetCanvas.ActualWidth-this.ActualWidth))
                    {
                        _newvalue = (_targetCanvas.ActualWidth - this.ActualWidth);
                    }
                    Canvas.SetRight(_ctrl, _newvalue);
                }

                if (!double.IsNaN(top))
                {
                    var _newvalue = top + e.VerticalChange;
                    if (_newvalue < 0) _newvalue = 0;
                    if (_targetCanvas != null && _newvalue > (_targetCanvas.ActualHeight - this.ActualHeight))
                    {
                        _newvalue = (_targetCanvas.ActualHeight - this.ActualHeight);
                    }
                    Canvas.SetTop(_ctrl, _newvalue);
                }

                if (!double.IsNaN(bottom))
                {
                    var _newvalue = bottom - e.VerticalChange;
                    if (_newvalue < 0) _newvalue = 0;
                    if (_targetCanvas != null && _newvalue > (_targetCanvas.ActualHeight - this.ActualHeight))
                    {
                        _newvalue = (_targetCanvas.ActualHeight - this.ActualHeight);
                    }
                    Canvas.SetBottom(_ctrl, _newvalue);
                }
            }
        }
    }
}
