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
        public static readonly DependencyProperty DefaultColorProperty =
            DependencyProperty.RegisterAttached("DefaultColor", typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata(_defaultColorChanged));

        public static readonly DependencyProperty DefaultKindProperty =
            DependencyProperty.RegisterAttached(DEFAULT_KIND, typeof(IconKind?), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, DEFAULT_KIND)));

        public static readonly DependencyProperty DefaultProperty =
                    DependencyProperty.RegisterAttached(DEFAULT, typeof(ImageSource), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, DEFAULT)));

        public static readonly DependencyProperty DisableColorChangeProperty =
                            DependencyProperty.RegisterAttached("DisableColorChange", typeof(bool), typeof(Icon), new PropertyMetadata(false));

        public static readonly DependencyProperty HoverColorProperty =
            DependencyProperty.RegisterAttached("HoverColor", typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata(_hoverColorChanged));

        public static readonly DependencyProperty HoverKindProperty =
            DependencyProperty.RegisterAttached(HOVER_KIND, typeof(IconKind?), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, HOVER_KIND)));

        public static readonly DependencyProperty HoverProperty =
                    DependencyProperty.RegisterAttached(HOVER, typeof(ImageSource), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, HOVER)));

        public static readonly DependencyProperty PreferenceProperty =
            //DependencyProperty.RegisterAttached("Preference", typeof(IconSourcePreference), typeof(Icon), new FrameworkPropertyMetadata(IconSourcePreference.ImageSource,propertyChangedCallback:ReInitiateImages));
            DependencyProperty.RegisterAttached("Preference", typeof(IconSourcePreference), typeof(Icon), new FrameworkPropertyMetadata(IconSourcePreference.ImageSource));

        public static readonly DependencyProperty PressedColorProperty =
                    DependencyProperty.RegisterAttached("PressedColor", typeof(SolidColorBrush), typeof(Icon), new FrameworkPropertyMetadata(_pressedColorChanged));

        //Reinitiating when preference changes resets all the cache. Think of a better solution before using reinititate
        public static readonly DependencyProperty PressedKindProperty =
            DependencyProperty.RegisterAttached(PRESSED_KIND, typeof(IconKind?), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, PRESSED_KIND)));

        public static readonly DependencyProperty PressedProperty =
                    DependencyProperty.RegisterAttached(PRESSED, typeof(ImageSource), typeof(Icon), new FrameworkPropertyMetadata((d, e) => ReInitiateImages(d, e, PRESSED)));

        public static ImageSource EmptyImage = ResourceHelper.getIcon(IconKind.empty_image.ToString());

        private const string DEFAULT = "Default";

        private const string DEFAULT_KIND = "DefaultKind";

        private const string HOVER = "Hover";

        private const string HOVER_KIND = "HoverKind";

        private const string PRESSED = "Pressed";

        private const string PRESSED_KIND = "PressedKind";

        private static readonly DependencyProperty ChangeInProgressProperty =
                                                                                                                    DependencyProperty.RegisterAttached("ChangeInProgress", typeof(bool), typeof(Icon), new PropertyMetadata(false));

        private static readonly DependencyProperty InitializationStatusProperty =
                                                                                                            DependencyProperty.RegisterAttached("InitializationStatus", typeof(Dictionary<string, bool>), typeof(Icon), new PropertyMetadata(new Dictionary<string, bool>()));

        public static readonly DependencyProperty IsHandlerProperty =
            DependencyProperty.RegisterAttached("IsHandler", typeof(bool), typeof(Icon), new PropertyMetadata(false));
        public Icon() {
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Initiate, Execute_Initiate));
        }

        public static ImageSource GetDefault(DependencyObject obj) {
            return (ImageSource)obj.GetValue(DefaultProperty);
        }

        public static SolidColorBrush GetDefaultColor(DependencyObject obj) {
            return (SolidColorBrush)obj.GetValue(DefaultColorProperty);
        }

        public static IconKind? GetDefaultKind(DependencyObject obj) {
            return (IconKind?)obj.GetValue(DefaultKindProperty);
        }

        public static bool GetDisableColorChange(DependencyObject obj) {
            return (bool)obj.GetValue(DisableColorChangeProperty);
        }

        public static ImageSource GetHover(DependencyObject obj) {
            return (ImageSource)obj.GetValue(HoverProperty);
        }

        public static SolidColorBrush GetHoverColor(DependencyObject obj) {
            return (SolidColorBrush)obj.GetValue(HoverColorProperty);
        }

        public static IconKind? GetHoverKind(DependencyObject obj) {
            return (IconKind?)obj.GetValue(HoverKindProperty);
        }

        public static IconSourcePreference GetPreference(DependencyObject obj) {
            return (IconSourcePreference)obj.GetValue(PreferenceProperty);
        }

        public static ImageSource GetPressed(DependencyObject obj) {
            return (ImageSource)obj.GetValue(PressedProperty);
        }

        public static SolidColorBrush GetPressedColor(DependencyObject obj) {
            return (SolidColorBrush)obj.GetValue(PressedColorProperty);
        }

        public static IconKind? GetPressedKind(DependencyObject obj) {
            return (IconKind?)obj.GetValue(PressedKindProperty);
        }

        public static void InitiateImages(DependencyObject sender, bool resetCaches = false, string[] affected_props = null) {
            //ONLY HANDLERS INITIATE THE IMAGES
            SetIsHandler(sender, true);
            SetChangeInProgress(sender, true);

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

            var defImage = GetDefault(sender);
            //Process Images
            if (defImage == null || (resetCaches && affected_props.Contains(DEFAULT) && defImage is CachedBitmap)) {
                SetDefault(sender, EmptyImage);
            }

            var hoverImage = GetHover(sender);
            if (hoverImage == null || (resetCaches && affected_props.Contains(HOVER) && (hoverImage is CachedBitmap || hoverImage == EmptyImage))) SetHover(sender, GetDefault(sender));

            var pressedImage = GetPressed(sender);
            if (pressedImage == null || (resetCaches && affected_props.Contains(PRESSED) && (pressedImage is CachedBitmap || pressedImage == EmptyImage))) SetPressed(sender, GetDefault(sender));
            ProcessColorChange(sender);
            SetChangeInProgress(sender, false);
        }

        public static void ReInitiateImages(DependencyObject sender, bool resetCaches = true, string[] affected_props = null) {
            //When there is no image, we get null, but when we have set some image, it is then cached. So, subsequent calls will return cached bitmap. This method of initiate image itself is called only when we try to initiate for the first time. So, if deliberately we initiate for second time, we should have already changed the image. So, we check if it is cached, then we reset that as well.
            InitiateImages(sender, resetCaches, affected_props);
        }

        public static void SetDefault(DependencyObject obj, ImageSource value) {
            obj.SetValue(DefaultProperty, value);
        }

        public static void SetDefaultColor(DependencyObject obj, SolidColorBrush value) {
            obj.SetValue(DefaultColorProperty, value);
        }

        public static void SetDefaultKind(DependencyObject obj, IconKind? value) {
            obj.SetValue(DefaultKindProperty, value);
        }

        public static void SetDisableColorChange(DependencyObject obj, bool value) {
            obj.SetValue(DisableColorChangeProperty, value);
        }

        public static void SetHover(DependencyObject obj, ImageSource value) {
            obj.SetValue(HoverProperty, value);
        }

        public static void SetHoverColor(DependencyObject obj, SolidColorBrush value) {
            obj.SetValue(HoverColorProperty, value);
        }

        public static void SetHoverKind(DependencyObject obj, IconKind? value) {
            obj.SetValue(HoverKindProperty, value);
        }

        public static void SetPreference(DependencyObject obj, IconSourcePreference value) {
            obj.SetValue(PreferenceProperty, value);
        }

        public static void SetPressed(DependencyObject obj, ImageSource value) {
            obj.SetValue(PressedProperty, value);
        }

        public static void SetPressedColor(DependencyObject obj, SolidColorBrush value) {
            obj.SetValue(PressedColorProperty, value);
        }

        public static void SetPressedKind(DependencyObject obj, IconKind? value) {
            obj.SetValue(PressedKindProperty, value);
        }

        private static void _defaultColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ProcessColorChange(d, new string[] { DEFAULT });
        }

        private static void _hoverColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ProcessColorChange(d, new string[] { HOVER });
        }

        private static void _pressedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ProcessColorChange(d, new string[] { PRESSED });
        }

        private static bool GetChangeInProgress(DependencyObject obj) {
            return (bool)obj.GetValue(ChangeInProgressProperty);
        }

        private static Dictionary<string, bool> GetInitializationStatus(DependencyObject obj) {
            return (Dictionary<string, bool>)obj.GetValue(InitializationStatusProperty);
        }

        public static bool GetIsHandler(DependencyObject obj) {
            return (bool)obj.GetValue(IsHandlerProperty);
        }

        private static void ProcessColorChange(DependencyObject sender, string[] affected_props = null) {
            bool newchange_inprogress = false;
            try {
                //Process Image Colors
                if (!GetDisableColorChange(sender)) {

                    //Mark the change in progress
                    if (!GetChangeInProgress(sender)) {
                        SetChangeInProgress(sender, true);
                        newchange_inprogress = true;
                    }

                    //Fetching the imagesource and setting is very fast.
                    if (affected_props == null || affected_props.Contains(DEFAULT)) {
                        if (GetDefaultColor(sender) != null) { SetDefault(sender, ImageHelper.ChangeColor(GetDefault(sender), GetDefaultColor(sender))); }
                    }

                    if (affected_props == null || affected_props.Contains(HOVER)) {
                        if (GetHoverColor(sender) != null) { SetHover(sender, ImageHelper.ChangeColor(GetHover(sender), GetHoverColor(sender))); }
                    }

                    if (affected_props == null || affected_props.Contains(PRESSED)) {
                        if (GetPressedColor(sender) != null) { SetPressed(sender, ImageHelper.ChangeColor(GetPressed(sender), GetPressedColor(sender))); }
                    }
                }
            } finally {
                if (newchange_inprogress) {
                    SetChangeInProgress(sender, false);
                }
            }
        }

        private static void ReInitiateImages(DependencyObject d, DependencyPropertyChangedEventArgs e, string propname) {

            //Images should only be reinitiated by the final initiator. Whatever remains in between are merely holders.
            if (!GetIsHandler(d)) return;

            var status_dic = GetInitializationStatus(d);
            if (!status_dic.ContainsKey(propname)) {
                status_dic.Add(propname, false); //Not initialized
            }
            if (!status_dic[propname]) {
                //If not initialized, we should not try to reinitiate. else it will return empty image.
                status_dic[propname] = true; //consider it initialized.
                return;
            }
            //no change or is in progress or this is the first setting
            if (e.OldValue == e.NewValue || GetChangeInProgress(d) || string.IsNullOrWhiteSpace(propname)) return;
            //if (!GetChangeInProgress(d) && e.NewValue is CachedBitmap) return; // needed because we also try to set the cached image after color change

            ReInitiateImages(d, true, new string[] { propname });
        }

        private static void SetChangeInProgress(DependencyObject obj, bool value) {
            obj.SetValue(ChangeInProgressProperty, value);
        }

        public static void SetIsHandler(DependencyObject obj, bool value) {
            obj.SetValue(IsHandlerProperty, value);
        }
        void Execute_Initiate(object sender, ExecutedRoutedEventArgs e) {
            if (sender is DependencyObject sender_do)
            {
                InitiateImages(sender_do);
            }
        }
    }
}
