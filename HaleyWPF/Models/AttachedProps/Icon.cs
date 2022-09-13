using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Haley.WPF;
using Haley.Events;
using System.Windows.Input;
using Haley.Utils;
using System.Windows.Media.Imaging;

namespace Haley.Models
{
    public class Icon : Control
    {
        public static ImageSource EmptyImage = ResourceHelper.getIcon(IconEnums.empty_image.ToString());
        public Icon() 
        { 
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Initiate, Execute_Initiate));
        }

        void Execute_Initiate(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is DependencyObject sender_do)
            {
                InitiateImages(sender_do);
            }
        }

        public static void InitiateImages(DependencyObject sender)
        {
            //Process Images
            if (GetDefault(sender) == null)
            { SetDefault(sender, EmptyImage); }

            if (GetHover(sender) == null) SetHover(sender, GetDefault(sender));
            if (GetPressed(sender) == null ) SetPressed(sender, GetDefault(sender));

            //Process Image Colors
            if (!GetDisableColorChange(sender))
            {
                if (GetDefaultColor(sender) != null)
                { SetDefault(sender,ImageHelper.changeColor(GetDefault(sender), GetDefaultColor(sender))); }

                if (GetHoverColor(sender) != null)
                    { SetHover(sender, ImageHelper.changeColor(GetHover(sender), GetHoverColor(sender))); }

                if (GetPressedColor(sender) != null)
                { SetPressed(sender, ImageHelper.changeColor(GetPressed(sender), GetPressedColor(sender))); }
            }
        }

        public static void ReInitiateImages(DependencyObject sender, bool inheritfromDefault = false) {
            //When there is no image, we get null, but when we have set some image, it is then cached. So, subsequent calls will return cached bitmap. This method of initiate image itself is called only when we try to initiate for the first time. So, if deliberately we initiate for second time, we should have already changed the image. So, we check if it is cached, then we reset that as well.
            var defImage = GetDefault(sender);
            //Process Images
            if (defImage == null || defImage is CachedBitmap) { SetDefault(sender, EmptyImage); }

            var hoverImage = GetHover(sender);
            if (hoverImage == null || hoverImage == EmptyImage || hoverImage is CachedBitmap || inheritfromDefault) SetHover(sender, GetDefault(sender));

            var pressedImage = GetPressed(sender);
            if (pressedImage == null || pressedImage == EmptyImage || pressedImage is CachedBitmap || inheritfromDefault) SetPressed(sender, GetDefault(sender));

            //Process Image Colors
            if (!GetDisableColorChange(sender))
            {
                if (GetDefaultColor(sender) != null)
                { SetDefault(sender,ImageHelper.changeColor(GetDefault(sender), GetDefaultColor(sender))); }

                if (GetHoverColor(sender) != null)
                    { SetHover(sender, ImageHelper.changeColor(GetHover(sender), GetHoverColor(sender))); }

                if (GetPressedColor(sender) != null)
                { SetPressed(sender, ImageHelper.changeColor(GetPressed(sender), GetPressedColor(sender))); }
            }
        }



        #region Images

        public static ImageSource GetDefault(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(DefaultProperty);
        }

        public static void SetDefault(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(DefaultProperty, value);
        }

        public static readonly DependencyProperty DefaultProperty =
            DependencyProperty.RegisterAttached("Default", typeof(ImageSource), typeof(Icon), new PropertyMetadata(null));

        public static ImageSource GetHover(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(HoverProperty);
        }

        public static void SetHover(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(HoverProperty, value);
        }

        public static readonly DependencyProperty HoverProperty =
            DependencyProperty.RegisterAttached("Hover", typeof(ImageSource), typeof(Icon), new PropertyMetadata(null));

        public static ImageSource GetPressed(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(PressedProperty);
        }

        public static void SetPressed(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(PressedProperty, value);
        }

        public static readonly DependencyProperty PressedProperty =
            DependencyProperty.RegisterAttached("Pressed", typeof(ImageSource), typeof(Icon), new PropertyMetadata(null));

        #endregion

        #region ImageColors

        public static SolidColorBrush GetDefaultColor(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(DefaultColorProperty);
        }

        public static void SetDefaultColor(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(DefaultColorProperty, value);
        }

        public static readonly DependencyProperty DefaultColorProperty =
            DependencyProperty.RegisterAttached("DefaultColor", typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata(_defaultColorChanged));

        public static SolidColorBrush GetHoverColor(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(HoverColorProperty);
        }

        public static void SetHoverColor(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(HoverColorProperty, value);
        }

        public static readonly DependencyProperty HoverColorProperty =
            DependencyProperty.RegisterAttached("HoverColor", typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata(_hoverColorChanged));

        public static SolidColorBrush GetPressedColor(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(PressedColorProperty);
        }

        public static void SetPressedColor(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(PressedColorProperty, value);
        }

        public static readonly DependencyProperty PressedColorProperty =
            DependencyProperty.RegisterAttached("PressedColor", typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata(_pressedColorChanged));

        #endregion

        public static bool GetDisableColorChange(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableColorChangeProperty);
        }

        public static void SetDisableColorChange(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableColorChangeProperty, value);
        }

        public static readonly DependencyProperty DisableColorChangeProperty =
            DependencyProperty.RegisterAttached("DisableColorChange", typeof(bool), typeof(Icon), new PropertyMetadata(false));

        #region Private Methods
        private static void _hoverColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _disable = (bool)d.GetValue(DisableColorChangeProperty);
            if (_disable) return;
            ImageHelper.changeColor(HoverProperty, d, e);
        }
        private static void _pressedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _disable = (bool)d.GetValue(DisableColorChangeProperty);
            if (_disable) return;
            ImageHelper.changeColor(PressedProperty, d, e);
        }
        private static void _defaultColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _disable = (bool)d.GetValue(DisableColorChangeProperty);
            if (_disable) return;
            ImageHelper.changeColor(DefaultProperty, d, e);
        }
        
        #endregion
    }
}
