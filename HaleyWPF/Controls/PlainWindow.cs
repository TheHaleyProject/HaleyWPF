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

        private const string UIEHeaderBorder = "PART_header_holder";
        private const string UIEFooterBorder = "PART_footer_holder";
        private const string UIEHeader = "PART_header";
        private const string UIEFooter = "PART_footer";
        private const string UIEMinimizeBtn = "PART_btn_minimize";
        private const string UIEMaximizeBtn = "PART_btn_maximize";
        private const string UIECloseBtn = "PART_btn_close";
        private const string UIEControlBox = "grdControlBox";

        private Border _headerBorder;
        private Border _footerBorder;
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
            _headerBorder = GetTemplateChild(UIEHeaderBorder) as Border;
            _footerBorder = GetTemplateChild(UIEFooterBorder) as Border;
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
            _headerDefaultTemplate = TryFindResource("defaultHeaderTemplate") as DataTemplate;
            _setHeaderFooterCorners(this.CornerRadius);
            _changeFooter();
            _changeHeader();
            if (HideCloseButton) _btnClose.Visibility = Visibility.Collapsed;
            if (HideMaximizeButton) _btnMaximize.Visibility = Visibility.Collapsed;
            if (HideMinimizeButton)_btnMinimize.Visibility = Visibility.Collapsed;
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(PlainWindow), new PropertyMetadata(null,propertyChangedCallback:HeaderTemplateChanged));

        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FooterTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterTemplateProperty =
            DependencyProperty.Register(nameof(FooterTemplate), typeof(DataTemplate), typeof(PlainWindow), new PropertyMetadata(null,propertyChangedCallback:FooterTemplateChanged));

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderHeight.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for FooterHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterHeightProperty =
            DependencyProperty.Register("FooterHeight", typeof(double), typeof(PlainWindow), new FrameworkPropertyMetadata(25.0,null,coerceValueCallback: _headerfooterHeightCoerce));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainWindow), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius,propertyChangedCallback:_cornerRadiusChanged));

        static void _cornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainWindow pw)
            {
                pw._setHeaderFooterCorners((CornerRadius)e.NewValue);
            }
        }
        private void _setHeaderFooterCorners(CornerRadius _radius)
        {
            if (_headerBorder != null)
            {
                _headerBorder.CornerRadius = new CornerRadius(_radius.TopLeft, _radius.TopRight, 0.0, 0.0);
            }

            if (_footerBorder != null)
            {
                _footerBorder.CornerRadius = new CornerRadius(0.0, 0.0, _radius.BottomRight, _radius.BottomLeft);
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
