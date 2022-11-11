using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Haley.Enums;
using Haley.WPF.Controls;
using Haley.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Haley.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Markup;
using System.ComponentModel;

namespace Haley.Utils
{
    public class ImgIntExtension : MarkupExtension {
        private IconSourceProvider _sourceProvider = new IconSourceProvider();
        public ImgIntExtension(string binding_source) : this() {
            BindingSource = binding_source;
        }
        public ImgIntExtension() {
            Kind = IconKind.empty_image;
        }

        public string BindingSource { get; set; }

        //[ConstructorArgument("@enum")] //If we setup constructor argument, empty values will throw exception
        public IconKind Kind { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {

            //PROVIDE VALUE WILL BE CALLED WHENEVER ANY PROPERTY ON THIS WILL BE CHANGED.
            if (!string.IsNullOrWhiteSpace(BindingSource)) {
                //Binding takes top most priority
                //If binding is not null, we try to fetch the datacontext of the target element and then bind to the changes.
                if (CommonUtils.GetTargetElement(serviceProvider, out var target)) {
                    //create binding and then return the binding expression, rather than directly returning the value.
                    target.TargetObject.DataContextChanged -= TargetDataChanged;
                    target.TargetObject.DataContextChanged += TargetDataChanged; //To receive the property changes during runtime
                    var binding = CreateBinding();
                    return binding.ProvideValue(serviceProvider); //This will provide the value of IconSource Property
                    //If we directly add a binding to the property name, then whenever the value of that property changes, we will 
                }
            }

            if (!Process(out var @enum, out var @key)) return null;
            return IconFinder.GetIcon(key, @enum); //When we direclty return value like this, it will not be trigger.
        }

        private Binding CreateBinding() {
            //Create a binding that will provide value eventually
            //This binding itself is to return value from EXT to DO
            var binding = new Binding(nameof(IconSourceProvider.IconSource)) {
                Source = _sourceProvider
            };
            return binding;
        }

        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e) {
            //Todo when property changes.
            //now check and compare the property.
            if (e.PropertyName != BindingSource) return; //ignore don't try to change the image.
                                                         //Get proeprty from sender
            object propValue = null;
            try {
                propValue = sender.GetType()?.GetProperty(BindingSource)?.GetValue(sender);
                
            } catch (Exception) {

            }
            _sourceProvider.OnDataChanged(propValue); //this will be the new data.
        }

        bool Process(out IconSourceKey source_key, out string resource_key) {
            source_key = IconSourceKey.Default; //We have only one source key for the Inbuilt wpf images
            resource_key = Kind.ToString();
            return true;
        }
        private void TargetDataChanged(object sender, DependencyPropertyChangedEventArgs e) {
            object propValue = e.NewValue;
            try {
                //To receive message whenever the property value is changed.
                //Since we are dealing with DataContextChange, we will always get DataContext Property
                //If Binding Source is "." then we directly bind the property. So, don't process or validate.
                if (e.NewValue != null && !(e.NewValue is string || e.NewValue is Enum) && BindingSource != ".") {
                    propValue = CommonUtils.FetchValueAndMonitor(e.NewValue, BindingSource, ObjectPropertyChanged);
                }
            } catch (Exception) { }
            _sourceProvider.OnDataChanged(propValue); //this will be the new data.
        }
    }
}