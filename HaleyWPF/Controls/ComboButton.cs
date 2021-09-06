using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Haley.Models;

namespace Haley.WPF.Controls
{
    public class ComboButton : PlainButton
    {
        #region Constructors
        static ComboButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboButton), new FrameworkPropertyMetadata(typeof(ComboButton)));
        }

        public ComboButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Icon.InitiateImages(this);
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

        static void ImagLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imgCmbBtn = d as ComboButton;
            if (imgCmbBtn != null)
            {
                imgCmbBtn._rearrangeDock();
            }
        }

        public Dock ImageLocation
        {
            get { return (Dock)GetValue(ImageLocationProperty); }
            set { SetValue(ImageLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageLocationProperty =
            DependencyProperty.Register(nameof(ImageLocation), typeof(Dock), typeof(ComboButton), new FrameworkPropertyMetadata(Dock.Top, propertyChangedCallback: ImagLocationPropertyChanged));

        public bool HideContent
        {
            get { return (bool)GetValue(HideContentProperty); }
            set { SetValue(HideContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideContentProperty =
            DependencyProperty.Register(nameof(HideContent), typeof(bool), typeof(ComboButton), new PropertyMetadata(false));
    }
}
