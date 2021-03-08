using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.Abstractions;
using Haley.Utils;
using Haley.Models;
using Haley.Enums;

namespace Haley.WPF.BaseControls
{
    public class PlainImage : Control , IImageHolder
    {
        #region Constructors
        static PlainImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainImage), new FrameworkPropertyMetadata(typeof(PlainImage)));
        }

        public PlainImage()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //DEFAULT IMAGE
            if (DefaultImage == null)
            { DefaultImage = ResourceStore.getIcon(IconEnums.empty_image.ToString()); }
            if (DefaultImageColor != null)
            { DefaultImage = InternalHelper.changeColor(DefaultImage, DefaultImageColor); }

            //HOVER IMAGE
            if (HoverImage == null) HoverImage = DefaultImage;
            if (HoverImageColor != null)
            { HoverImage = InternalHelper.changeColor(HoverImage, HoverImageColor); }

        }

        #region Images
        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register(nameof(DefaultImage), typeof(ImageSource), typeof(PlainImage), new FrameworkPropertyMetadata(null));

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(nameof(HoverImage), typeof(ImageSource), typeof(PlainImage), new FrameworkPropertyMetadata(null));
        #endregion

        #region ImageColors
        public SolidColorBrush DefaultImageColor
        {
            get { return (SolidColorBrush)GetValue(DefaultImageColorProperty); }
            set { SetValue(DefaultImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageColorProperty =
            DependencyProperty.Register(nameof(DefaultImageColor), typeof(SolidColorBrush), typeof(PlainImage), new FrameworkPropertyMetadata(_defaultColorChanged));

        private static void _defaultColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InternalHelper.changeColor(nameof(DefaultImage), d, e);
        }

        public SolidColorBrush HoverImageColor
        {
            get { return (SolidColorBrush)GetValue(HoverImageColorProperty); }
            set { SetValue(HoverImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageColorProperty =
            DependencyProperty.Register(nameof(HoverImageColor), typeof(SolidColorBrush), typeof(PlainImage), new FrameworkPropertyMetadata(_hoverColorChanged));

        private static void _hoverColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InternalHelper.changeColor(nameof(HoverImageColor), d, e);
        }

        #endregion
    }
}
