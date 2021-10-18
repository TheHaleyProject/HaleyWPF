using Haley.Abstractions;
using Haley.Enums;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    /// <summary>
    /// Fleximenu for both left/right and topbottom docking.s
    /// </summary>
    public class FlexiMenu : Control
    {
        #region Attributes
        private const string UIEMainContentHolder = "PART_MainContentArea";
        private const string UIEMessageHolder = "PART_messageHolder";
        private const string UIEMessage = "PART_message";
        private const string UIEHeaderHolder = "PART_header";
        private const string UIEFloatingPanel = "PART_FloatingPanel";
        private const string UIEFloatingPanelCanvas = "PART_FloatingPanelHolderCanvas";

        private static double _headerRegionHeight = Convert.ToDouble(100);
        private static double _menuItemHeight = Convert.ToDouble(30);
        private static double _menuBarWidth = Convert.ToDouble(250);
        #endregion

        #region UIElements
        private ContentControl _mainContentHolder;
        private FrameworkElement _messageHolder;
        private TextBlock _message;
        private ContentControl _headerHolder;
        private ContentControl _floatingPanel;
        private Canvas _floatingPanelHolderCanvas;
        #endregion

        #region Constructors
        public FlexiMenu()
        {
            //For both menuitems and option items, we do not directly allow the items to raise the command. When the button is clicked, we raise an application command which will be handled in this class and corresponding action will be taken.
            this.MenuItems = new ObservableCollection<MenuItem>();
            this.OptionItems = new ObservableCollection<CommandMenuItem>();
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ExecuteAction, _processMenuAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ExecuteAction2, _processOptionsAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Toggle, _toggleMenuBar));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, _closeMessage));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Reset , _resetFloatingPanel));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Show, _changeFloatingPanelVisibility));
            _addProxyResource();
            //IControlContainer v;
        }


        static FlexiMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiMenu), new FrameworkPropertyMetadata(typeof(FlexiMenu)));
        }
        #endregion

        #region Properties

        #region FloatingPanel

        public object FloatingPanel
        {
            get { return (object)GetValue(FloatingPanelProperty); }
            set { SetValue(FloatingPanelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FloatingPanel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloatingPanelProperty =
            DependencyProperty.Register("FloatingPanel", typeof(object), typeof(FlexiMenu), new FrameworkPropertyMetadata(null,propertyChangedCallback:FloatingPanelChanged));
        public bool IsFloatingPanelVisible
        {
            get { return (bool)GetValue(IsFloatingPanelVisibleProperty); }
            set { SetValue(IsFloatingPanelVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFloatingPanelVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFloatingPanelVisibleProperty =
            DependencyProperty.Register("IsFloatingPanelVisible", typeof(bool), typeof(FlexiMenu), new PropertyMetadata(true));
        #endregion

        public ImageSource ToggleIcon
        {
            get { return (ImageSource)GetValue(ToggleIconProperty); }
            set { SetValue(ToggleIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleIconProperty =
            DependencyProperty.Register(nameof(ToggleIcon), typeof(ImageSource), typeof(FlexiMenu), new PropertyMetadata(null));

        public Brush SelectedMenuColor
        {
            get { return (Brush)GetValue(SelectedMenuColorProperty); }
            set { SetValue(SelectedMenuColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMenuColorProperty =
            DependencyProperty.Register(nameof(SelectedMenuColor), typeof(Brush), typeof(FlexiMenu), new PropertyMetadata(null));

        public MenuItem ActiveMenu
        {
            get { return (MenuItem)GetValue(ActiveMenuProperty); }
            set { SetValue(ActiveMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveMenuProperty =
            DependencyProperty.Register(nameof(ActiveMenu), typeof(MenuItem), typeof(FlexiMenu), new FrameworkPropertyMetadata(null));

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

        public ObservableCollection<CommandMenuItem> OptionItems
        {
            get { return (ObservableCollection<CommandMenuItem>)GetValue(OptionItemsProperty); }
            set { SetValue(OptionItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionItemsProperty =
            DependencyProperty.Register(nameof(OptionItems), typeof(ObservableCollection<CommandMenuItem>), typeof(FlexiMenu), new PropertyMetadata(null));


        public IControlContainer LocalContainer
        {
            get { return (IControlContainer)GetValue(LocalContainerProperty); }
            set { SetValue(LocalContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocalContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalContainerProperty =
            DependencyProperty.Register(nameof(LocalContainer), typeof(IControlContainer), typeof(FlexiMenu), new PropertyMetadata(null));

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

        public Alignment MenuItemsAlignment
        {
            get { return (Alignment)GetValue(MenuItemsAlignmentProperty); }
            set { SetValue(MenuItemsAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItemsAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsAlignmentProperty =
            DependencyProperty.Register(nameof(MenuItemsAlignment), typeof(Alignment), typeof(FlexiMenu), new PropertyMetadata(Alignment.Left));


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
        public bool HideHeaderRegion
        {
            get { return (bool)GetValue(HideHeaderRegionProperty); }
            set { SetValue(HideHeaderRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowHeaderRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideHeaderRegionProperty =
            DependencyProperty.Register(nameof(HideHeaderRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public bool HideMenuRegion
        {
            get { return (bool)GetValue(HideMenuRegionProperty); }
            set { SetValue(HideMenuRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideMenuRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideMenuRegionProperty =
            DependencyProperty.Register(nameof(HideMenuRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public bool HideOptionsRegion
        {
            get { return (bool)GetValue(HideOptionsRegionProperty); }
            set { SetValue(HideOptionsRegionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideOptionsRegion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideOptionsRegionProperty =
            DependencyProperty.Register(nameof(HideOptionsRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(FlexiMenu), new FrameworkPropertyMetadata(null, propertyChangedCallback: HeaderTemplatePropertyChanged));
        #endregion
#endregion

        private void _addProxyResource()
        {
            if (! this.Resources.Contains("proxy"))
            {
                //Create binding
                var bp = new BindingProxy(); //The proxy to hold the datacontext
                Binding _dcbinding = new Binding();
                //_dcbinding.Source = this.DataContext; //Source dtatacontext
                _dcbinding.NotifyOnSourceUpdated = true;
                _dcbinding.NotifyOnTargetUpdated = true;
                _dcbinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(bp, BindingProxy.DataContextProperty, _dcbinding); //Binding operation.
                //Add resources
                this.Resources.Add("proxy", bp);
            }
        }

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
            _floatingPanel = GetTemplateChild(UIEFloatingPanel) as ContentControl;
            _floatingPanelHolderCanvas = GetTemplateChild(UIEFloatingPanelCanvas) as Canvas;
            //Set Welcome view if not null
            if (WelcomeView != null)
            {
                _mainContentHolder.Content = WelcomeView;
                //If welcome view is active, then we should hide the floating panel
                _setFloatingCanvasVisibility(Visibility.Collapsed);
            }
            _changeHeader();
            _changeFloatingPanel();
        }

        #endregion

        #region Private Methods

        void _setFloatingCanvasVisibility(Visibility _visibility)
        {
            if (_floatingPanelHolderCanvas != null)
            {
                _floatingPanelHolderCanvas.Visibility = _visibility;
            }
        }

        void _processMenuAction(object sender, ExecutedRoutedEventArgs e)
        {
            _processAction(sender, e, true);
        }
        void _processOptionsAction(object sender, ExecutedRoutedEventArgs e)
        {
            _processAction(sender, e, false);
        }

        void _processAction(object sender, ExecutedRoutedEventArgs e,bool isMenu)
        {
            //First close any previous message
            _closeMessage();

            //Send in the menu item as the parameter. Use that to fetch the target from the menuitem list or option item list and process the action.
            var _inputitem = e.Parameter as CommandMenuItem;
            if (_inputitem == null) return;

            var _menuAction = MenuAction.RaiseCommand;
            CommandMenuItem _targetItem = null;

            if(isMenu)
            {
                _targetItem = MenuItems.FirstOrDefault(p => p.Id == _inputitem.Id);

                if (_targetItem is MenuItem _targetMenuItem)
                {
                    _menuAction = _targetMenuItem.Action;
                    if (_targetMenuItem.Action != MenuAction.RaiseCommand)
                    {
                        //Because usually, raising command will only be used for showing dialog or performing actions like signing out, exporting, printing etc. So, we need not highlight that
                        ActiveMenu = _targetMenuItem; //this is actualy set so that it can be used for highlighting.
                    }
                }
            }
            else
            {
                //For options we always have the action as Command
                _targetItem = OptionItems.FirstOrDefault(p => p.Id == _inputitem.Id);
            }

            //Even after this, we are not able to get the target item, return.
            if (_targetItem == null) return;

            //Now, based on the target item, process the action.
            switch (_menuAction)
            {
                case MenuAction.RaiseCommand:
                    _executeCommand(_targetItem); //Raise the command and send parameter along with it.
                    break;
                case MenuAction.ShowContainerView:
                    _setFloatingCanvasVisibility(Visibility.Visible);
                    _setContainerView(_targetItem as MenuItem);
                    break;
                case MenuAction.ShowLocalView:
                    if (_mainContentHolder != null && ((MenuItem)_targetItem).View != null)
                    {
                        _mainContentHolder.Content = ((MenuItem)_targetItem).View;
                        _setFloatingCanvasVisibility(Visibility.Visible);
                    }
                    break;
            }
        }

        ICommand _getCommand(string cmdName)
        {
            ICommand resultCmd = null;
            object dataContext = this.DataContext;//FlexiMenu's datacontext.
            if (dataContext != null)
            {
                PropertyInfo commandPropertyInfo = dataContext
                    .GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(
                        p =>
                        typeof(ICommand).IsAssignableFrom(p.PropertyType) &&
                        string.Equals(p.Name, cmdName, StringComparison.Ordinal)
                    );

                if (commandPropertyInfo != null)
                {
                    resultCmd = (ICommand)commandPropertyInfo.GetValue(dataContext, null);
                }
            }
            return resultCmd;
        }
        void _executeCommand(CommandMenuItem item)
        {
            var dc = this.DataContext;
            //if command is null, try to check if we can process a new command with the help of command name.
            var _command = item.Command;
            if (_command == null && !string.IsNullOrWhiteSpace(item.CommandName))
            {
                //Try to get using the command name.
                _command = _getCommand(item.CommandName);
            }

            if (_command != null)
            {
                _command.Execute(item.CommandParameter);
            }
            else
            {
                //Set the error as message
                _message.Text = $@"Command is null. Cannot proceed.";
                _messageHolder.Visibility = Visibility.Visible;
                return;
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

                var _globalContainer = ContainerStore.Singleton.Controls;

                //PRIORITY 1 : If local container is present, then try to find the view. 
                if (_targetView == null && !item.IgnoreLocalContainer && LocalContainer != null)
                {
                    if (LocalContainer.ContainsKey(item.ContainerKey))
                    {
                        _targetView = LocalContainer.GenerateView(item.ContainerKey);
                    }
                }

                //PRIORIY 2 : If global container is present and also view is still empty, try finding the view.
                if (_targetView == null && !item.IgnoreGlobalContainer && _globalContainer != null)
                {
                    if (_globalContainer.ContainsKey(item.ContainerKey))
                    {
                        _targetView = _globalContainer.GenerateView(item.ContainerKey);
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
            switch (_param)
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
                case "MenuItems":
                    Alignment _menudirection = MenuItemsAlignment == Alignment.Left ? Alignment.Right : Alignment.Left;
                    this.SetCurrentValue(MenuItemsAlignmentProperty, _menudirection); //
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
                this.SetCurrentValue(HideHeaderRegionProperty, false);
            }
        }

        void _changeFloatingPanel()
        {
            if (_floatingPanel == null) return;

            _floatingPanel.Content = FloatingPanel;
        }

        static void FloatingPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlexiMenu flexiobj = d as FlexiMenu;
            if (flexiobj == null) return;
            flexiobj._changeFloatingPanel();
        }
        void _resetFloatingPanel(object sender, ExecutedRoutedEventArgs e)
        {
            if (_floatingPanel != null)
            {
                Canvas.SetLeft(_floatingPanel, 100.0);
                Canvas.SetTop(_floatingPanel, 100.0);
            }
        }
        void _changeFloatingPanelVisibility(object sender, ExecutedRoutedEventArgs e)
        {
           if (e.Parameter is bool _checked)
            {
                this.SetCurrentValue(IsFloatingPanelVisibleProperty, _checked);
            }
        }

        #endregion
    }
}
