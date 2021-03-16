using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Haley.WPF.BaseControls
{
    public class ImageComboRepeatButton : ImageRepeatButton
    {
        #region Constructors
        static ImageComboRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageComboRepeatButton), new FrameworkPropertyMetadata(typeof(ImageComboRepeatButton)));
        }

        public ImageComboRepeatButton()
        {

        }
        #endregion

        #region Initiation
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _processImages();
            //Only in case of top or bottom, re arrange the dock children
            _rearrangeDock();
        }

        private void _processImages()
        {
            //Process Images
            if (DefaultImage == null)
            { DefaultImage = ResourceHelper.getIcon(IconEnums.empty_image.ToString()); }
            if (HoverImage == null) HoverImage = DefaultImage;
            if (PressedImage == null) PressedImage = HoverImage;

            if (!DisableColorChange)
            {
                //Process Image Colors
                if (DefaultImageColor != null)
                { DefaultImage = ImageHelper.changeColor(DefaultImage, DefaultImageColor); }

                if (HoverImageColor != null)
                { HoverImage = ImageHelper.changeColor(HoverImage, HoverImageColor); }

                if (PressedImageColor != null)
                { PressedImage = ImageHelper.changeColor(PressedImage, PressedImageColor); }
            }
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
            var imgRepeat = d as ImageComboRepeatButton;
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

        // Using a DependencyProperty as the backing store for ImageLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageLocationProperty =
            DependencyProperty.Register(nameof(ImageLocation), typeof(Dock), typeof(ImageComboRepeatButton), new FrameworkPropertyMetadata(Dock.Top,propertyChangedCallback:ImagLocationPropertyChanged));
    }
}
