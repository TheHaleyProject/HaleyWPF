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
    public class PlainRepeatButton : RepeatButton, IShadow, ICornerRadius, IHoverBase, IImageHolder
    {
        #region Constructors
        static PlainRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainRepeatButton), new FrameworkPropertyMetadata(typeof(PlainRepeatButton)));
        }

        public PlainRepeatButton()
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
            var imgRepeat = d as PlainRepeatButton;
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
            DependencyProperty.Register(nameof(ImageLocation), typeof(Dock), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(Dock.Top,propertyChangedCallback:ImagLocationPropertyChanged));

        #region Images
        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register(nameof(DefaultImage), typeof(ImageSource), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(nameof(HoverImage), typeof(ImageSource), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register(nameof(PressedImage), typeof(ImageSource), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        #endregion

        #region ImageColors
        public SolidColorBrush DefaultImageColor
        {
            get { return (SolidColorBrush)GetValue(DefaultImageColorProperty); }
            set { SetValue(DefaultImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageColorProperty =
            DependencyProperty.Register(nameof(DefaultImageColor), typeof(SolidColorBrush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(_defaultColorChanged));

        public SolidColorBrush HoverImageColor
        {
            get { return (SolidColorBrush)GetValue(HoverImageColorProperty); }
            set { SetValue(HoverImageColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HoverImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageColorProperty =
            DependencyProperty.Register(nameof(HoverImageColor), typeof(SolidColorBrush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(_hoverColorChanged));

        public SolidColorBrush PressedImageColor
        {
            get { return (SolidColorBrush)GetValue(PressedImageColorProperty); }
            set { SetValue(PressedImageColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PressedImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageColorProperty =
            DependencyProperty.Register(nameof(PressedImageColor), typeof(SolidColorBrush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(_pressedColorChanged));
        #endregion

        public bool DisableColorChange
        {
            get { return (bool)GetValue(DisableColorChangeProperty); }
            set { SetValue(DisableColorChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisableColorChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisableColorChangeProperty =
            DependencyProperty.Register(nameof(DisableColorChange), typeof(bool), typeof(PlainRepeatButton), new PropertyMetadata(false));

        #region Private Methods
        private static void _hoverColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _disable = (bool)d.GetValue(DisableColorChangeProperty);
            if (_disable) return;
            ImageHelper.changeColor(nameof(HoverImageColor), d, e);
        }
        private static void _pressedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _disable = (bool)d.GetValue(DisableColorChangeProperty);
            if (_disable) return;
            ImageHelper.changeColor(nameof(PressedImageColor), d, e);
        }
        private static void _defaultColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _disable = (bool)d.GetValue(DisableColorChangeProperty);
            if (_disable) return;
            ImageHelper.changeColor(nameof(DefaultImage), d, e);
        }

        #endregion

        #region Corner Radius
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));
        #endregion

        #region Hover
        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(nameof(HoverBackground), typeof(Brush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public Brush HoverBorderBrush
        {
            get { return (Brush)GetValue(HoverBorderBrushProperty); }
            set { SetValue(HoverBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBorderBrushProperty =
            DependencyProperty.Register(nameof(HoverBorderBrush), typeof(Brush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public Thickness HoverBorderThickness
        {
            get { return (Thickness)GetValue(HoverBorderThicknessProperty); }
            set { SetValue(HoverBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBorderThicknessProperty =
            DependencyProperty.Register(nameof(HoverBorderThickness), typeof(Thickness), typeof(PlainRepeatButton), new PropertyMetadata(ResourceHelper.borderThickness));
        #endregion

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(PlainRepeatButton), new PropertyMetadata(null));

        #region SHADOW
        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(false));

        public Brush ShadowColor
        {
            get { return (Brush)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Brush), typeof(PlainRepeatButton), new FrameworkPropertyMetadata(null));

        public bool ShadowOnlyOnMouseOver
        {
            get { return (bool)GetValue(ShadowOnlyOnMouseOverProperty); }
            set { SetValue(ShadowOnlyOnMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowOnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowOnlyOnMouseOverProperty =
            DependencyProperty.Register(nameof(ShadowOnlyOnMouseOver), typeof(bool), typeof(PlainRepeatButton), new PropertyMetadata(true));
        #endregion
    }
}
