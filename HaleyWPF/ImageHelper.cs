using Haley.Utils;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Haley.Models;

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
        public static void changeColor(string propname, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //CHECK: Is New color value empty?
            if (e.NewValue == null || d == null) return;

            //CHECK: Is new value is actually a SolidColorBrush?
            var _newcolor = e.NewValue as SolidColorBrush;
            if (_newcolor == null) return;

            //CHECK: Does the dependency object contain an actual property with the provided name?
            var actual_propInfo = d.GetType().GetProperty(propname);
            if (actual_propInfo == null) return;

            //CHECK: If property is found, then is it really an imagesource type?
            ImageSource target_imagesource = actual_propInfo.GetValue(d) as ImageSource;
            if (target_imagesource == null) return;

            //If all okay change the color.
            ImageSource modified_source = null;

            modified_source = changeColor(target_imagesource, _newcolor);
            if (modified_source != null)
            { target_imagesource = modified_source; }
        }

        public static ImageSource changeColor(ImageSource source, SolidColorBrush brush)
        {
            //FIRST LOADING IS DIRECT AND SUBSEQUENT ARE PREPARED USING CACHED BITMAP BY DOT NET. SO, NO NEED TO IMPLEMENT A SEPARATE CACHING SYSTEM.
            //ImageRequest _request = new ImageRequest(source, brush);
            ImageSource result = null;
            //if (ImageCacheSource.ContainsKey(_request))
            //{
            //    ImageCacheSource.TryGetValue(_request, out result);
            //}
            //else
            //{
            //    result = ImageUtils.changeImageColor(source, brush);
            //    ImageCacheSource.TryAdd(_request, result);
            //}
            result = ImageUtils.changeImageColor(source, brush);
            return result;
        }
    }
}
