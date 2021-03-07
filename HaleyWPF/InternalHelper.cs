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
using Haley.Utils;
using Haley.WPF.BaseControls;
using Haley.Enums;


namespace Haley.WPF
{
    internal static class InternalHelper
    {
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
            //TODO : AT THIS POINT INSTEAD OF CHANGING COLOR OF THE IMAGESOURCE USING IMAGE UTILS EVERYTIME, TRY TO ADD A CACHE WHERE THE IMAGESOURCE FOR A SPECIFIED SOLIDCOLOR BRUSH IS STORED AND RETRIEVED. SAVES PROCESSING AND MEMORY.

            return ImageUtils.changeImageColor(source, brush);

            //Cache based service to be added.
        }
    }
}
