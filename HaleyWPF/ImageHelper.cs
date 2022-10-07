using Haley.Models;
using Haley.Utils;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Haley.WPF
{
    internal static class ImageHelper
    {
        private static ConcurrentDictionary<ImageRequest, ImageSource> ImageCacheSource = new ConcurrentDictionary<ImageRequest, ImageSource>();

        /// <summary>
        /// Changes color
        /// </summary>
        /// <param name="propname">Property name of the ImageSource</param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void ChangeColorWithName(string propname, DependencyObject d, object value)
        {
            try
            {
                Func<DependencyPropertyDescriptor> _getDescriptor = () => { return DependencyPropertyDescriptor.FromName(propname, d.GetType(),d.GetType()); };

                if (value is DependencyPropertyChangedEventArgs e) {
                    value = e.NewValue;
                    if (value == null) return;
                }

                changeColorInternal(_getDescriptor, d, value );
                
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void ChangeColor(DependencyProperty prop, DependencyObject d, object value) {
            try {
                Func<DependencyPropertyDescriptor> _getDescriptor = () => { return DependencyPropertyDescriptor.FromProperty(prop, d.GetType()); };

                if (value is DependencyPropertyChangedEventArgs e) {
                    value = e.NewValue;
                    if (value == null) return;
                }

                changeColorInternal(_getDescriptor, d, value);
            } catch (Exception) {
                return;
            }
        }

        ///// <summary>
        ///// Changes color
        ///// </summary>
        ///// <param name="propname">Property name of the ImageSource</param>
        ///// <param name="d"></param>
        ///// <param name="e"></param>
        //public static void changeColor(Func<DependencyPropertyDescriptor> descriptorDelgate, DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //CHECK: Is New color value empty?
        //    if (e.NewValue == null || d == null) return;
        //    changeColorInternal(descriptorDelgate, d, e.NewValue);
        //}

        private static void changeColorInternal(Func<DependencyPropertyDescriptor> descriptorDelgate, DependencyObject d, object NewValue) {
            try {
                if (NewValue == null) return;
                //CHECK: Is new value is actually a SolidColorBrush?
                var _newcolor = NewValue as SolidColorBrush;
                if (_newcolor == null) return;

                //CHECK: Does the dependency object contain an actual property with the provided name?
                var actual_propInfo = descriptorDelgate.Invoke();
                //var actual_propInfo = d.GetType().GetProperty(propname); //WORKS FOR BOTH NORMAL AND DEPENDENCY OBJECTS.
                if (actual_propInfo == null) return;

                //CHECK: If property is found, then is it really an imagesource type?
                ImageSource target_imagesource = actual_propInfo.GetValue(d) as ImageSource;
                if (target_imagesource == null) return;

                //If all okay change the color.
                //ImageSource modified_source = null;

                var modified_source = ChangeColor(target_imagesource, _newcolor);
                if (modified_source != null) {
                    actual_propInfo.SetValue(d, modified_source);
                    //var property = actual_propInfo.DependencyProperty; //TO GET THE PROPERTY DIRECTLY
                    //d.SetValue(actual_propInfo, modified_source); //IF NORMAL PROPERTY
                }
            } catch (Exception) {
                return;
            }
        }

        public static ImageSource ChangeColor(ImageSource source, SolidColorBrush brush)
        {
            if (source == null) return null;
            //FIRST LOADING IS DIRECT AND SUBSEQUENT ARE PREPARED USING CACHED BITMAP BY DOT NET. SO, NO NEED TO IMPLEMENT A SEPARATE CACHING SYSTEM.
            //ImageRequest _request = new ImageRequest(source, brush);
            //ImageSource result = null;
            //if (ImageCacheSource.ContainsKey(_request))
            //{
            //    ImageCacheSource.TryGetValue(_request, out result);
            //}
            //else
            //{
            //    result = ImageUtils.changeImageColor(source, brush);
            //    ImageCacheSource.TryAdd(_request, result);
            //}
            return ImageUtils.changeImageColor(source, brush);
        }
    }
}
