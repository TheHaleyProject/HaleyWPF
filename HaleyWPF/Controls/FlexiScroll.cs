using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    //[TemplatePart(Name = UIERepeatDown, Type = typeof(RepeatButton))]
    //[TemplatePart(Name = UIERepeatUp, Type = typeof(RepeatButton))]
    [TemplatePart(Name = UIERoot, Type = typeof(FrameworkElement))]
    public class FlexiScroll : ScrollViewer, ICornerRadius
    {
        //private const string UIERepeatDown = "PART_RepeatDown";
        //private const string UIERepeatUp = "PART_RepeatUp";
        //private const string UIETrack = "PART_Track";
        private const string UIERoot = "PART_root";

        //private RepeatButton _repeatUp;
        //private RepeatButton _repeatDown;
        //private Track _track;
        private FrameworkElement _root;
        private RoutedEventHandler _lineUp = null;
        private RoutedEventHandler _lineDown = null;
        private RoutedEventHandler _lineLeft = null;
        private RoutedEventHandler _lineRight = null;

        static FlexiScroll()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiScroll), new FrameworkPropertyMetadata(typeof(FlexiScroll)));
        }

        public FlexiScroll() { }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _root = GetTemplateChild(UIERoot) as FrameworkElement;
            _associateExternalButtons();
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(FlexiScroll), new PropertyMetadata(ResourceHelper.cornerRadius));

        //public ImageSource Arrow
        //{
        //    get { return (ImageSource)GetValue(ArrowProperty); }
        //    set { SetValue(ArrowProperty, value); }
        //}

        //public static readonly DependencyProperty ArrowProperty =
        //    DependencyProperty.Register(nameof(Arrow), typeof(ImageSource), typeof(FlexiScroll), new PropertyMetadata(ResourceHelper.getIcon(IconEnums.arrow_line_medium.ToString())));

        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register(nameof(ThumbBackground), typeof(Brush), typeof(FlexiScroll), new PropertyMetadata(null));

        public Brush TrackBackground
        {
            get { return (Brush)GetValue(TrackBackgroundProperty); }
            set { SetValue(TrackBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.Register(nameof(TrackBackground), typeof(Brush), typeof(FlexiScroll), new PropertyMetadata(null));

        public Brush RepeatBackground
        {
            get { return (Brush)GetValue(RepeatBackgroundProperty); }
            set { SetValue(RepeatBackgroundProperty, value); }
        }

        public static readonly DependencyProperty RepeatBackgroundProperty =
            DependencyProperty.Register(nameof(RepeatBackground), typeof(Brush), typeof(FlexiScroll), new PropertyMetadata(null));

        public double HorizontalScrollBarSize
        {
            get { return (double)GetValue(HorizontalScrollBarSizeProperty); }
            set { SetValue(HorizontalScrollBarSizeProperty, value); }
        }

        public static readonly DependencyProperty HorizontalScrollBarSizeProperty =
            DependencyProperty.Register(nameof(HorizontalScrollBarSize), typeof(double), typeof(FlexiScroll), new PropertyMetadata(25.0));

        public double VerticalScrollBarSize
        {
            get { return (double)GetValue(VerticalScrollBarSizeProperty); }
            set { SetValue(VerticalScrollBarSizeProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarSizeProperty =
            DependencyProperty.Register(nameof(VerticalScrollBarSize), typeof(double), typeof(FlexiScroll), new PropertyMetadata(25.0));

        public bool ShowVerticalButtons
        {
            get { return (bool)GetValue(ShowVerticalButtonsProperty); }
            set { SetValue(ShowVerticalButtonsProperty, value); }
        }

        public static readonly DependencyProperty ShowVerticalButtonsProperty =
            DependencyProperty.Register(nameof(ShowVerticalButtons), typeof(bool), typeof(FlexiScroll), new PropertyMetadata(true));

        public bool ShowHorizontalButtons
        {
            get { return (bool)GetValue(ShowHorizontalButtonsProperty); }
            set { SetValue(ShowHorizontalButtonsProperty, value); }
        }

        public static readonly DependencyProperty ShowHorizontalButtonsProperty =
            DependencyProperty.Register(nameof(ShowHorizontalButtons), typeof(bool), typeof(FlexiScroll), new PropertyMetadata(true));

        public bool EnableOverLay
        {
            get { return (bool)GetValue(EnableOverLayProperty); }
            set { SetValue(EnableOverLayProperty, value); }
        }

        public static readonly DependencyProperty EnableOverLayProperty =
            DependencyProperty.Register(nameof(EnableOverLay), typeof(bool), typeof(FlexiScroll), new PropertyMetadata(true));

        public bool AutoHide
        {
            get { return (bool)GetValue(AutoHideProperty); }
            set { SetValue(AutoHideProperty, value); }
        }

        public static readonly DependencyProperty AutoHideProperty =
            DependencyProperty.Register(nameof(AutoHide), typeof(bool), typeof(FlexiScroll), new PropertyMetadata(false));

        #region Repeat Buttons
        public RepeatButton RepeatUp
        {
            get { return (RepeatButton)GetValue(RepeatUpProperty); }
            set { SetValue(RepeatUpProperty, value); }
        }

        public static readonly DependencyProperty RepeatUpProperty =
            DependencyProperty.Register(nameof(RepeatUp), typeof(RepeatButton), typeof(FlexiScroll), new FrameworkPropertyMetadata(null));

        public RepeatButton RepeatDown
        {
            get { return (RepeatButton)GetValue(RepeatDownProperty); }
            set { SetValue(RepeatDownProperty, value); }
        }

        public static readonly DependencyProperty RepeatDownProperty =
            DependencyProperty.Register(nameof(RepeatDown), typeof(RepeatButton), typeof(FlexiScroll), new FrameworkPropertyMetadata(null));

        public RepeatButton RepeatLeft
        {
            get { return (RepeatButton)GetValue(RepeatLeftProperty); }
            set { SetValue(RepeatLeftProperty, value); }
        }

        public static readonly DependencyProperty RepeatLeftProperty =
            DependencyProperty.Register(nameof(RepeatLeft), typeof(RepeatButton), typeof(FlexiScroll), new FrameworkPropertyMetadata(null));

        public RepeatButton RepeatRight
        {
            get { return (RepeatButton)GetValue(RepeatRightProperty); }
            set { SetValue(RepeatRightProperty, value); }
        }

        public static readonly DependencyProperty RepeatRightProperty =
            DependencyProperty.Register(nameof(RepeatRight), typeof(RepeatButton), typeof(FlexiScroll), new FrameworkPropertyMetadata(null));

        void _associateExternalButtons()
        {
            if (_root == null) return;

            //For repeat up
            if (RepeatUp != null)
            {
                if (_lineUp == null)
                {
                    //Reason for using a private variable event handler is to ensure that we can unsubscribe and subscribe to same event.
                    _lineUp = (sender, args) => { ScrollBar.LineUpCommand.Execute(null, _root); };
                }
                RepeatUp.Click -= _lineUp;
                RepeatUp.Click += _lineUp;
            }

            //For repeat down
            if (RepeatDown != null)
            {
                if (_lineDown == null)
                {
                    //Reason for using a private variable event handler is to ensure that we can unsubscribe and subscribe to same event.
                    _lineDown = (sender, args) => { ScrollBar.LineDownCommand.Execute(null, _root); };
                }
                RepeatDown.Click -= _lineDown;
                RepeatDown.Click += _lineDown;
            }

            //For repeat Left
            if (RepeatLeft != null)
            {
                if (_lineLeft == null)
                {
                    //Reason for using a private variable event handler is to ensure that we can unsubscribe and subscribe to same event.
                    _lineLeft = (sender, args) => { ScrollBar.LineLeftCommand.Execute(null, _root); };
                }
                RepeatLeft.Click -= _lineLeft;
                RepeatLeft.Click += _lineLeft;
            }

            //For repeat Right
            if (RepeatRight != null)
            {
                if (_lineRight == null)
                {
                    //Reason for using a private variable event handler is to ensure that we can unsubscribe and subscribe to same event.
                    _lineRight = (sender, args) => { ScrollBar.LineRightCommand.Execute(null, _root); };
                }
                RepeatRight.Click -= _lineRight;
                RepeatRight.Click += _lineRight;
            }
        }
        #endregion
    }
}
