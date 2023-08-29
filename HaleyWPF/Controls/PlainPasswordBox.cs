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
using Haley.Enums;

namespace Haley.WPF.Controls
{
    /// <summary>
    /// Sealed class that cannot be inherited.
    /// </summary>
    public sealed class PlainPasswordBox : PlainTextBox
    {
        public static readonly DependencyProperty DefaultImageColorProperty =
            DependencyProperty.Register(nameof(DefaultImageColor), typeof(SolidColorBrush), typeof(PlainPasswordBox), new PropertyMetadata(null));

        public static readonly DependencyProperty EyeIconProperty =
            DependencyProperty.Register("EyeIcon", typeof(ImageSource), typeof(PlainPasswordBox), new PropertyMetadata(IconFinder.GetIcon(IconKind.eye.ToString(), IconSourceKey.Default)));

        public static readonly DependencyProperty HasPasswordProperty =
                    DependencyProperty.Register(nameof(HasPassword), typeof(bool), typeof(PlainPasswordBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.NotDataBindable));

        public static readonly DependencyProperty HoverImageColorProperty =
            DependencyProperty.Register(nameof(HoverImageColor), typeof(SolidColorBrush), typeof(PlainPasswordBox), new PropertyMetadata(null));

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(nameof(PasswordChar), typeof(char), typeof(PlainPasswordBox), new PropertyMetadata('*'));

        public static readonly DependencyProperty PasswordValueProperty =
            DependencyProperty.Register("PasswordValue", typeof(string), typeof(PlainPasswordBox), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordValueChanged));

        private const string UIEMainPWDbox = "PART_mainpwdbox";

        private const string UIEPWDDisplay = "PART_pwdDisplay";

        private const string UIEPWDEye = "PART_Eye";
        bool _disablePasswordSync = false;

        private PasswordBox _pboxMain;

        private FrameworkElement _pwdEye;

        private TextBlock _tblckDisplay;

        static PlainPasswordBox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainPasswordBox), new FrameworkPropertyMetadata(typeof(PlainPasswordBox)));
        }

        public PlainPasswordBox() {
            EnablePasswordViewing = true;
            //Plain password box is inheriting from PlainTextbox which already has a commandbinding for Show.
            SubscribeSuggestions = false; //Do not register popup events.
            CommandBindings.Remove(ShowPopupBinding);
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Show, Execute_show));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Hide, Execute_hide));
        }

        public SolidColorBrush DefaultImageColor {
            get { return (SolidColorBrush)GetValue(DefaultImageColorProperty); }
            set { SetValue(DefaultImageColorProperty, value); }
        }

        public bool EnablePasswordViewing { get; set; }

        public ImageSource EyeIcon {
            get { return (ImageSource)GetValue(EyeIconProperty); }
            set { SetValue(EyeIconProperty, value); }
        }

        [Bindable(false)]
        public bool HasPassword {
            get { return (bool)GetValue(HasPasswordProperty); }
            set { SetValue(HasPasswordProperty, value); }
        }

        public SolidColorBrush HoverImageColor {
            get { return (SolidColorBrush)GetValue(HoverImageColorProperty); }
            set { SetValue(HoverImageColorProperty, value); }
        }

        public char PasswordChar {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        public string PasswordValue {
            get { return (string)GetValue(PasswordValueProperty); }
            set { SetValue(PasswordValueProperty, value); }
        }

        public SecureString SecurePassword { get; private set; }

        public string GetPassword() {
            if (SecurePassword == null) return null;
            return HashUtils.UnWrap(SecurePassword);
        }

        public override void OnApplyTemplate() {
            DisplaySuggestions = false; //Before applying template disable the suggestions.
            base.OnApplyTemplate();
            _pboxMain = GetTemplateChild(UIEMainPWDbox) as PasswordBox;
            _tblckDisplay = GetTemplateChild(UIEPWDDisplay) as TextBlock;
            _pwdEye = GetTemplateChild(UIEPWDEye) as FrameworkElement;

            if (!EnablePasswordViewing && _pwdEye != null) _pwdEye.Visibility = Visibility.Collapsed;
            if (_pboxMain != null) {
                _pboxMain.PasswordChanged -= _pboxMain_PasswordChanged;
                _pboxMain.PasswordChanged += _pboxMain_PasswordChanged;
            }
        }

        private static void OnPasswordValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            //if it is same value dont' do anything. else update.
            if (!(d is PlainPasswordBox pbox) || pbox._disablePasswordSync) return;
            pbox._pboxMain.Password = e.NewValue as string;
        }
        #region Click Event
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(nameof(PasswordChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlainPasswordBox));

        public event RoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }
        #endregion
        private void _pboxMain_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _disablePasswordSync = true; //this will allow the password to be set from the source or viewmodel
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
            //Also, update the value in the PasswordBoxValue
            this.SetCurrentValue(PasswordValueProperty, GetPassword());
            _disablePasswordSync = false;
        }
        void Execute_hide(object sender, ExecutedRoutedEventArgs e) {
            _tblckDisplay.Text = string.Empty;
            _pboxMain.Visibility = Visibility.Visible;
            _tblckDisplay.Visibility = Visibility.Collapsed;
        }

        void Execute_show(object sender, ExecutedRoutedEventArgs e) {
            _tblckDisplay.Text = _pboxMain.Password;
            _pboxMain.Visibility = Visibility.Collapsed;
            _tblckDisplay.Visibility = Visibility.Visible;
        }
    }
}
