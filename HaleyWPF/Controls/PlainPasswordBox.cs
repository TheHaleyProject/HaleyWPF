using Haley.Utils;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Haley.Events;

namespace Haley.WPF.Controls
{
    /// <summary>
    /// Sealed class that cannot be inherited.
    /// </summary>
    public sealed class PlainPasswordBox : PlainTextBox
    {
        private const string UIEMainPWDbox = "PART_mainpwdbox";
        private const string UIEPWDDisplay = "PART_pwdDisplay";
        private const string UIEPWDEye = "PART_Eye";

        private PasswordBox _pboxMain;
        private TextBlock _tblckDisplay;
        private FrameworkElement _pwdEye;
        public bool EnablePasswordViewing { get; set; }
        #region Click Event
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(nameof(PasswordChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlainPasswordBox));

        public event RoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }
        #endregion


        static PlainPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainPasswordBox), new FrameworkPropertyMetadata(typeof(PlainPasswordBox)));
        }
        public PlainPasswordBox()
        {
            EnablePasswordViewing = true;
            //Plain password box is inheriting from PlainTextbox which already has a commandbinding for Show.
            SubscribeSuggestions = false; //Do not register popup events.
            CommandBindings.Remove(ShowPopupBinding);
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Show, Execute_show));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Hide, Execute_hide));
        }

        public override void OnApplyTemplate()
        {
            DisplaySuggestions = false; //Before applying template disable the suggestions.
            base.OnApplyTemplate();
            _pboxMain = GetTemplateChild(UIEMainPWDbox) as PasswordBox;
            _tblckDisplay = GetTemplateChild(UIEPWDDisplay) as TextBlock;
            _pwdEye = GetTemplateChild(UIEPWDEye) as FrameworkElement;

            if (!EnablePasswordViewing && _pwdEye != null) _pwdEye.Visibility = Visibility.Collapsed;
            if (_pboxMain != null)
            {
                _pboxMain.PasswordChanged -= _pboxMain_PasswordChanged;
                _pboxMain.PasswordChanged += _pboxMain_PasswordChanged;
            }
        }

        private void _pboxMain_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Whenever the password changes, just save that as secure password here.
            SecurePassword = _pboxMain.SecurePassword;

            //Check if password is not empty and set haspassword property
            if (string.IsNullOrEmpty(_pboxMain.Password))
            {
                HasPassword = false;
            }
            else
            {
                HasPassword = true;
            }
            RaiseEvent(new UIRoutedEventArgs<SecureString>(PasswordChangedEvent, this) {Value = SecurePassword }); //Raise the event and send this object. Then user can get the password from it.
        }
        void Execute_show(object sender, ExecutedRoutedEventArgs e)
        {
            _tblckDisplay.Text = _pboxMain.Password;
            _pboxMain.Visibility = Visibility.Collapsed;
            _tblckDisplay.Visibility = Visibility.Visible;
        }
        void Execute_hide(object sender, ExecutedRoutedEventArgs e)
        {
            _tblckDisplay.Text = string.Empty;
            _pboxMain.Visibility = Visibility.Visible;
            _tblckDisplay.Visibility = Visibility.Collapsed;
        }

        public SecureString SecurePassword { get; private set; }

        [Bindable(false)]
        public bool HasPassword
        {
            get { return (bool)GetValue(HasPasswordProperty); }
            set { SetValue(HasPasswordProperty, value); }
        }

        public static readonly DependencyProperty HasPasswordProperty =
            DependencyProperty.Register(nameof(HasPassword), typeof(bool), typeof(PlainPasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.NotDataBindable));

        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(nameof(PasswordChar), typeof(char), typeof(PlainPasswordBox), new PropertyMetadata('*'));


        public string GetPassword()
        {
            if (SecurePassword == null) return null;
            return HashUtils.UnWrap(SecurePassword);
        }

        public SolidColorBrush HoverImageColor
        {
            get { return (SolidColorBrush)GetValue(HoverImageColorProperty); }
            set { SetValue(HoverImageColorProperty, value); }
        }

        public static readonly DependencyProperty HoverImageColorProperty =
            DependencyProperty.Register(nameof(HoverImageColor), typeof(SolidColorBrush), typeof(PlainPasswordBox), new PropertyMetadata(null));

        public SolidColorBrush DefaultImageColor
        {
            get { return (SolidColorBrush)GetValue(DefaultImageColorProperty); }
            set { SetValue(DefaultImageColorProperty, value); }
        }

        public static readonly DependencyProperty DefaultImageColorProperty =
            DependencyProperty.Register(nameof(DefaultImageColor), typeof(SolidColorBrush), typeof(PlainPasswordBox), new PropertyMetadata(null));

       
    }
}
