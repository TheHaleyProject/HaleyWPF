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
using SysCtrls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Haley.WPF.Controls
{
    /// <summary>
    /// Fleximenu for both left/right and topbottom docking.s
    /// </summary>
    public class FlexiMenu : Control
    {
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
            _contextMenuShow = GetTemplateChild(UIEContextMenuShow) as SysCtrls.MenuItem;
            _contextMenuReposition = GetTemplateChild(UIEContextMenuReposition) as SysCtrls.MenuItem;

            _changeHeader();
            _changeFloatingPanel();

            if (DisableFloatingPanel)
            {
                if (_contextMenuReposition != null) _contextMenuReposition.Visibility = Visibility.Collapsed;
                if (_contextMenuShow != null) _contextMenuShow.Visibility = Visibility.Collapsed; //If we need to disable floating panel, then the context menu should not be shown.
            }

            //Set Welcome view if not null
            if (WelcomeView != null && !DisableWelcomeView)
            {
                _mainContentHolder.Content = WelcomeView;
                //If welcome view is active, then we should hide the floating panel
                _setFloatingCanvasVisibility(Visibility.Collapsed);
                _setupWelcomeViewCloseTimer();
            }
            else
            {
                _setFirstView(); //In case the welcome view is present and also it is not disabled, then we set the welcome view and then after timer runs out we set the first view.
            }
        }

        #endregion

        #region Attributes
        private const string UIEMainContentHolder = "PART_MainContentArea";
        private const string UIEMessageHolder = "PART_messageHolder";
        private const string UIEMessage = "PART_message";
        private const string UIEHeaderHolder = "PART_header";
        private const string UIEFloatingPanel = "PART_FloatingPanel";
        private const string UIEFloatingPanelCanvas = "PART_FloatingPanelHolderCanvas";
        private const string UIEContextMenuShow = "PART_MenuItem_ShowPanel";
        private const string UIEContextMenuReposition = "PART_MenuItem_RepositionPanel";

        private static double _headerRegionHeight = Convert.ToDouble(100);
        private static double _menuItemHeight = Convert.ToDouble(30);
        private static double _menuBarWidth = Convert.ToDouble(250);

        private DispatcherTimer _messageTimer;
        private bool _canvasSet = false;
        private bool _pauseMenuSelection = false;

        private static SolidColorBrush _defaultToggleButtonBg = Brushes.Transparent;

        #endregion

        #region UIElements
        private ContentControl _mainContentHolder;
        private FrameworkElement _messageHolder;
        private TextBlock _message;
        private ContentControl _headerHolder;
        private ContentControl _floatingPanel;
        private Canvas _floatingPanelHolderCanvas;
        private SysCtrls.MenuItem _contextMenuShow;
        private SysCtrls.MenuItem _contextMenuReposition;
        #endregion

        #region Constructors
        public FlexiMenu()
        {
            //For both menuitems and option items, we do not directly allow the items to raise the command. When the button is clicked, we raise an application command which will be handled in this class and corresponding action will be taken.
            this.MenuItems = new ObservableCollection<MenuItem>();
            this.OptionItems = new ObservableCollection<CommandMenuItem>();

            CommandBindings.Add(new CommandBinding(AdditionalCommands.ProcessMenuAction, _processMenuAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ProcessOptionsAction, _processOptionsAction));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Toggle, _toggleMenuBar));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, _closeMessage));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Reset, _resetFloatingPanel));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Show, _changeFloatingPanelVisibility));

            _addProxyResource();
            _messageTimer = new DispatcherTimer();
            _messageTimer.Interval = TimeSpan.FromSeconds(4);
            _messageTimer.Tick += _messageTimerTick;
        }


        static FlexiMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiMenu), new FrameworkPropertyMetadata(typeof(FlexiMenu)));
        }
        #endregion

        #region Properties

        public bool AutoCloseWelcomeView { get; set; }
        public double AutoCloseWelcomeViewTimeSpan { get; set; }
        public bool DisableFloatingPanel { get; set; }

        #region FloatingPanel

        public object FloatingPanel
        {
            get { return (object)GetValue(FloatingPanelProperty); }
            set { SetValue(FloatingPanelProperty, value); }
        }

        public static readonly DependencyProperty FloatingPanelProperty =
            DependencyProperty.Register("FloatingPanel", typeof(object), typeof(FlexiMenu), new FrameworkPropertyMetadata(null, propertyChangedCallback: FloatingPanelChanged));
        public bool IsFloatingPanelVisible
        {
            get { return (bool)GetValue(IsFloatingPanelVisibleProperty); }
            set { SetValue(IsFloatingPanelVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsFloatingPanelVisibleProperty =
            DependencyProperty.Register("IsFloatingPanelVisible", typeof(bool), typeof(FlexiMenu), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: IsFloatingPanelVisiblePropertyChanged));
        #endregion

        public bool DisableWelcomeView
        {
            get { return (bool)GetValue(DisableWelcomeViewProperty); }
            set { SetValue(DisableWelcomeViewProperty, value); }
        }

        public static readonly DependencyProperty DisableWelcomeViewProperty =
            DependencyProperty.Register(nameof(DisableWelcomeView), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public ImageSource ToggleIcon
        {
            get { return (ImageSource)GetValue(ToggleIconProperty); }
            set { SetValue(ToggleIconProperty, value); }
        }

        public static readonly DependencyProperty ToggleIconProperty =
            DependencyProperty.Register(nameof(ToggleIcon), typeof(ImageSource), typeof(FlexiMenu), new PropertyMetadata(null));

        public Brush SelectedMenuColor
        {
            get { return (Brush)GetValue(SelectedMenuColorProperty); }
            set { SetValue(SelectedMenuColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedMenuColorProperty =
            DependencyProperty.Register(nameof(SelectedMenuColor), typeof(Brush), typeof(FlexiMenu), new PropertyMetadata(null));

        public MenuItem ActiveMenu
        {
            get { return (MenuItem)GetValue(ActiveMenuProperty); }
            set { SetValue(ActiveMenuProperty, value); }
        }

        public static readonly DependencyProperty ActiveMenuProperty =
            DependencyProperty.Register(nameof(ActiveMenu), typeof(MenuItem), typeof(FlexiMenu), new FrameworkPropertyMetadata(null));

        public UserControl WelcomeView
        {
            get { return (UserControl)GetValue(WelcomeViewProperty); }
            set { SetValue(WelcomeViewProperty, value); }
        }

        public static readonly DependencyProperty WelcomeViewProperty =
            DependencyProperty.Register(nameof(WelcomeView), typeof(UserControl), typeof(FlexiMenu), new PropertyMetadata(null));

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return (ObservableCollection<MenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems), typeof(ObservableCollection<MenuItem>), typeof(FlexiMenu), new PropertyMetadata(null));

        public ObservableCollection<CommandMenuItem> OptionItems
        {
            get { return (ObservableCollection<CommandMenuItem>)GetValue(OptionItemsProperty); }
            set { SetValue(OptionItemsProperty, value); }
        }

        public static readonly DependencyProperty OptionItemsProperty =
            DependencyProperty.Register(nameof(OptionItems), typeof(ObservableCollection<CommandMenuItem>), typeof(FlexiMenu), new PropertyMetadata(null));

        public IControlContainer LocalContainer
        {
            get { return (IControlContainer)GetValue(LocalContainerProperty); }
            set { SetValue(LocalContainerProperty, value); }
        }

        public static readonly DependencyProperty LocalContainerProperty =
            DependencyProperty.Register(nameof(LocalContainer), typeof(IControlContainer), typeof(FlexiMenu), new PropertyMetadata(null));

        public string FootNote
        {
            get { return (string)GetValue(FootNoteProperty); }
            set { SetValue(FootNoteProperty, value); }
        }

        public static readonly DependencyProperty FootNoteProperty =
            DependencyProperty.Register(nameof(FootNote), typeof(string), typeof(FlexiMenu), new PropertyMetadata("Foot Note"));

        public bool IsMenuBarOpen
        {
            get { return (bool)GetValue(IsMenuBarOpenProperty); }
            set { SetValue(IsMenuBarOpenProperty, value); }
        }

        public static readonly DependencyProperty IsMenuBarOpenProperty =
            DependencyProperty.Register(nameof(IsMenuBarOpen), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(true));

        public DockLocation Location
        {
            get { return (DockLocation)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register(nameof(Location), typeof(DockLocation), typeof(FlexiMenu), new PropertyMetadata(DockLocation.Left));

        public Alignment MenuItemsAlignment
        {
            get { return (Alignment)GetValue(MenuItemsAlignmentProperty); }
            set { SetValue(MenuItemsAlignmentProperty, value); }
        }

        public static readonly DependencyProperty MenuItemsAlignmentProperty =
            DependencyProperty.Register(nameof(MenuItemsAlignment), typeof(Alignment), typeof(FlexiMenu), new PropertyMetadata(Alignment.Left));

        public string SelectedMenuId
        {
            get { return (string)GetValue(SelectedMenuIdProperty); }
            set { SetValue(SelectedMenuIdProperty, value); }
        }

        public static readonly DependencyProperty SelectedMenuIdProperty =
            DependencyProperty.Register("SelectedMenuId", typeof(string), typeof(FlexiMenu), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: SelectedIdChanged));

        public SolidColorBrush ToggleButtonBackground
        {
            get { return (SolidColorBrush)GetValue(ToggleButtonBackgroundProperty); }
            set { SetValue(ToggleButtonBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ToggleButtonBackgroundProperty =
            DependencyProperty.Register(nameof(ToggleButtonBackground), typeof(SolidColorBrush), typeof(FlexiMenu), new PropertyMetadata(_defaultToggleButtonBg));


        #region Heights
        public double HeaderRegionHeight
        {
            get { return (double)GetValue(HeaderRegionHeightProperty); }
            set { SetValue(HeaderRegionHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderRegionHeightProperty =
            DependencyProperty.Register(nameof(HeaderRegionHeight), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_headerRegionHeight));

        public double MenuItemHeight
        {
            get { return (double)GetValue(MenuItemHeightProperty); }
            set { SetValue(MenuItemHeightProperty, value); }
        }

        public static readonly DependencyProperty MenuItemHeightProperty =
            DependencyProperty.Register(nameof(MenuItemHeight), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_menuItemHeight));

        public double MenuBarWidth
        {
            get { return (double)GetValue(MenuBarWidthProperty); }
            set { SetValue(MenuBarWidthProperty, value); }
        }

        public static readonly DependencyProperty MenuBarWidthProperty =
            DependencyProperty.Register(nameof(MenuBarWidth), typeof(double), typeof(FlexiMenu), new PropertyMetadata(_menuBarWidth));

        #endregion

        #region Header Region
        public bool HideHeaderRegion
        {
            get { return (bool)GetValue(HideHeaderRegionProperty); }
            set { SetValue(HideHeaderRegionProperty, value); }
        }

        public static readonly DependencyProperty HideHeaderRegionProperty =
            DependencyProperty.Register(nameof(HideHeaderRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public bool HideMenuRegion
        {
            get { return (bool)GetValue(HideMenuRegionProperty); }
            set { SetValue(HideMenuRegionProperty, value); }
        }

        public static readonly DependencyProperty HideMenuRegionProperty =
            DependencyProperty.Register(nameof(HideMenuRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public bool HideOptionsRegion
        {
            get { return (bool)GetValue(HideOptionsRegionProperty); }
            set { SetValue(HideOptionsRegionProperty, value); }
        }

        public static readonly DependencyProperty HideOptionsRegionProperty =
            DependencyProperty.Register(nameof(HideOptionsRegion), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(false));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(FlexiMenu), new FrameworkPropertyMetadata(null, propertyChangedCallback: HeaderTemplatePropertyChanged));

        public bool ShowFooter
        {
            get { return (bool)GetValue(ShowFooterProperty); }
            set { SetValue(ShowFooterProperty, value); }
        }

        public static readonly DependencyProperty ShowFooterProperty =
            DependencyProperty.Register(nameof(ShowFooter), typeof(bool), typeof(FlexiMenu), new PropertyMetadata(true));
        #endregion
        #endregion

        #region Private Methods
        void _setupWelcomeViewCloseTimer()
        {
            if (AutoCloseWelcomeView)
            {
                //Validate timer duration
                if (AutoCloseWelcomeViewTimeSpan < 1 || AutoCloseWelcomeViewTimeSpan > 20 || double.IsNaN(AutoCloseWelcomeViewTimeSpan))
                {
                    AutoCloseWelcomeViewTimeSpan = 3.0;
                }

                //Setup timer
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(AutoCloseWelcomeViewTimeSpan);
                timer.Tick += timerTick;
                timer.Start();
            }
        }
        void timerTick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            _setFirstView();
        }

        void _messageTimerTick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            _closeMessage();
        }
        void _setFirstView()
        {
            //if we have any other view, then try to show it. If we have already moved to a different view, then in that case, do not process it (check: activemenu == null)
            if (MenuItems != null && MenuItems?.Count > 0 && ActiveMenu == null)
            {
                //Get the first menutime which is not a command.
                var _targetMenu = MenuItems.FirstOrDefault(p => p.Action != MenuAction.RaiseCommand);

                if (_targetMenu == null) return;
                _processAction(_targetMenu, true);
            }
        }
        private void _addProxyResource()
        {
            if (!this.Resources.Contains("proxy"))
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

        void _processMenuAction(object sender, ExecutedRoutedEventArgs e)
        {
            _processAction(e.Parameter as CommandMenuItem, true);
        }
        void _processOptionsAction(object sender, ExecutedRoutedEventArgs e)
        {
            _processAction(e.Parameter as CommandMenuItem, false);
        }
        void _processAction(CommandMenuItem _inputitem, bool isMenu)
        {
            //First close any previous message
            _closeMessage();

            //Send in the menu item as the parameter. Use that to fetch the target from the menuitem list or option item list and process the action.
            if (_inputitem == null) return;

            var _menuAction = MenuAction.RaiseCommand;
            CommandMenuItem _targetItem = null;

            if (isMenu)
            {
                _targetItem = MenuItems.FirstOrDefault(p => p.Id == _inputitem.Id);

                if (_targetItem is MenuItem _targetMenuItem)
                {
                    _menuAction = _targetMenuItem.Action;
                    if (_targetMenuItem.Action != MenuAction.RaiseCommand)
                    {
                        //Because usually, raising command will only be used for showing dialog or performing actions like signing out, exporting, printing etc. So, we need not highlight that
                        ActiveMenu = _targetMenuItem; //this is actualy set so that it can be used for highlighting.
                        if (SelectedMenuId != ActiveMenu.Id)
                        {
                            //Here, when we set, we don't need recursive effect.
                            _pauseMenuSelection = true;
                            SelectedMenuId = ActiveMenu.Id; //So, we set the selected id.
                            _pauseMenuSelection = false;
                        }
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
                _setMessage($@"Command is null. Cannot proceed.");
                return;
            }
        }
        void _setContainerView(MenuItem item)
        {
            try
            {
                //First ensure that the key is present.
                if (item.ContainerKey == null)
                {
                    //Set the error as message

                    _setMessage($@"Container key cannot be empty. Please assign a container key value.");
                    return;
                }

                //Check the menu item to find which container to use.
                UserControl _targetView = null;

                var _globalContainer = ContainerStore.Singleton.Controls;

                //PRIORITY 1 : If local container is present, then try to find the view. 
                if (_targetView == null && !item.IgnoreLocalContainer && LocalContainer != null)
                {
                    var _value = LocalContainer.ContainsKey(item.ContainerKey);
                    if (_value.HasValue && _value.Value)
                    {
                        _targetView = LocalContainer.GenerateViewFromKey(item.ContainerKey) as UserControl;
                    }
                }

                //PRIORIY 2 : If global container is present and also view is still empty, try finding the view.
                if (_targetView == null && !item.IgnoreGlobalContainer && _globalContainer != null)
                {
                    var _value = _globalContainer.ContainsKey(item.ContainerKey);
                    if (_value.HasValue && _value.Value)
                    {
                        _targetView = _globalContainer.GenerateViewFromKey(item.ContainerKey) as UserControl;
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
            _messageTimer.Stop(); //stop any running timer.
            //Set the error as message
            _message.Text = message;
            _messageHolder.Visibility = Visibility.Visible;
            _messageTimer.Start();
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
            //If disable then we don't set the content.
            if (_floatingPanel == null || DisableFloatingPanel) return;

            //Also try to get the canvas location for the first time.
            if (!_canvasSet)
            {
                _canvasSet = true;
                _setFloatingPanelCanvasPosition();
            }
            _floatingPanel.Content = FloatingPanel;
        }
        void _setFloatingPanelCanvasPosition()
        {
            if (_floatingPanel == null || DisableFloatingPanel) return;

            double left = double.NaN;
            double right = double.NaN;
            double bottom = double.NaN;
            double top = double.NaN;

            if (FloatingPanel != null && FloatingPanel is UIElement floating_uie)
            {
                left = Position.GetLeft(floating_uie);
                right = Position.GetRight(floating_uie);
                bottom = Position.GetBottom(floating_uie);
                top = Position.GetTop(floating_uie);
            }

            if (double.IsNaN(top) && double.IsNaN(bottom))
            {
                //we need atleast one value not to be nan
                top = 25.0;
            }

            if (double.IsNaN(left) && double.IsNaN(right))
            {
                left = 25.0;
            }

            if (!double.IsNaN(top))
            {
                Canvas.SetTop(_floatingPanel, top);
            }

            if (!double.IsNaN(bottom))
            {
                Canvas.SetBottom(_floatingPanel, bottom);
            }

            if (!double.IsNaN(right))
            {
                Canvas.SetRight(_floatingPanel, right);
            }

            if (!double.IsNaN(left))
            {
                Canvas.SetLeft(_floatingPanel, left);
            }
        }
        void _setFloatingCanvasVisibility(Visibility _visibility)
        {
            if (DisableFloatingPanel && _floatingPanelHolderCanvas != null)
            {
                _floatingPanelHolderCanvas.Visibility = Visibility.Collapsed;
                return;
            }

            if (_floatingPanelHolderCanvas != null)
            {
                _floatingPanelHolderCanvas.Visibility = _visibility;
            }
        }
        static void FloatingPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlexiMenu flexiobj = d as FlexiMenu;
            if (flexiobj == null) return;
            flexiobj._canvasSet = false; //Since we have received a new object, we need the canvas locations from here.
            flexiobj._changeFloatingPanel();
        }
        void _resetFloatingPanel(object sender, ExecutedRoutedEventArgs e)
        {
            _setFloatingPanelCanvasPosition();
        }
        void _changeFloatingPanelVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is bool _checked)
            {
                this.SetCurrentValue(IsFloatingPanelVisibleProperty, _checked);
            }
        }
        void _changeFloatingPanelContextMenu()
        {
            //based on the property, set the menuitem value.
            if (_contextMenuShow != null)
            {
                _contextMenuShow.IsChecked = IsFloatingPanelVisible;
            }
        }
        void _setviewtoNewId()
        {
            if (_pauseMenuSelection || SelectedMenuId == null) return;
            //based on the new id, change the view.

            if (ActiveMenu != null)
            {
                if (ActiveMenu?.Id.ToLower() == SelectedMenuId.ToLower()) return; //Because we already are in the correct view.
            }
            //Try to get the new id.
            var menutoSet = MenuItems.FirstOrDefault(p => p.Id.ToLower() == SelectedMenuId.ToLower());
            _processAction(menutoSet, true);
        }
        static void SelectedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //The id has changed.
            FlexiMenu flexiobj = d as FlexiMenu;
            if (flexiobj == null) return;
            flexiobj._setviewtoNewId();
        }
        static void IsFloatingPanelVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //The id has changed.
            FlexiMenu flexiobj = d as FlexiMenu;
            if (flexiobj == null) return;
            flexiobj._changeFloatingPanelContextMenu();
        }

        #endregion
    }
}
