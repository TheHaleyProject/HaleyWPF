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
using Haley.Utils;
using Haley.Enums;

namespace Haley.WPF.GroupControls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class FlexiMenu : Control
    {
        #region Attributes
        private static double _headerRegionHeight = Convert.ToDouble(100);
        private static double _optionRegionHeight = Convert.ToDouble(150);
        private static double _menuItemHeight = Convert.ToDouble(30);
        private static double _optionItemHeight = Convert.ToDouble(30);
        private static double _menuBarWidth = Convert.ToDouble(250);
        #endregion

        #region UIElements

        #endregion

        #region Constructors
        public FlexiMenu()
        {
            //For both menuitems and option items, we do not directly allow the items to raise the command. When the button is clicked, we raise an application command which will be handled in this class and corresponding action will be taken.
            this.MenuItems = new ObservableCollection<MenuItem>();
            this.OptionItems = new ObservableCollection<MenuItem>();
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ExecuteAction, ProcessAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Toggle, ToggleMenuBar));
        }

        static FlexiMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiMenu), new FrameworkPropertyMetadata(typeof(FlexiMenu)));
        }
        #endregion

        #region Properties
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
            DependencyProperty.Register(nameof(ControlContainer), typeof(IHaleyUIContainer<IHaleyControlVM, IHaleyControl>), typeof(FlexiMenu), new PropertyMetadata(null));

        public string FootNote
        {
            get { return (string)GetValue(FootNoteProperty); }
            set { SetValue(FootNoteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FootNote.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FootNoteProperty =
            DependencyProperty.Register(nameof(FootNote), typeof(string), typeof(FlexiMenu), new PropertyMetadata("Foot Note"));

        public bool IsMenuBarOpen
        {
            get { return (bool)GetValue(IsMenuBarOpenProperty); }
            set { SetValue(IsMenuBarOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMenuBarOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMenuBarOpenProperty =
            DependencyProperty.Register(nameof(IsMenuBarOpen), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(true));

        public DockLocation Location
        {
            get { return (DockLocation)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register(nameof(Location), typeof(DockLocation), typeof(FlexiMenu), new PropertyMetadata(DockLocation.Left));

        #region Heights

        public double HeaderRegionHeight
        {
            get { return (double)GetValue(HeaderRegionHeightProperty); }
            set { SetValue(HeaderRegionHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderRegionHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderRegionHeightProperty =
            DependencyProperty.Register(nameof(HeaderRegionHeight), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_headerRegionHeight));

        public double MenuItemHeight
        {
            get { return (double)GetValue(MenuItemHeightProperty); }
            set { SetValue(MenuItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemHeightProperty =
            DependencyProperty.Register(nameof(MenuItemHeight), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_menuItemHeight));

        public double OptionItemHeight
        {
            get { return (double)GetValue(OptionItemHeightProperty); }
            set { SetValue(OptionItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionItemHeightProperty =
            DependencyProperty.Register(nameof(OptionItemHeight), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_optionItemHeight));

        public double OptionRegionHeight
        {
            get { return (double)GetValue(OptionRegionHeightProperty); }
            set { SetValue(OptionRegionHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionRegionHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionRegionHeightProperty =
            DependencyProperty.Register(nameof(OptionRegionHeight), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_optionRegionHeight));

        public double MenuBarWidth
        {
            get { return (double)GetValue(MenuBarWidthProperty); }
            set { SetValue(MenuBarWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuBarWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuBarWidthProperty =
            DependencyProperty.Register(nameof(MenuBarWidth), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_menuBarWidth));

        #endregion

        #region Header Region
        public string HeaderLabel
        {
            get { return (string)GetValue(HeaderLabelProperty); }
            set { SetValue(HeaderLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderLabelProperty =
            DependencyProperty.Register(nameof(HeaderLabel), typeof(string), typeof(FlexiMenu), new PropertyMetadata("Project Header"));

        public ImageSource Logo
        {
            get { return (ImageSource)GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Logo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoProperty =
            DependencyProperty.Register(nameof(Logo), typeof(ImageSource), typeof(FlexiMenu), new PropertyMetadata(null));

        public bool ShowHeaderRegion
        {
            get { return (bool)GetValue(ShowHeaderRegionProperty); }
            set { SetValue(ShowHeaderRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowHeaderRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowHeaderRegionProperty =
            DependencyProperty.Register(nameof(ShowHeaderRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(true));

        public Brush HeaderBackGround
        {
            get { return (Brush)GetValue(HeaderBackGroundProperty); }
            set { SetValue(HeaderBackGroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderBackGround.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBackGroundProperty =
            DependencyProperty.Register(nameof(HeaderBackGround), typeof(Brush), typeof(FlexiMenu), new PropertyMetadata(null));
        #endregion
        #endregion

        #region Overridden Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var items = this.MenuItems;
            var ops = this.OptionItems;
        }

        #endregion

        #region Private Methods
        void ProcessAction(object sender, ExecutedRoutedEventArgs e)
        {
            //Send in the menu item as the parameter. Use that to fetch the target from the menuitem list or option item list and process the action.
        }
        void ToggleMenuBar(object sender, ExecutedRoutedEventArgs e)
        {
            //Toggle the menu bar.
            this.SetCurrentValue(IsMenuBarOpenProperty, !IsMenuBarOpen); //We just toggle the value.
        }
        #endregion
    }
}
