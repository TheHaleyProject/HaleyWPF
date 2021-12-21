using System;
using System.Windows;

namespace Haley.Events
{
    public class ValueChangedArgs<T> : EventArgs
    {
        public string Message { get; set; }
        public T Value { get; set; }
        public ValueChangedArgs(T value,string message = null)
        {
            Value = value;
            Message = message;
        }
    }
}
