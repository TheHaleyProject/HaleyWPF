using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.Models
{
    public class FlexiScrollViewer : DependencyObject
    {

        public static readonly DependencyProperty AutoHideProperty =
            DependencyProperty.RegisterAttached("AutoHide", typeof(bool), typeof(FlexiScrollViewer), new PropertyMetadata(false));

        public static readonly DependencyProperty EnableOverLayProperty =
            DependencyProperty.RegisterAttached("EnableOverLay", typeof(bool), typeof(FlexiScrollViewer), new PropertyMetadata(false));

        public static readonly DependencyProperty HorizontalBarSizeProperty =
            DependencyProperty.RegisterAttached("HorizontalBarSize", typeof(double), typeof(FlexiScrollViewer), new PropertyMetadata(18.0));

        public static readonly DependencyProperty HorizontalBarVisibilityProperty =
            DependencyProperty.RegisterAttached("HorizontalBarVisibility", typeof(ScrollBarVisibility), typeof(FlexiScrollViewer), new PropertyMetadata(ScrollBarVisibility.Auto));

        public static readonly DependencyProperty RepeatBackgroundProperty =
            DependencyProperty.RegisterAttached("RepeatBackground", typeof(Brush), typeof(FlexiScrollViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowButtonsProperty =
                                    DependencyProperty.RegisterAttached("ShowButtons", typeof(bool), typeof(FlexiScrollViewer), new PropertyMetadata(true));
        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.RegisterAttached("ThumbBackground", typeof(Brush), typeof(FlexiScrollViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.RegisterAttached("TrackBackground", typeof(Brush), typeof(FlexiScrollViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty VerticalBarSizeProperty =
                            DependencyProperty.RegisterAttached("VerticalBarSize", typeof(double), typeof(FlexiScrollViewer), new PropertyMetadata(18.0));

        public static readonly DependencyProperty VerticalBarVisibilityProperty =
            DependencyProperty.RegisterAttached("VerticalBarVisibility", typeof(ScrollBarVisibility), typeof(FlexiScrollViewer), new PropertyMetadata(ScrollBarVisibility.Auto));

        public FlexiScrollViewer() { }

        public static bool GetAutoHide(DependencyObject obj) {
            return (bool)obj.GetValue(AutoHideProperty);
        }

        public static bool GetEnableOverLay(DependencyObject obj) {
            return (bool)obj.GetValue(EnableOverLayProperty);
        }

        public static double GetHorizontalBarSize(DependencyObject obj) {
            return (double)obj.GetValue(HorizontalBarSizeProperty);
        }

        public static ScrollBarVisibility GetHorizontalBarVisibility(DependencyObject obj) {
            return (ScrollBarVisibility)obj.GetValue(HorizontalBarVisibilityProperty);
        }

        public static Brush GetRepeatBackground(DependencyObject obj) {
            return (Brush)obj.GetValue(RepeatBackgroundProperty);
        }

        public static bool GetShowButtons(DependencyObject obj) {
            return (bool)obj.GetValue(ShowButtonsProperty);
        }

        public static Brush GetThumbBackground(DependencyObject obj) {
            return (Brush)obj.GetValue(ThumbBackgroundProperty);
        }

        public static Brush GetTrackBackground(DependencyObject obj) {
            return (Brush)obj.GetValue(TrackBackgroundProperty);
        }

        public static double GetVerticalBarSize(DependencyObject obj) {
            return (double)obj.GetValue(VerticalBarSizeProperty);
        }

        public static ScrollBarVisibility GetVerticalBarVisibility(DependencyObject obj) {
            return (ScrollBarVisibility)obj.GetValue(VerticalBarVisibilityProperty);
        }

        public static void SetAutoHide(DependencyObject obj, bool value) {
            obj.SetValue(AutoHideProperty, value);
        }

        public static void SetEnableOverLay(DependencyObject obj, bool value) {
            obj.SetValue(EnableOverLayProperty, value);
        }

        public static void SetHorizontalBarSize(DependencyObject obj, double value) {
            obj.SetValue(HorizontalBarSizeProperty, value);
        }

        public static void SetHorizontalBarVisibility(DependencyObject obj, ScrollBarVisibility value) {
            obj.SetValue(HorizontalBarVisibilityProperty, value);
        }

        public static void SetRepeatBackground(DependencyObject obj, Brush value) {
            obj.SetValue(RepeatBackgroundProperty, value);
        }

        public static void SetShowButtons(DependencyObject obj, bool value) {
            obj.SetValue(ShowButtonsProperty, value);
        }

        public static void SetThumbBackground(DependencyObject obj, Brush value) {
            obj.SetValue(ThumbBackgroundProperty, value);
        }
        public static void SetTrackBackground(DependencyObject obj, Brush value) {
            obj.SetValue(TrackBackgroundProperty, value);
        }
        public static void SetVerticalBarSize(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalBarSizeProperty, value);
        }
        public static void SetVerticalBarVisibility(DependencyObject obj, ScrollBarVisibility value)
        {
            obj.SetValue(VerticalBarVisibilityProperty, value);
        }
    }
}
