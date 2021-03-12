using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections.Concurrent;
using System.ComponentModel;


namespace Haley.Models
{
    public class CommonDictionary:ResourceDictionary
    {
        public CommonDictionary() { }//need a new constructor

        //private static bool IsInDesignMode = DesignerProperties.GetIsInDesignMode(this); //NEED A DEPENDENCY PROPERTY. CANNOT CALL FROM HERE.
        //private static bool IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

        //A static dictionary
        public static ConcurrentDictionary<Uri, ResourceDictionary> DictionaryStore = new ConcurrentDictionary<Uri, ResourceDictionary>();

        private Uri _source;

        public new Uri Source
        {
            get 
            { 
                //if (IsInDesignMode)
                //{
                //    return base.Source;
                //}
                return _source; 
            }
            set 
            {
                if (value == null) return;
                
                //if (IsInDesignMode)
                //{
                //    base.Source = value;
                //    return;
                //}

                //Instead of creating a new RD each time, we create and save the RD in a ConcurrentDictionary.
                //So, next time some user control requests a RD for this same URI, we fetch the RD from our store and then add it to merged dictionaries. basically, we are having same dictionary referenced everywhere. The first benefit would be that if we change theme in one place, it should affect all places.
                if (!DictionaryStore.ContainsKey(value))
                {
                    DictionaryStore.TryAdd(value, this);
                }
                else
                {
                    ResourceDictionary result;
                    DictionaryStore.TryGetValue(value, out result);
                    MergedDictionaries.Add(DictionaryStore[value]);
                }

                //And finally, we assign the source value to this resource dictionary.
                //We are also adding the value to the source to generate the dictionary
                base.Source = value;
                _source = value; 
            }
        }

    }
}
