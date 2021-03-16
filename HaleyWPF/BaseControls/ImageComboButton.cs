using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Haley.WPF.BaseControls
{
    public class ImageComboButton : ImageButton
    {
        #region Constructors
        static ImageComboButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageComboButton), new FrameworkPropertyMetadata(typeof(ImageComboButton)));
        }

        public ImageComboButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
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

        static void ImagLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imgCmbBtn = d as ImageComboButton;
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
            DependencyProperty.Register(nameof(ImageLocation), typeof(Dock), typeof(ImageComboButton), new FrameworkPropertyMetadata(Dock.Top,propertyChangedCallback:ImagLocationPropertyChanged));
    }
}
