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

                Canvas.SetLeft(_ctrl, left + e.HorizontalChange);
                Canvas.SetTop(_ctrl, top + e.VerticalChange);
            }
        }
    }
}
