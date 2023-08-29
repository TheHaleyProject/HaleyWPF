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
using Haley.Events;

namespace Haley.WPF.Controls
{
    /// <summary>
    /// ContainerViewer
    /// </summary>
    public class ContainerViewer : Control
    {
        public static readonly RoutedEvent ViewChangingEvent = EventManager.RegisterRoutedEvent(nameof(ViewChanging), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBar));

        public event RoutedEventHandler ViewChanging
        {
            add { AddHandler(ViewChangingEvent, value); }
            remove { RemoveHandler(ViewChangingEvent, value); }
        }

        #region Overridden Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainContentHolder = GetTemplateChild(UIEMainContentHolder) as ContentControl;
            _messageHolder = GetTemplateChild(UIEMessageHolder) as FrameworkElement;
            _message = GetTemplateChild(UIEMessage) as TextBlock;

            //Set Welcome view if not null
            if (WelcomeView != null && !DisableWelcomeView)
            {
                _mainContentHolder.Content = WelcomeView;
                //If welcome view is active, then we should hide the floating panel
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
        private const string UIEMessage = "PART_message";
        private const string UIEMessageHolder = "PART_messageHolder";
        private DispatcherTimer _messageTimer;
        private bool _welcomeInProgress = false;
        #endregion

        #region UIElements
        private ContentControl _mainContentHolder;
        private TextBlock _message;
        private FrameworkElement _messageHolder;
        #endregion

        #region Constructors
        static ContainerViewer() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContainerViewer), new FrameworkPropertyMetadata(typeof(ContainerViewer)));
        }

        public ContainerViewer() {
            //For both menuitems and option items, we do not directly allow the items to raise the command. When the button is clicked, we raise an application command which will be handled in this class and corresponding action will be taken.
            AutoCloseWelcomeView = true;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, _closeMessage));
            _messageTimer = new DispatcherTimer();
            _messageTimer.Interval = TimeSpan.FromSeconds(4);
            _messageTimer.Tick += _messageTimerTick;
        }
        #endregion

        #region Properties
        public static readonly DependencyProperty DisableWelcomeViewProperty =
            DependencyProperty.Register(nameof(DisableWelcomeView), typeof(bool), typeof(ContainerViewer), new PropertyMetadata(false));

        public static readonly DependencyProperty HideMenuDuringWelcomeProperty =
            DependencyProperty.Register("HideMenuDuringWelcome", typeof(bool), typeof(ContainerViewer), new PropertyMetadata(false));

        public static readonly DependencyProperty LocalContainerProperty =
            DependencyProperty.Register(nameof(LocalContainer), typeof(IControlContainer), typeof(ContainerViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty MenuVisibilityProperty =
            DependencyProperty.Register("MenuVisibility", typeof(Visibility), typeof(ContainerViewer), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty ViewKeyProperty =
            DependencyProperty.Register(nameof(ViewKey), typeof(object), typeof(ContainerViewer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnViewKeyPropertyChanged));

        public static readonly DependencyProperty WelcomeViewProperty =
            DependencyProperty.Register(nameof(WelcomeView), typeof(UserControl), typeof(ContainerViewer), new PropertyMetadata(null));

        public bool AutoCloseWelcomeView { get; set; }
        public double AutoCloseWelcomeViewTimeSpan { get; set; }
        public bool DisableWelcomeView {
            get { return (bool)GetValue(DisableWelcomeViewProperty); }
            set { SetValue(DisableWelcomeViewProperty, value); }
        }

        public bool HideMenuDuringWelcome {
            get { return (bool)GetValue(HideMenuDuringWelcomeProperty); }
            set { SetValue(HideMenuDuringWelcomeProperty, value); }
        }

        public bool IgnoreGlobalContainer { get; set; }
        public bool IgnoreLocalContainer { get; set; }
        public IControlContainer LocalContainer {
            get { return (IControlContainer)GetValue(LocalContainerProperty); }
            set { SetValue(LocalContainerProperty, value); }
        }

        public Visibility MenuVisibility {
            get { return (Visibility)GetValue(MenuVisibilityProperty); }
            set { SetValue(MenuVisibilityProperty, value); }
        }

        public object ViewKey {
            get { return (object)GetValue(ViewKeyProperty); }
            set { SetValue(ViewKeyProperty, value); }
        }

        public UserControl WelcomeView {
            get { return (UserControl)GetValue(WelcomeViewProperty); }
            set { SetValue(WelcomeViewProperty, value); }
        }
        private static void OnViewKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if (d is ContainerViewer cviewr)
            {
                cviewr._setContainerView(e.NewValue);//Calling directly.
            }
        }
        #endregion

        #region Private Methods
        void _closeMessage(object sender, ExecutedRoutedEventArgs e) {
            _closeMessage();
        }

        void _closeMessage() {
            //hide the message display.
            if (_messageHolder != null) {
                _messageHolder.Visibility = Visibility.Collapsed;
            }
        }

        void _messageTimerTick(object sender, EventArgs e) {
            ((DispatcherTimer)sender).Stop();
            _closeMessage();
        }

        void _setContainerView(object key) {
            try {
                if (_welcomeInProgress) return; //Don't do any task when the welcome is in progress
                //First ensure that the key is present.
                if (key == null) {
                    //Set the error as message

                    _setMessage($@"Container key cannot be empty. Please assign a container key value.");
                    SendEvent(false, "Container is empty");
                    return;
                }

                //Check the menu item to find which container to use.
                UserControl _targetView = null;
                string containerName = string.Empty;
                var _globalContainer = ContainerStore.Controls;

                //PRIORITY 1 : If local container is present, then try to find the view. 
                if (_targetView == null && !IgnoreLocalContainer && LocalContainer != null) {
                    var _value = LocalContainer.ContainsKey(key);
                    if (_value.HasValue && _value.Value) {
                        _targetView = LocalContainer.GenerateViewFromKey(key) as UserControl;
                        containerName = "Local Container";
                    }
                }

                //PRIORIY 2 : If global container is present and also view is still empty, try finding the view.
                if (_targetView == null && !IgnoreGlobalContainer && _globalContainer != null) {
                    var _value = _globalContainer.ContainsKey(key);
                    if (_value.HasValue && _value.Value) {
                        _targetView = _globalContainer.GenerateViewFromKey(key) as UserControl;
                        containerName = "Global Container";
                    }
                }

                //Somehow , if we manage to find the view, then set it.
                if (_mainContentHolder != null && _targetView != null) {
                    _mainContentHolder.Content = _targetView;
                    SendEvent(true, $@"View for key {key.AsString()} is obtained from {containerName} sucessfully.");
                } else {
                    var _message = $@"Unable to find any view for the container key - {key}. Check if key is correct. {Environment.NewLine} If you are using a separate ContainerFactory, then update the LocalContainer Value to fetch correct mapped value.";
                    _setMessage(_message);
                    SendEvent(false, _message);
                }
            } catch (Exception ex) {
                _setMessage(ex.ToString());
                SendEvent(false, ex.ToString());
            }
        }

        void _setFirstView() {
            if (ViewKey == null) {
                _mainContentHolder.Content = new TextBlock() { Text = "Container Key is empty. Please set a proper key to initiate the view.", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Foreground = Brushes.Gray, FontSize = 20 };
                return;
            }
            _setContainerView(ViewKey);
        }

        void _setMessage(string message) {
            if (_messageHolder == null) return;
            _messageTimer.Stop(); //stop any running timer.
            //Set the error as message
            _message.Text = message;
            _messageHolder.Visibility = Visibility.Visible;
            _messageTimer.Start();
        }

        void _setupWelcomeViewCloseTimer() {
            if (AutoCloseWelcomeView)
            {
                //Validate timer duration
                if (AutoCloseWelcomeViewTimeSpan < 1 || AutoCloseWelcomeViewTimeSpan > 20 || double.IsNaN(AutoCloseWelcomeViewTimeSpan))
                {
                    AutoCloseWelcomeViewTimeSpan = 3.0;
                }

                //Start the welcome Process (Setup the hide menu during welcome only when we are dealing with autoclose mode or else the welcome view will be visible and hte menu bar will be hidden and there will be no way to close it.
                if (HideMenuDuringWelcome) this.SetCurrentValue(MenuVisibilityProperty, Visibility.Collapsed);
                _welcomeInProgress = true;
                //Setup timer
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(AutoCloseWelcomeViewTimeSpan);
                timer.Tick += timerTick;
                timer.Start();
            }
        }
        private void SendEvent(bool status, string message) {
            if (!IsInitialized) return;
            RaiseEvent(new UIRoutedEventArgs<bool>(ViewChangingEvent, this) { Value = status, Message = message });
        }

        void timerTick(object sender, EventArgs e) {
            ((DispatcherTimer)sender).Stop();
            _welcomeInProgress = false;
            if (HideMenuDuringWelcome) this.SetCurrentValue(MenuVisibilityProperty, Visibility.Visible); //Which means that we have already changed the value. Now, we need to change it back
            _setFirstView();
        }
        #endregion
    }
}
