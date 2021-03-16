using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Haley.WPF.BaseControls
{
    public class FlexiScroll : ScrollViewer
    {
        static FlexiScroll()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiScroll), new FrameworkPropertyMetadata(typeof(FlexiScroll)));
        }

        public FlexiScroll() {}

        public double ScrollBarWidth
        {
            get { return (double)GetValue(ScrollBarWidthProperty); }
            set { SetValue(ScrollBarWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollBarWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollBarWidthProperty =
            DependencyProperty.Register(nameof(ScrollBarWidth), typeof(double), typeof(FlexiScroll), new PropertyMetadata(25.0));

        public bool ShowRepeatButtons
        {
            get { return (bool)GetValue(ShowRepeatButtonsProperty); }
            set { SetValue(ShowRepeatButtonsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowRepeatButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowRepeatButtonsProperty =
            DependencyProperty.Register(nameof(ShowRepeatButtons), typeof(bool), typeof(FlexiScroll), new PropertyMetadata(true));

        public RepeatButton RepeatUp
        {
            get { return (RepeatButton)GetValue(RepeatUpProperty); }
            set { SetValue(RepeatUpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RepeatUp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RepeatUpProperty =
            DependencyProperty.Register(nameof(RepeatUp), typeof(RepeatButton), typeof(FlexiScroll), new FrameworkPropertyMetadata(null,propertyChangedCallback: RepeatUpPropertyChanged));
        static void RepeatUpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlexiScroll fs = d as FlexiScroll;
            fs.RepeatUp.Click -= RepeatUp_Click;
            fs.RepeatUp.Click += RepeatUp_Click;
        }

        private static void RepeatUp_Click(object sender, RoutedEventArgs e)
        {
            //On click get the repeat button from the scroll viewer and assign there
            //ScrollBar.LineUpCommand.Execute(null,) //Should start at repeat button.
        }
    }
}
