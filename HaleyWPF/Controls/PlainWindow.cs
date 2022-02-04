﻿using Haley.Abstractions;
using Haley.Enums;
using Haley.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    [TemplatePart(Name = UIEDragMoveRegion, Type = typeof(Border))]
    [TemplatePart(Name = UIEFooterHolder, Type = typeof(Border))]
    [TemplatePart(Name = UIEControlBoxHolder, Type = typeof(Grid))]
    [TemplatePart(Name = UIEHeader, Type = typeof(ContentControl))]
    [TemplatePart(Name = UIEFooter, Type = typeof(ContentControl))]
    [TemplatePart(Name = UIEControlBox, Type = typeof(ContentControl))]
    [TemplatePart(Name = UIECloseBtn, Type = typeof(Control))]
    [TemplatePart(Name = UIEMinimizeBtn, Type = typeof(Control))]
    [TemplatePart(Name = UIEMaximizeBtn, Type = typeof(Control))]
    [TemplatePart(Name = UIEOverallBorder,Type =typeof(Border))]
    public class PlainWindow : Window, ICornerRadius
    {
        private const string UIEDragMoveRegion = "PART_dragemoveArea";
        private const string UIEFooterHolder = "PART_footer_holder";
        private const string UIEControlBoxHolder = "PART_controlbox_holder";
        private const string UIEOverallBorder = "PART_OverallBorder";

        private const string UIEHeader = "PART_header";
        private const string UIEFooter = "PART_footer";
        private const string UIEControlBox = "PART_controlbox";

        private const string UIEMinimizeBtn = "PART_minimize";
        private const string UIEMaximizeBtn = "PART_maximize";
        private const string UIECloseBtn = "PART_close";

        private Border _dragMoveRegion;
        private Border _footerHolder;
        private Border _overallBorder;
        private Grid _controlboxHolder;

        private ContentControl _header;
        private ContentControl _footer;
        private ContentControl _controlBox;

        private DataTemplate _headerDefaultTemplate;
        private DataTemplate _controlboxDefaultTemplate;

        static PlainWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainWindow), new FrameworkPropertyMetadata(typeof(PlainWindow)));
        }

        

        public PlainWindow() 
        {
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            ControlBoxStyle = ControlBoxStyle.Windows;
            WindowChrome.SetWindowChrome(this, new WindowChrome() {ResizeBorderThickness=new Thickness(4.0),GlassFrameThickness = new Thickness(3.0)}); //for enabling resize in borderless window
            //set border thickness to match the resize border
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Minimize, _minimizeAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Maximize, _maximizeAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Close, _closeAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.WindowDragMove, _dragMoveHeader));
            

            ////This is to limit the maximum height of the screen.
            //MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _overallBorder = GetTemplateChild(UIEOverallBorder) as Border;
            _dragMoveRegion = GetTemplateChild(UIEDragMoveRegion) as Border;
            _footerHolder = GetTemplateChild(UIEFooterHolder) as Border;
            _controlboxHolder = GetTemplateChild(UIEControlBoxHolder) as Grid;

            _header = GetTemplateChild(UIEHeader) as ContentControl;
            _footer = GetTemplateChild(UIEFooter) as ContentControl;
            _controlBox = GetTemplateChild(UIEControlBox) as ContentControl;

            WindowChrome.SetIsHitTestVisibleInChrome(_controlboxHolder, true); //Important or else the control box items hit will not be visible.
            _initiate();
        }

        void _initiate()
        {
            _headerDefaultTemplate = TryFindResource("defaultHeaderTemplate") as DataTemplate;
           
            _setFooter();
            _setHeader();
            _getAndSetControlBox();
            _setWindowCornerRadius(this.CornerRadius);
        }

        public bool ClipBorder
        {
            get { return (bool)GetValue(ClipBorderProperty); }
            set { SetValue(ClipBorderProperty, value); }
        }

        public static readonly DependencyProperty ClipBorderProperty =
            DependencyProperty.Register(nameof(ClipBorder), typeof(bool), typeof(PlainWindow), new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ControlBoxStyle ControlBoxStyle
        {
            get { return (ControlBoxStyle)GetValue(ControlBoxStyleProperty); }
            set { SetValue(ControlBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty ControlBoxStyleProperty =
            DependencyProperty.Register(nameof(ControlBoxStyle), typeof(ControlBoxStyle), typeof(PlainWindow), new PropertyMetadata(ControlBoxStyle.Windows));

        public bool HideCloseButton
        {
            get { return (bool)GetValue(HideCloseButtonProperty); }
            set { SetValue(HideCloseButtonProperty, value); }
        }

        public static readonly DependencyProperty HideCloseButtonProperty =
            DependencyProperty.Register(nameof(HideCloseButton), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));
        public bool HideMinimizeButton
        {
            get { return (bool)GetValue(HideMinimizeButtonProperty); }
            set { SetValue(HideMinimizeButtonProperty, value); }
        }

        public static readonly DependencyProperty HideMinimizeButtonProperty =
            DependencyProperty.Register(nameof(HideMinimizeButton), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));

        public bool HideMaximizeButton
        {
            get { return (bool)GetValue(HideMaximizeButtonProperty); }
            set { SetValue(HideMaximizeButtonProperty, value); }
        }

        public static readonly DependencyProperty HideMaximizeButtonProperty =
            DependencyProperty.Register(nameof(HideMaximizeButton), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));

        public bool HideFooter
        {
            get { return (bool)GetValue(HideFooterProperty); }
            set { SetValue(HideFooterProperty, value); }
        }

        public static readonly DependencyProperty HideFooterProperty =
            DependencyProperty.Register(nameof(HideFooter), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));

        public bool HideHeader
        {
            get { return (bool)GetValue(HideHeaderProperty); }
            set { SetValue(HideHeaderProperty, value); }
        }

        public static readonly DependencyProperty HideHeaderProperty =
            DependencyProperty.Register(nameof(HideHeader), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));

        public bool HideIcon
        {
            get { return (bool)GetValue(HideIconProperty); }
            set { SetValue(HideIconProperty, value); }
        }

        public static readonly DependencyProperty HideIconProperty =
            DependencyProperty.Register(nameof(HideIcon), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));

        public DataTemplate ControlBoxTemplate
        {
            get { return (DataTemplate)GetValue(ControlBoxTemplateProperty); }
            set { SetValue(ControlBoxTemplateProperty, value); }
        }

        public static readonly DependencyProperty ControlBoxTemplateProperty =
            DependencyProperty.Register(nameof(ControlBoxTemplate), typeof(DataTemplate), typeof(PlainWindow), new PropertyMetadata(null,propertyChangedCallback:headerControlBoxChanged));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(PlainWindow), new PropertyMetadata(null,propertyChangedCallback:HeaderTemplateChanged));

        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public static readonly DependencyProperty FooterTemplateProperty =
            DependencyProperty.Register(nameof(FooterTemplate), typeof(DataTemplate), typeof(PlainWindow), new PropertyMetadata(null,propertyChangedCallback:FooterTemplateChanged));

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(double), typeof(PlainWindow), new FrameworkPropertyMetadata(35.0,null,coerceValueCallback: _headerfooterHeightCoerce));
        static object _headerfooterHeightCoerce(DependencyObject d, object baseValue)
        {
            double _actual = (double)baseValue;
            if (_actual < 5.0) return 5.0;
            return baseValue;
        }

        public double FooterHeight
        {
            get { return (double)GetValue(FooterHeightProperty); }
            set { SetValue(FooterHeightProperty, value); }
        }

        public static readonly DependencyProperty FooterHeightProperty =
            DependencyProperty.Register(nameof(FooterHeight), typeof(double), typeof(PlainWindow), new FrameworkPropertyMetadata(25.0,null,coerceValueCallback: _headerfooterHeightCoerce));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainWindow), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius,propertyChangedCallback:_cornerRadiusChanged));

        static void _cornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainWindow pw)
            {
                pw._setWindowCornerRadius((CornerRadius)e.NewValue);
            }
        }
        private void _setWindowCornerRadius(CornerRadius _radius)
        {
            if (_dragMoveRegion != null)
            {
                _dragMoveRegion.CornerRadius = new CornerRadius(_radius.TopLeft, _radius.TopRight, 0.0, 0.0);
            }

            if (!HideFooter && _footerHolder != null)
            {
                _footerHolder.CornerRadius = new CornerRadius(0.0, 0.0, _radius.BottomRight, _radius.BottomLeft);
            }
        }

        void _dragMoveHeader(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (this == null) return;
                this.DragMove();
                e.Handled = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
        void _closeAction(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (this == null) return;
                e.Handled = true;
                this.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
        void _minimizeAction(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (this == null) return;
                e.Handled = true;
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
        void _maximizeAction(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (this == null) return;
                e.Handled = true;
                switch (this.WindowState)
                {
                    case WindowState.Maximized:
                        this.WindowState = WindowState.Normal;
                        break;
                    case WindowState.Normal:
                        this.WindowState = WindowState.Maximized;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
        static void HeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainWindow pw)
            {
                pw._setHeader();
            }
        }
        static void FooterTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if (d is PlainWindow pw)
            {
                pw._setFooter();
            }
        }

        static void headerControlBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainWindow pw)
            {
                pw._getAndSetControlBox();
            }
        }

        void _getAndSetControlBox()
        {
            if (ControlBoxTemplate != null) _setControlBox(); //User defined custom tempate is not empty.

            switch (ControlBoxStyle)
            {
                case ControlBoxStyle.Mac:
                    _controlboxDefaultTemplate = TryFindResource("internal_controlboxMac") as DataTemplate;
                    break;
                case ControlBoxStyle.Windows:
                default:
                    _controlboxDefaultTemplate = TryFindResource("internal_controlboxWindows") as DataTemplate;
                    break;
            }

            _setControlBox();
        }

        void _setControlBox()
        {
            if (_controlBox == null) return;

            if (ControlBoxTemplate != null)
            {
                _controlBox.ContentTemplate = ControlBoxTemplate;
            }
            else
            {
                _controlBox.ContentTemplate = _controlboxDefaultTemplate;
            }
        }

        void _setFooter()
        {
            if (_footer == null) return;

            _footer.ContentTemplate = FooterTemplate;
        }
        void _setHeader()
        {
            if (_header == null) return;

            if (HeaderTemplate != null)
            {
                _header.ContentTemplate = HeaderTemplate;
            }
            else
            {
                //Setting the foreground from xaml, results in parser exception which in returns delays the loading process.
                _header.ContentTemplate = _headerDefaultTemplate;
                var _foregroundBinding = new Binding();
                _foregroundBinding.Source = this;
                _foregroundBinding.Path = new PropertyPath(Models.Icon.DefaultColorProperty);
                _header.SetBinding(ContentControl.ForegroundProperty, _foregroundBinding);
            }
        }
       
    }
}
