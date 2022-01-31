using System;
using System.Windows;

namespace Haley.Events
{
    public class UIRoutedEventArgs<T> : RoutedEventArgs
    {
        public string Message { get; set; }
        public T Value { get; set; }
        public UIRoutedEventArgs(RoutedEvent base_event, object sender) : base(base_event, sender)
        {
        }
    }
}
