using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Haley.Models
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
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

                if (!double.IsNaN(left))
                {
                    Canvas.SetLeft(_ctrl, left + e.HorizontalChange);
                }

                if (!double.IsNaN(right))
                {
                    Canvas.SetRight(_ctrl, right - e.HorizontalChange);
                }

                if (!double.IsNaN(top))
                {
                    Canvas.SetTop(_ctrl, top + e.VerticalChange);
                }

                if (!double.IsNaN(bottom))
                {
                    Canvas.SetBottom(_ctrl, bottom- e.VerticalChange);
                }
            }
        }
    }
}
