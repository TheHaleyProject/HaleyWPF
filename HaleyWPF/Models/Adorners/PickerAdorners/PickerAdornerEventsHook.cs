using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Haley.Models
{
    /// <summary>
    /// Class for a hooking up the pickeradorner with this adorned element events.
    /// </summary>
    public sealed class PickerAdornerEventsHook
    {
        private static Dictionary<string, PickerAdornerEventsHook> _hooks = new Dictionary<string, PickerAdornerEventsHook>();

        private UIElement _element;
        private PickerAdornerBase _adorner;

        public static string Hook(PickerAdornerBase adorner)
        {
            if (adorner == null)
            {
                System.Console.WriteLine("Adorner is null. Cannot proceed.");
                return null;
            };

            if (adorner.AdornedElement == null)
            {
                System.Console.WriteLine("Adornered element is null. Cannot proceed.");
                return null;
            }
            var id = adorner?.Id;

            if (!_hooks.ContainsKey(id))
            {
                //Only add if the key is not already added.
                PickerAdornerEventsHook _newhook = new PickerAdornerEventsHook(adorner);
                _hooks.Add(id, _newhook);
            }

            return id;
        }

        public static PickerAdornerEventsHook GetHook(string id)
        {
            if (_hooks.ContainsKey(id))
            {
                return _hooks[id];
            }
            return null;
        }

        public static bool RemoveHook(string id)
        {
            if (_hooks.ContainsKey(id))
            {
                var _hook = _hooks[id];
                _hook._unsubscribe(); //Unsubscribe the events for this hook.
                _hooks.Remove(id);
                return true;
            }
            return false;
        }

       /// <summary>
       /// Hook to link an adorner and the adorned element's events.
       /// </summary>
       /// <param name="adorner"></param>
       internal PickerAdornerEventsHook(PickerAdornerBase adorner)
       {
            //This class is used to convert any FrameworkElement into base for a picker adorner.
            _adorner = adorner;
            _element = adorner?.AdornedElement;

            //Since the element is a UIElement, it should have mouse based events. Based on those events, we need to update our adorner.

            if (_element is FrameworkElement felement)
            {
                felement.Loaded += Felement_Loaded;
                //When the user moves the mouse with Leftbutton pressed down or click on a location, we need to update it.
                felement.MouseMove += _element_MouseMove;
                felement.MouseLeftButtonUp += _element_MouseUp;
            }
       }

        private void Felement_Loaded(object sender, RoutedEventArgs e)
        {
            //This is very important as the LoadedEvent is present only in the framework element.
            //Only for the framework element, we will able to add the adorner layer.
            var _layer = AdornerLayer.GetAdornerLayer(sender as FrameworkElement);
            if (_layer == null)
            {
                throw new ArgumentException("Adorner Layer is null.");
            }
            _layer?.Add(_adorner); //Add the adorner to the layer so that it can draw on top of the UIElement.
            _adorner.Layer = _layer; // Also store the layer to the adorner for further usage.
        }

        private void _element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null); //Donot capture any element.Or else, the previous element is still capture by the mouse. New element will be ignored.
            _updateAdornerPosition(sender, e); //When we click a location and then release mouse button.
        }

        private void _element_MouseMove(object sender, MouseEventArgs e)
        {
            //The element's mouse move is already handled by the element.
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return; // We expect the mouse move to happed only on pressed.
            }
            Mouse.Capture(_element); //Capture the mouse position with respect to the element.
            _updateAdornerPosition(sender, e);
        }

        private void _unsubscribe()
        {
            _element.MouseMove -= _element_MouseMove;
            _element.MouseUp -= _element_MouseUp;
            _adorner.UnSubscribe(); //If there is any subscription, that will be unsubscribed.
        }

        private void _updateAdornerPosition(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(_element); //Get the captured position. with respect to the element.

            if (_element is FrameworkElement fele)
            {
                //the x value should not exceed the elements actual width. So we use the Math.Min function to figure our if we have the low value.
                position.X = Math.Min(Math.Max(0, position.X), fele.ActualWidth);
                position.Y = Math.Min(Math.Max(0, position.Y), fele.ActualHeight);
            }
        
            if (_adorner!= null)
            {
                _adorner.SetPosition(position);
            }
        }
    }
}