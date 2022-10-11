using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.Threading;
//using System.Drawing;

namespace Haley.Utils {

    //https://stackoverflow.com/questions/6890472/wpf-maximize-window-with-windowstate-problem-application-will-hide-windows-ta
    //https://social.msdn.microsoft.com/Forums/vstudio/en-US/e77c3b58-41f6-4534-8a92-be0f8287b734/windowstate-maximize-but-not-full-screen?forum=wpf
    //https://stackoverflow.com/questions/46451382/wpf-window-is-under-top-left-placed-taskbar-in-maximized-state/46465322#46465322

    public class WindowStateUtil {
        const int MONITOR_DEFAULTTONEAREST = 0x00000002;
        #region Native Imports
        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

        public static void Minimize(Window window) {
            if (window == null) {
                return;
            }

            window.WindowState = WindowState.Minimized;
        }
        public static void Restore(Window window) {
            if (window == null) {
                return;
            }

            window.WindowState = WindowState.Normal;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
        }
        public static void AddMaximizeHook(Window window) {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(WindowChangeHandler);
        }
        public static void Maximize(Window window) {
            window.ResizeMode = ResizeMode.NoResize;
            //we should have already subscribed to the hook.
            window.WindowState = WindowState.Maximized; //Since we have already set up a hook, before maximizing we receive the message and we set handled.
        }
        private static IntPtr WindowChangeHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {

            //This handler will be called for all changes. But we should focus only on Maximize (0x0024)

            switch (msg) {
                case 0x0024:
                    // Create working area dimensions, converted to DPI-independent values
                    MaximizeInternal(hwnd, lParam);
                    handled = true; //Important or else system will also try to maximize (which will nullify our efforts)
                    break;
            }
            return (IntPtr)0; // return a null pointer.
        }
        private static void MaximizeInternal(IntPtr hwnd, IntPtr lParam) {
            //To find the minimum size of the window
            var source = HwndSource.FromHwnd(hwnd);
            //assuming source is a window
            var wndow = source.RootVisual as Window; //to be used only for getting the min,max widths.
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            // Adjust the maximized size and position to fit the work area of the correct monitor
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);

                if (wndow != null) {
                    mmi.ptMinTrackSize.x = (int) wndow.MinWidth;
                    mmi.ptMinTrackSize.y =(int) wndow.MinHeight;
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
    }

    /// <summary>
    /// POINT aka POINTAPI
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
        /// <summary>
        /// x coordinate of point.
        /// </summary>
        public int x;
        /// <summary>
        /// y coordinate of point.
        /// </summary>
        public int y;

        /// <summary>
        /// Construct a point of coordinates (x,y).
        /// </summary>
        public POINT(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    };

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO {
        /// <summary>
        /// </summary>            
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

        /// <summary>
        /// </summary>            
        public RECT rcMonitor = new RECT();

        /// <summary>
        /// </summary>            
        public RECT rcWork = new RECT();

        /// <summary>
        /// </summary>            
        public int dwFlags = 0;
    }


    /// <summary> Win32 </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT {
        /// <summary> Win32 </summary>
        public int left;
        /// <summary> Win32 </summary>
        public int top;
        /// <summary> Win32 </summary>
        public int right;
        /// <summary> Win32 </summary>
        public int bottom;

        /// <summary> Win32 </summary>
        public static readonly RECT Empty = new RECT();

        /// <summary> Win32 </summary>
        public int Width {
            get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
        }
        /// <summary> Win32 </summary>
        public int Height {
            get { return bottom - top; }
        }

        /// <summary> Win32 </summary>
        public RECT(int left, int top, int right, int bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }


        /// <summary> Win32 </summary>
        public RECT(RECT rcSrc) {
            this.left = rcSrc.left;
            this.top = rcSrc.top;
            this.right = rcSrc.right;
            this.bottom = rcSrc.bottom;
        }

        /// <summary> Win32 </summary>
        public bool IsEmpty {
            get {
                // BUGBUG : On Bidi OS (hebrew arabic) left > right
                return left >= right || top >= bottom;
            }
        }
        /// <summary> Return a user friendly representation of this struct </summary>
        public override string ToString() {
            if (this == RECT.Empty) { return "RECT {Empty}"; }
            return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
        }

        /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
        public override bool Equals(object obj) {
            if (!(obj is Rect)) { return false; }
            return (this == (RECT)obj);
        }

        /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
        public override int GetHashCode() {
            return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
        }


        /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
        public static bool operator ==(RECT rect1, RECT rect2) {
            return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
        }

        /// <summary> Determine if 2 RECT are different(deep compare)</summary>
        public static bool operator !=(RECT rect1, RECT rect2) {
            return !(rect1 == rect2);
        }
    }
}
