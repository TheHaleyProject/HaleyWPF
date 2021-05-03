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
using Haley.Abstractions;
using System.Collections.ObjectModel;

namespace Haley.WPF.GroupControls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class FlexiMenu : Control
    {
        public FlexiMenu()
        {
            this.MenuItems = new ObservableCollection<MenuItem>();
            this.OptionItems = new ObservableCollection<MenuItem>();
        }

        static FlexiMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiMenu), new FrameworkPropertyMetadata(typeof(FlexiMenu)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var items = this.MenuItems;
            var ops = this.OptionItems;
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return (ObservableCollection<MenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems), typeof(ObservableCollection<MenuItem>), typeof(FlexiMenu), new PropertyMetadata(null));

        public ObservableCollection<MenuItem> OptionItems
        {
            get { return (ObservableCollection<MenuItem>)GetValue(OptionItemsProperty); }
            set { SetValue(OptionItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionItemsProperty =
            DependencyProperty.Register(nameof(OptionItems), typeof(ObservableCollection<MenuItem>), typeof(FlexiMenu), new PropertyMetadata(null));


        public IHaleyUIContainer<IHaleyControlVM, IHaleyControl> ControlContainer
        {
            get { return (IHaleyUIContainer<IHaleyControlVM, IHaleyControl>)GetValue(ControlContainerProperty); }
            set { SetValue(ControlContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ControlContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlContainerProperty =
            DependencyProperty.Register("ControlContainer", typeof(IHaleyUIContainer<IHaleyControlVM, IHaleyControl>), typeof(FlexiMenu), new PropertyMetadata(null));
    }
}
