using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Data;
using System.Collections;
using System.Xml.Linq;
//using System.Drawing;

namespace Haley.Utils {

    public class BindingUtils {

        public static CollectionViewSource PopulateCollectionViewSource(IEnumerable items_source,Predicate<object> filter = null) {
			try {

                var source = new CollectionViewSource(); //Whenever the elements changed (not updated/modified but completely replaced with another), our binding is also lost. So, if we need the filters and collection view source to work properly, we should reset the binding.
                Binding binding = new Binding();
                binding.Source = items_source;
                BindingOperations.SetBinding(source, CollectionViewSource.SourceProperty, binding);
                if (filter != null) {
                    try {
                        source.View.Filter = (item) => filter(item);
                    } catch (Exception) {
                        throw;
                    }
                }
                source.View.Refresh();
                return source;
            } catch (Exception) {
               return new CollectionViewSource() { Source = items_source };
			}
        }
    }
}
