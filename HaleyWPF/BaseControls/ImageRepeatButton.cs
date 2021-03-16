using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
{
    public class ImageRepeatButton : PlainRepeatButton, IImageHolder
    {
        #region Constructors
        static ImageRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageRepeatButton), new FrameworkPropertyMetadata(typeof(ImageRepeatButton)));
        }

        public ImageRepeatButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

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

        #region Images
        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register(nameof(DefaultImage), typeof(ImageSource), typeof(ImageRepeatButton), new FrameworkPropertyMetadata(null));

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(nameof(HoverImage), typeof(ImageSource), typeof(ImageRepeatButton), new FrameworkPropertyMetadata(null));

        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register(nameof(PressedImage), typeof(ImageSource), typeof(ImageRepeatButton), new FrameworkPropertyMetadata(null));

        #endregion

        #region ImageColors
        public SolidColorBrush DefaultImageColor
        {
            get { return (SolidColorBrush)GetValue(DefaultImageColorProperty); }
            set { SetValue(DefaultImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageColorProperty =
            DependencyProperty.Register(nameof(DefaultImageColor), typeof(SolidColorBrush), typeof(ImageRepeatButton), new FrameworkPropertyMetadata(_defaultColorChanged));

        public SolidColorBrush HoverImageColor
        {
            get { return (SolidColorBrush)GetValue(HoverImageColorProperty); }
            set { SetValue(HoverImageColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HoverImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageColorProperty =
            DependencyProperty.Register(nameof(HoverImageColor), typeof(SolidColorBrush), typeof(ImageRepeatButton), new FrameworkPropertyMetadata(_hoverColorChanged));

        public SolidColorBrush PressedImageColor
        {
            get { return (SolidColorBrush)GetValue(PressedImageColorProperty); }
            set { SetValue(PressedImageColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PressedImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageColorProperty =
            DependencyProperty.Register(nameof(PressedImageColor), typeof(SolidColorBrush), typeof(ImageRepeatButton), new FrameworkPropertyMetadata(_pressedColorChanged));
        #endregion

        public bool DisableColorChange
        {
            get { return (bool)GetValue(DisableColorChangeProperty); }
            set { SetValue(DisableColorChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisableColorChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisableColorChangeProperty =
            DependencyProperty.Register(nameof(DisableColorChange), typeof(bool), typeof(ImageRepeatButton), new PropertyMetadata(false));

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
    }
}
