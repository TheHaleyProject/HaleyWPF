using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;


namespace Haley.Models
{
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register(nameof(DataContext), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
