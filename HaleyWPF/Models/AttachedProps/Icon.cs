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
using System.Collections.Generic;

namespace Haley.Models
{
    public class Icon : Control
    {
        #region Attributes
        private const string DEFAULT = "Default";
        private const string DEFAULT_COLOR = "DefaultColor";
        private const string DEFAULT_KIND = "DefaultKind";
        private const string HOVER = "Hover";
        private const string HOVER_COLOR = "HoverColor";
        private const string HOVER_KIND = "HoverKind";
        private const string PRESSED = "Pressed";
        private const string PRESSED_COLOR = "PressedColor";
        private const string PRESSED_KIND = "PressedKind";
        #endregion

        #region Attached Properties - Image Color
        public static SolidColorBrush GetDefaultColor(DependencyObject obj) {
            return (SolidColorBrush)obj.GetValue(DefaultColorProperty);
        }
        public static void SetDefaultColor(DependencyObject obj, SolidColorBrush value) {
            obj.SetValue(DefaultColorProperty, value);
        }

        public static readonly DependencyProperty DefaultColorProperty = DependencyProperty.RegisterAttached(DEFAULT_COLOR, typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ProcessColorChangeSingle(d, e, DEFAULT_COLOR)));

        public static bool GetDisableColorChange(DependencyObject obj) {
            return (bool)obj.GetValue(DisableColorChangeProperty);
        }
        public static void SetDisableColorChange(DependencyObject obj, bool value) {
            obj.SetValue(DisableColorChangeProperty, value);
        }

        public static readonly DependencyProperty DisableColorChangeProperty = DependencyProperty.RegisterAttached("DisableColorChange", typeof(bool), typeof(Icon), new PropertyMetadata(false));

        public static SolidColorBrush GetHoverColor(DependencyObject obj) {
            return (SolidColorBrush)obj.GetValue(HoverColorProperty);
        }
        public static void SetHoverColor(DependencyObject obj, SolidColorBrush value) {
            obj.SetValue(HoverColorProperty, value);
        }

        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.RegisterAttached(HOVER_COLOR, typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ProcessColorChangeSingle(d, e, HOVER_COLOR)));

        public static SolidColorBrush GetPressedColor(DependencyObject obj) {
            return (SolidColorBrush)obj.GetValue(PressedColorProperty);
        }
        public static void SetPressedColor(DependencyObject obj, SolidColorBrush value) {
            obj.SetValue(PressedColorProperty, value);
        }

        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.RegisterAttached(PRESSED_COLOR, typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ProcessColorChangeSingle(d, e, PRESSED_COLOR)));
        #endregion

        #region Attached Properties - General

        public static double GetRotateAngle(DependencyObject obj) {
            return (double)obj.GetValue(RotateAngleProperty);
        }

        public static void SetRotateAngle(DependencyObject obj, double value) {
            obj.SetValue(RotateAngleProperty, value);
        }

        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.RegisterAttached("RotateAngle", typeof(double), typeof(Icon), new PropertyMetadata(0.0));

        private static bool GetChangeInProgress(DependencyObject obj) {
            return (bool)obj.GetValue(ChangeInProgressProperty);
        }
        private static void SetChangeInProgress(DependencyObject obj, bool value) {
            obj.SetValue(ChangeInProgressProperty, value);
        }
        private static readonly DependencyProperty ChangeInProgressProperty = DependencyProperty.RegisterAttached("ChangeInProgress", typeof(bool), typeof(Icon), new PropertyMetadata(false));
        public static IconSourcePreference GetPreference(DependencyObject obj) {
            return (IconSourcePreference)obj.GetValue(PreferenceProperty);
        }
        public static void SetPreference(DependencyObject obj, IconSourcePreference value) {
            obj.SetValue(PreferenceProperty, value);
        }

        public static readonly DependencyProperty PreferenceProperty = DependencyProperty.RegisterAttached("Preference", typeof(IconSourcePreference), typeof(Icon), new FrameworkPropertyMetadata(IconSourcePreference.IconKind));

        private static bool GetIsInitialized(DependencyObject obj) {
            return (bool)obj.GetValue(IsInitializedProperty);
        }
        private static void SetIsInitialized(DependencyObject obj, bool value) {
            obj.SetValue(IsInitializedProperty, value);
        }
        private static readonly DependencyProperty IsInitializedProperty =
            DependencyProperty.RegisterAttached("IsInitialized", typeof(bool), typeof(Icon), new PropertyMetadata(false));

        #endregion

        #region Attached Properties - Image Sources
        public static ImageSource GetDefault(DependencyObject obj) {
            return (ImageSource)obj.GetValue(DefaultProperty);
        }
        public static void SetDefault(DependencyObject obj, ImageSource value) {
            obj.SetValue(DefaultProperty, value);
        }
        public static readonly DependencyProperty DefaultProperty = DependencyProperty.RegisterAttached(DEFAULT, typeof(ImageSource), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, DEFAULT)));

        public static IconKind? GetDefaultKind(DependencyObject obj) {
            return (IconKind?)obj.GetValue(DefaultKindProperty);
        }
        public static void SetDefaultKind(DependencyObject obj, IconKind? value) {
            obj.SetValue(DefaultKindProperty, value);
        }

        public static readonly DependencyProperty DefaultKindProperty =
          DependencyProperty.RegisterAttached(DEFAULT_KIND, typeof(IconKind?), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, DEFAULT_KIND)));
       
        public static IconKind? GetHoverKind(DependencyObject obj) {
            return (IconKind?)obj.GetValue(HoverKindProperty);
        }
        public static void SetHoverKind(DependencyObject obj, IconKind? value) {
            obj.SetValue(HoverKindProperty, value);
        }

        public static readonly DependencyProperty HoverKindProperty = DependencyProperty.RegisterAttached(HOVER_KIND, typeof(IconKind?), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, HOVER_KIND)));

        public static ImageSource GetHover(DependencyObject obj) {
            return (ImageSource)obj.GetValue(HoverProperty);
        }
        public static void SetHover(DependencyObject obj, ImageSource value) {
            obj.SetValue(HoverProperty, value);
        }

        public static readonly DependencyProperty HoverProperty = DependencyProperty.RegisterAttached(HOVER, typeof(ImageSource), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, HOVER)));

        public static IconKind? GetPressedKind(DependencyObject obj) {
            return (IconKind?)obj.GetValue(PressedKindProperty);
        }
        public static void SetPressedKind(DependencyObject obj, IconKind? value) {
            obj.SetValue(PressedKindProperty, value);
        }
        //Reinitiating when preference changes resets all the cache. Think of a better solution before using reinititate
        public static readonly DependencyProperty PressedKindProperty = DependencyProperty.RegisterAttached(PRESSED_KIND, typeof(IconKind?), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, PRESSED_KIND)));

        public static ImageSource GetPressed(DependencyObject obj) {
            return (ImageSource)obj.GetValue(PressedProperty);
        }
        public static void SetPressed(DependencyObject obj, ImageSource value) {
            obj.SetValue(PressedProperty, value);
        }

        public static readonly DependencyProperty PressedProperty = DependencyProperty.RegisterAttached(PRESSED, typeof(ImageSource), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, PRESSED)));

        #endregion

        public static ImageSource EmptyImage = ResourceHelper.getIcon(IconKind.empty_image.ToString());
     
        public Icon() {
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Initiate, Execute_Initiate));
        }

        public static void InitiateImages(DependencyObject sender, bool resetCaches = false, string[] affected_props = null) {
            //When we try to access any property before they are set, we end up with initiating the control (applying OnApplyTemplate) and finally end up with calling InitiateImages.
            try {
                SetIsInitialized(sender, true); //On first call (probably by the constructor of the control/frameworkelement) set the status to true. //On first call, lets mark it as initialized (whether it is successfull or not.
                //ONLY HANDLERS INITIATE THE IMAGES
                SetChangeInProgress(sender, true); //to avoid 

                if (GetPreference(sender) is IconSourcePreference.IconKind) {

                    //WHEN WE USE ICON KIND, WE ARE OVERRIDDING WHATEVER VALUE THE USER HAS PROVIDED IN THE FIRST PLACE.
                    //If it is iconkind, we will set new images, and we don't have to worry about caches.

                    //Only when we have iconkind preference, override the default images here.
                    if (GetDefaultKind(sender).HasValue) {
                        SetDefault(sender, ResourceHelper.getIcon(GetDefaultKind(sender).Value.ToString()));
                    }

                    //Only when we have iconkind preference, override the default images here.
                    if (GetHoverKind(sender).HasValue) {
                        SetHover(sender, ResourceHelper.getIcon(GetHoverKind(sender).Value.ToString()));
                    }

                    //Only when we have iconkind preference, override the default images here.
                    if (GetPressedKind(sender).HasValue) {
                        SetPressed(sender, ResourceHelper.getIcon(GetPressedKind(sender).Value.ToString()));
                    }
                }

                Func<string, bool> validator = (s) => {
                    return (resetCaches && affected_props != null && affected_props.Any(p => p.Equals(s)));
                };

                var defImage = GetDefault(sender);
                //Process Images
                if (defImage == null || (validator(DEFAULT) && defImage is CachedBitmap)) {
                    SetDefault(sender, EmptyImage);
                }

                var hoverImage = GetHover(sender);
                if (hoverImage == null || (validator(HOVER) && (hoverImage is CachedBitmap || hoverImage == EmptyImage))) {
                    SetHover(sender, GetDefault(sender));
                }

                var pressedImage = GetPressed(sender);
                if (pressedImage == null || (validator(PRESSED) && (pressedImage is CachedBitmap || pressedImage == EmptyImage))) {
                    SetPressed(sender, GetDefault(sender));
                }
                ProcessColorChange(sender);
            } finally {
                SetChangeInProgress(sender, false);
            }
        }

        public static void ReInitiateImages(DependencyObject sender, bool resetCaches = true, string[] affected_props = null) {
            //When there is no image, we get null, but when we have set some image, it is then cached. So, subsequent calls will return cached bitmap. This method of initiate image itself is called only when we try to initiate for the first time. So, if deliberately we initiate for second time, we should have already changed the image. So, we check if it is cached, then we reset that as well.
            if (!GetIsInitialized(sender)) return; //Call only after first initialization
            InitiateImages(sender, resetCaches, affected_props);
        }

        private static void ProcessColorChangeSingle(DependencyObject sender, DependencyPropertyChangedEventArgs e, string propname) {
         
            ProcessColorChange(sender, new string[] { propname });
        }

        private static void ProcessColorChange(DependencyObject sender, string[] affected_props = null) {
            try {
                //Don't perform if not initialized
                if (!GetIsInitialized(sender)) return;

                //Process Image Colors
                if (!GetDisableColorChange(sender)) {
                    //Mark the change in progress
                    if (!GetChangeInProgress(sender)) {
                        SetChangeInProgress(sender, true);
                    }

                    //Fetching the imagesource and setting is very fast.
                    if (affected_props == null || affected_props.Contains(DEFAULT_COLOR)) {
                        if (GetDefaultColor(sender) != null) { SetDefault(sender, ImageHelper.ChangeColor(GetDefault(sender), GetDefaultColor(sender))); }
                    }

                    if (affected_props == null || affected_props.Contains(HOVER_COLOR)) {
                        if (GetHoverColor(sender) != null) { SetHover(sender, ImageHelper.ChangeColor(GetHover(sender), GetHoverColor(sender))); }
                    }

                    if (affected_props == null || affected_props.Contains(PRESSED_COLOR)) {
                        if (GetPressedColor(sender) != null) { SetPressed(sender, ImageHelper.ChangeColor(GetPressed(sender), GetPressedColor(sender))); }
                    }
                }
            } finally {
                SetChangeInProgress(sender, false);
            }
        }

        private static void ReInitiateImages(DependencyObject d, DependencyPropertyChangedEventArgs e, string propname) {
            if (e.OldValue == e.NewValue || GetChangeInProgress(d) || string.IsNullOrWhiteSpace(propname)) return;
            if (!GetIsInitialized(d)) return;
            ReInitiateImages(d, false, new string[] { propname });
        }
       
        void Execute_Initiate(object sender, ExecutedRoutedEventArgs e) {
            if (sender is DependencyObject sender_do)
            {
                InitiateImages(sender_do);
            }
        }
    }
}
