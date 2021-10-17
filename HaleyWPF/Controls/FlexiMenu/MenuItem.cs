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
using Haley.Enums;
using Haley.Abstractions;

namespace Haley.WPF.Controls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class MenuItem : CommandMenuItem, IMenuItem
    {
        public MenuItem(){ }
        
        public MenuAction Action
        {
            get { return (MenuAction)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Action.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register(nameof(Action), typeof(MenuAction), typeof(MenuItem), new PropertyMetadata(MenuAction.RaiseCommand));

        public UserControl View
        {
            get { return (UserControl)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register(nameof(View), typeof(UserControl), typeof(MenuItem), new PropertyMetadata(null));

        public string ContainerKey
        {
            get { return (string)GetValue(ContainerKeyProperty); }
            set { SetValue(ContainerKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContainerKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContainerKeyProperty =
            DependencyProperty.Register(nameof(ContainerKey), typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(null));

        public bool IgnoreLocalContainer
        {
            get { return (bool)GetValue(IgnoreLocalContainerProperty); }
            set { SetValue(IgnoreLocalContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreLocalContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreLocalContainerProperty =
            DependencyProperty.Register(nameof(IgnoreLocalContainer), typeof(bool), typeof(MenuItem), new PropertyMetadata(false));

        public bool IgnoreGlobalContainer
        {
            get { return (bool)GetValue(IgnoreGlobalContainerProperty); }
            set { SetValue(IgnoreGlobalContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreGlobalContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreGlobalContainerProperty =
            DependencyProperty.Register(nameof(IgnoreGlobalContainer), typeof(bool), typeof(MenuItem), new PropertyMetadata(false));
    }
}
