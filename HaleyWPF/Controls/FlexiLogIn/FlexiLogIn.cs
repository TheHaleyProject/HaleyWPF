﻿using System;
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
using Haley.MVVM;

namespace Haley.WPF.Controls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class FlexiLogIn: Control
    {
        #region Attributes
        private const string UIEMainContentHolder = "PART_MainContentArea";
        private const string UIEMessageHolder = "PART_messageHolder";
        private const string UIEMessage = "PART_message";
        private const string UIEHeaderHolder = "PART_header";

        private static double _headerRegionHeight = Convert.ToDouble(100);
        private static double _menuItemHeight = Convert.ToDouble(30);
        private static double _menuBarWidth = Convert.ToDouble(250);
        #endregion

        #region UIElements
        private ContentControl _mainContentHolder;
        private FrameworkElement _messageHolder;
        private TextBlock _message;
        private ContentControl _headerHolder;
        #endregion

        #region Constructors
        public FlexiMenu()
        {
            //For both menuitems and option items, we do not directly allow the items to raise the command. When the button is clicked, we raise an application command which will be handled in this class and corresponding action will be taken.
            this.MenuItems = new ObservableCollection<MenuItem>();
            this.OptionItems = new ObservableCollection<MenuItem>();
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ExecuteAction, _processAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Toggle, _toggleMenuBar));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, _closeMessage));
        }

        static FlexiMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiMenu), new FrameworkPropertyMetadata(typeof(FlexiMenu)));
        }
        #endregion

        #region Properties

        public UserControl WelcomeView
        {
            get { return (UserControl)GetValue(WelcomeViewProperty); }
            set { SetValue(WelcomeViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WelcomeView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WelcomeViewProperty =
            DependencyProperty.Register(nameof(WelcomeView), typeof(UserControl), typeof(FlexiMenu), new PropertyMetadata(null));

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


        public IHaleyUIContainer<IHaleyControlVM, IHaleyControl> LocalContainer
        {
            get { return (IHaleyUIContainer<IHaleyControlVM, IHaleyControl>)GetValue(LocalContainerProperty); }
            set { SetValue(LocalContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocalContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalContainerProperty =
            DependencyProperty.Register(nameof(LocalContainer), typeof(IHaleyUIContainer<IHaleyControlVM, IHaleyControl>), typeof(FlexiMenu), new PropertyMetadata(null));

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
        public bool ShowHeaderRegion
        {
            get { return (bool)GetValue(ShowHeaderRegionProperty); }
            set { SetValue(ShowHeaderRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowHeaderRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowHeaderRegionProperty =
            DependencyProperty.Register(nameof(ShowHeaderRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(true));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(FlexiMenu), new FrameworkPropertyMetadata(null,propertyChangedCallback:HeaderTemplatePropertyChanged));
        #endregion
        #endregion

        #region Overridden Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var items = this.MenuItems;
            var ops = this.OptionItems;

            _mainContentHolder = GetTemplateChild(UIEMainContentHolder) as ContentControl;
            _messageHolder = GetTemplateChild(UIEMessageHolder) as FrameworkElement;
            _message = GetTemplateChild(UIEMessage) as TextBlock;
            _headerHolder = GetTemplateChild(UIEHeaderHolder) as ContentControl;

            //Set Welcome view if not null
            if(WelcomeView != null)
            {
                _mainContentHolder.Content = WelcomeView;
            }
            _changeHeader();
        }

        #endregion

        #region Private Methods
        void _processAction(object sender, ExecutedRoutedEventArgs e)
        {
            //First close any previous message
            _closeMessage();

            //Send in the menu item as the parameter. Use that to fetch the target from the menuitem list or option item list and process the action.
            var _inputitem = e.Parameter as MenuItem;
            if (_inputitem == null) return; 

            var _targetItem = MenuItems.FirstOrDefault(p => p.Id == _inputitem.Id && p.Label == _inputitem.Label);

            if (_targetItem==null)
            {
                _targetItem = OptionItems.FirstOrDefault(p => p.Id == _inputitem.Id && p.Label == _inputitem.Label);
            }

            //Even after this, we are not able to get the target item, return.
            if (_targetItem == null) return; 

            //Now, based on the target item, process the action.
            switch(_targetItem.Action)
            {
                case MenuAction.RaiseCommand:
                    _executeCommand(_targetItem); //Raise the command and send parameter along with it.
                    break;
                case MenuAction.ShowContainerView:
                    _setContainerView(_targetItem);
                    break;
                case MenuAction.ShowLocalView:
                    if (_mainContentHolder != null && _targetItem.View != null)
                    {
                        _mainContentHolder.Content = _targetItem.View;
                    }
                    break;
            }
        }
        void _executeCommand(MenuItem item)
        {
            switch (item.Command)
            {
                case null:
                    return;
                default:
                    item.Command.Execute(item.CommandParameter);
                    break;
            }
        }
        void _setContainerView(MenuItem item)
        {
            try
            {
                //First ensure that the key is present.
                if (string.IsNullOrEmpty(item.ContainerKey))
                {
                    //Set the error as message
                    _message.Text = $@"Container key cannot be empty. Please assign a container key value.";
                    _messageHolder.Visibility = Visibility.Visible;
                    return;
                }

                //Check the menu item to find which container to use.
                UserControl _targetView = null;

                var _globalContainer = ContainerStore.Singleton.controls;

                //PRIORITY 1 : If local container is present, then try to find the view. 
                if (_targetView == null && !item.IgnoreLocalContainer && LocalContainer != null)
                {
                    if (LocalContainer.ContainsKey(item.ContainerKey))
                    {
                        _targetView = (UserControl)LocalContainer.generateView(item.ContainerKey);
                    }
                }

                //PRIORIY 2 : If global container is present and also view is still empty, try finding the view.
                if (_targetView == null && !item.IgnoreGlobalContainer && _globalContainer != null)
                {
                    if (_globalContainer.ContainsKey(item.ContainerKey))
                    {
                        _targetView = (UserControl)_globalContainer.generateView(item.ContainerKey);
                    }
                }

                //Somehow , if we manage to find the view, then set it.
                if (_mainContentHolder != null && _targetView != null)
                {
                    _mainContentHolder.Content = _targetView;
                }
                else
                {
                    _setMessage($@"Unable to set view for container key - {item.ContainerKey}. Check if key is correct");
                }
            }
            catch (Exception ex)
            {
                _setMessage(ex.ToString());
            }
        }
        void _setMessage(string message)
        {
            if (_messageHolder == null) return;
            //Set the error as message
            _message.Text = message;
            _messageHolder.Visibility = Visibility.Visible;
        }
        void _toggleMenuBar(object sender, ExecutedRoutedEventArgs e)
        {
            string _param = e.Parameter as string;
            if (_param == null) return;
            switch(_param)
            {
                case "Width":
                    //Toggle the menu bar width
                    this.SetCurrentValue(IsMenuBarOpenProperty, !IsMenuBarOpen); //We just toggle the value.
                    break;
                case "Location":
                    //Toggle the menu bar location
                    DockLocation _newlocation = Location == DockLocation.Left ? DockLocation.Right : DockLocation.Left;
                    this.SetCurrentValue(LocationProperty, _newlocation); //We just toggle the value.
                    break;
            }
        }
        void _closeMessage(object sender, ExecutedRoutedEventArgs e)
        {
            _closeMessage();  
        }
        void _closeMessage()
        {
            //hide the message display.
            if (_messageHolder != null)
            {
                _messageHolder.Visibility = Visibility.Collapsed;
            }
        }
        static void HeaderTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlexiMenu flexiobj = d as FlexiMenu;
            if (flexiobj == null) return;
            flexiobj._changeHeader();
        }
        void _changeHeader()
        {
            if (_headerHolder == null) return;

            _headerHolder.ContentTemplate = HeaderTemplate;

            if (HeaderTemplate == null)
            {
                //We also hide the header
                this.SetCurrentValue(ShowHeaderRegionProperty, false);
            }
        }
        #endregion
    }
}
