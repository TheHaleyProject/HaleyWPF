using Haley.Abstractions;
using Haley.Enums;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using SysCtrls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Shell;

namespace Haley.WPF.Controls
{
    public class PlainWindow : Window, ICornerRadius
    {

        private const string UIEHeaderHolder = "PART_header_holder";
        private const string UIEFooterHolder = "PART_footer_holder";

        private const string UIEHeader = "PART_header";
        private const string UIEFooter = "PART_footer";
        private const string UIEMinimizeBtn = "PART_btn_minimize";
        private const string UIEMaximizeBtn = "PART_btn_maximize";
        private const string UIECloseBtn = "PART_btn_close";
        private const string UIEControlBox = "grdControlBox";

        private Border _headerHolder;
        private Border _footerHolder;

        private ContentControl _header;
        private ContentControl _footer;
        private Control _btnMaximize;
        private Control _btnMinimize;
        private Control _btnClose;
        private Grid _controlGrid;

        private DataTemplate _headerDefaultTemplate;

        static PlainWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainWindow), new FrameworkPropertyMetadata(typeof(PlainWindow)));
        }


        public bool HideMinimizeButton { get; set; }
        public bool HideMaximizeButton { get; set; }
        public bool HideCloseButton { get; set; }
        public bool HideFooter { get; set; }
        public bool HideHeader { get; set; }

        public PlainWindow() 
        {
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            WindowChrome.SetWindowChrome(this, new WindowChrome() {ResizeBorderThickness=new Thickness(4.0),GlassFrameThickness = new Thickness(2.0)}); //for enabling resize
            //set border thickness to match the resize border
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ExecuteAction, _controlboxAction));

            ////This is to limit the maximum height of the screen.
            //MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerHolder = GetTemplateChild(UIEHeaderHolder) as Border;
            _footerHolder = GetTemplateChild(UIEFooterHolder) as Border;

            _header = GetTemplateChild(UIEHeader) as ContentControl;
            _footer = GetTemplateChild(UIEFooter) as ContentControl;
            _btnMaximize = GetTemplateChild(UIEMaximizeBtn) as Control;
            _btnMinimize = GetTemplateChild(UIEMinimizeBtn) as Control;
            _btnClose = GetTemplateChild(UIECloseBtn) as Control;
            _controlGrid = GetTemplateChild(UIEControlBox) as Grid;
            WindowChrome.SetIsHitTestVisibleInChrome(_controlGrid, true); //Important or else the control box items hit will not be visible.
            _initiate();
        }

        void _initiate()
        {
            if (HideCloseButton) _btnClose.Visibility = Visibility.Collapsed;
            if (HideMaximizeButton) _btnMaximize.Visibility = Visibility.Collapsed;
            if (HideMinimizeButton) _btnMinimize.Visibility = Visibility.Collapsed;
            if (HideFooter) _footerHolder.Visibility = Visibility.Collapsed;
            if (HideHeader) _headerHolder.Visibility = Visibility.Collapsed;

            _headerDefaultTemplate = TryFindResource("defaultHeaderTemplate") as DataTemplate;
            _changeFooter();
            _changeHeader();
            _setWindowCornerRadius(this.CornerRadius);
        }

        public bool HideIcon
        {
            get { return (bool)GetValue(HideIconProperty); }
            set { SetValue(HideIconProperty, value); }
        }

        public static readonly DependencyProperty HideIconProperty =
            DependencyProperty.Register(nameof(HideIcon), typeof(bool), typeof(PlainWindow), new PropertyMetadata(false));

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
            if (_actual < 15.0) return 15.0;
            return baseValue;
        }

        public double FooterHeight
        {
            get { return (double)GetValue(FooterHeightProperty); }
            set { SetValue(FooterHeightProperty, value); }
        }

        public static readonly DependencyProperty FooterHeightProperty =
            DependencyProperty.Register("FooterHeight", typeof(double), typeof(PlainWindow), new FrameworkPropertyMetadata(25.0,null,coerceValueCallback: _headerfooterHeightCoerce));

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
            if (_headerHolder != null)
            {
                _headerHolder.CornerRadius = new CornerRadius(_radius.TopLeft, _radius.TopRight, 0.0, 0.0);
            }

            if (!HideFooter && _footerHolder != null)
            {
                _footerHolder.CornerRadius = new CornerRadius(0.0, 0.0, _radius.BottomRight, _radius.BottomLeft);
            }

            if (HideFooter)
            {
                SetCurrentValue(FooterHeightProperty, 4.0);
            }
        }

        void _controlboxAction(object sender, ExecutedRoutedEventArgs e)
        {
            string _param = e.Parameter as string;
            if (_param == null) return;
            switch(_param)
            {
                case "Min":
                    this.WindowState = WindowState.Minimized;
                    break;
                case "Max":
                    switch (this.WindowState)
                    {
                        case WindowState.Maximized:
                            this.WindowState = WindowState.Normal;
                            break;
                        case WindowState.Normal:
                            this.WindowState = WindowState.Maximized;
                            break;
                    }
                    break;
                case "Close":
                    this.Close();
                    break;
                case "DragMove":
                    this.DragMove();
                    break;
            }
        }
        static void HeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainWindow pw)
            {
                pw._changeHeader();
            }
        }
        static void FooterTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if (d is PlainWindow pw)
            {
                pw._changeFooter();
            }
        }

        void _changeFooter()
        {
            if (_footer == null) return;

            _footer.ContentTemplate = FooterTemplate;
        }
        void _changeHeader()
        {
            if (_header == null) return;

            if (HeaderTemplate != null)
            {
                _header.ContentTemplate = HeaderTemplate;
            }
            else
            {
                _header.ContentTemplate = _headerDefaultTemplate;
            }
        }
    }
}
