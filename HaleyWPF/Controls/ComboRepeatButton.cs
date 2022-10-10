using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Haley.Models;

namespace Haley.WPF.Controls
{
    public class ComboRepeatButton : ImageRepeatButton
    {
        #region Constructors
        static ComboRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboRepeatButton), new FrameworkPropertyMetadata(typeof(ComboRepeatButton)));
        }

        public ComboRepeatButton()
        {
        }
        #endregion

        #region Initiation
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Icon.InitiateImages(this);
            //Only in case of top or bottom, re arrange the dock children
            _rearrangeDock();
        }

        private void _rearrangeDock()
        {
            //Try to get the template.
            var _dock = GetTemplateChild("PART_maindock") as DockPanel;
            var _imagebox = GetTemplateChild("PART_ImageViewBox") as Viewbox;
            var _cntPrsntr = GetTemplateChild("PART_ContentHolder") as ContentPresenter;

            if (_dock == null) return;
            if (ImageLocation == Dock.Top || ImageLocation == Dock.Bottom)
            {
                //In this case, Textbox holder should be the first child
                _dock.Children.Clear();
                _cntPrsntr.SetValue(DockPanel.DockProperty, ImageLocation == Dock.Top ? Dock.Bottom : Dock.Top); //Since we are using ImageLocation to specify Text location, we need to invert it.
                _dock.Children.Add(_cntPrsntr);
                _dock.Children.Add(_imagebox);
            }
            else
            {
                //In this case, Textbox holder should be the first child
                _dock.Children.Clear();
                _imagebox.SetValue(DockPanel.DockProperty, ImageLocation);
                _dock.Children.Add(_imagebox);
                _dock.Children.Add(_cntPrsntr);
            }
        }
        #endregion

        static void ImagLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imgRepeat = d as ComboRepeatButton;
            if (imgRepeat != null)
            {
                imgRepeat._rearrangeDock();
            }
        }

        public Dock ImageLocation
        {
            get { return (Dock)GetValue(ImageLocationProperty); }
            set { SetValue(ImageLocationProperty, value); }
        }

        public static readonly DependencyProperty ImageLocationProperty =
            DependencyProperty.Register(nameof(ImageLocation), typeof(Dock), typeof(ComboRepeatButton), new FrameworkPropertyMetadata(Dock.Top, propertyChangedCallback: ImagLocationPropertyChanged));

        public bool HideContent
        {
            get { return (bool)GetValue(HideContentProperty); }
            set { SetValue(HideContentProperty, value); }
        }

        public static readonly DependencyProperty HideContentProperty =
            DependencyProperty.Register(nameof(HideContent), typeof(bool), typeof(ComboRepeatButton), new PropertyMetadata(false));
    }
}
