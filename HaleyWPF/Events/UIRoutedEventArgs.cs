﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Haley.Events
{
    public class UIRoutedEventArgs<T> : RoutedEventArgs
    {
        public T value { get; set; }
        public UIRoutedEventArgs(RoutedEvent base_event,object sender) : base(base_event,sender)
        {
        }
    }
}
