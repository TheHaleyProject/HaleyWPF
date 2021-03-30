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

namespace Haley.Models
{
    public class Icon : Control
    {
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
            { SetDefault(sender, ResourceHelper.getIcon(IconEnums.empty_image.ToString())); }

            if (GetHover(sender) == null) SetHover(sender, GetDefault(sender));
            if (GetPressed(sender) == null) SetPressed(sender, GetHover(sender));

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

        // Using a DependencyProperty as the backing store for Default.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for Hover.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for Pressed.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for DefaultColor.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for HoverColor.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for PressedColor.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for DisableColorChange.  This enables animation, styling, binding, etc...
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
