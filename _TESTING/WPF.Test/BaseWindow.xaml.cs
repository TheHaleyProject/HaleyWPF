using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System.Windows;
using WPF.Test.Controls;
using Haley.WPF.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;
using System;
using WPF.Test.UITests;
using System.Windows.Controls;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        IDialogServiceEx _ds;
        public BaseWindow()
        {
            _ds = ContainerStore.Singleton.DI.Resolve<IDialogServiceEx>();
            InitializeComponent();
            //var _mainvm = new MainVM();
            //_mainvm.SetCurrentView(nameof(BaseWindow));
            //this.DataContext = _mainvm;
        }

        private void btnCardTest_Click(object sender, RoutedEventArgs e)
        {
            CardTest cardtestnew = new CardTest();
            cardtestnew.ShowDialog();
        }

        private void btnBadgeTest_Click(object sender, RoutedEventArgs e)
        {
            BadgeTesting badgetst = new BadgeTesting();
            badgetst.ShowDialog();
        }

        private void btnColorPickerTest_Click(object sender, RoutedEventArgs e)
        {
            ColorPIckerTest cpkrTest = new ColorPIckerTest();
            cpkrTest.ShowDialog();
        }

        private void btnExpanderTest_Click(object sender, RoutedEventArgs e)
        {
            ExpanderTest _wndw2 = new ExpanderTest();
            _wndw2.ShowDialog();
        }

        private void btnFlexiMenuTest_Click(object sender, RoutedEventArgs e)
        {
            NewFlexiMenu flexiNewMenuTest = new NewFlexiMenu();
            flexiNewMenuTest.ShowDialog();
        }

        private void btnNotificationTest_Click(object sender, RoutedEventArgs e)
        {
            NotificationTest _wndwNotification = new NotificationTest();
            _wndwNotification.ShowDialog();
            MainVM _vm = new MainVM();
            _vm.something = new ObservableCollection<Person>();

            _vm.something.Add(new Person("Johnson and Johnson", 45));
            _vm.something.Add(new Person("Lux", 35));
            _vm.something.Add(new Person("Medimix", 15));
            _vm.something.Add(new Person("Cinthol", 145));
            _vm.something.Add(new Person("Lifebouy", 63));

            var _jesh = _ds.ShowContainerView<LocalView2>("Now new view", _vm);
            //notificationTest();
        }

        private void btnScrollViewerTest_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewerTest wndwscroll = new ScrollViewerTest();
            wndwscroll.ShowDialog();
        }

        private void btnThemeTest_Click(object sender, RoutedEventArgs e)
        {
            ThemeTest wndw = new ThemeTest();
            wndw.ShowDialog();
        }
        private void notificationTest()
        {
           
            _ds.ShowDialog("Test", "hello world");
            _ds.ShowDialog("What", "this is to hide the icon", hideIcon: true);

            var _baseAccent = (SolidColorBrush)new BrushConverter().ConvertFromString("Purple");

            _ds.AccentColor = _baseAccent;
            _ds.ShowDialog("MyGoodness", "Warning, you are going to die at 90", NotificationIcon.Warning);
            var _data = _ds.ShowDialog("Confirm", "Do you know that you are an idiot", mode: DialogMode.Confirmation);
            var _data2 = _ds.ShowDialog("Name", "please write your name ", NotificationIcon.Error, DialogMode.GetInput);
            var _res = _ds.ShowDialog("Send toast", "Should send a toast?", NotificationIcon.Warning, DialogMode.Confirmation);
            if (_res.DialogResult.Value)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { _ds.SendToast("Proceed", "User Requested to send toast", NotificationIcon.Success); }), DispatcherPriority.Send);

            }
            else
            {
                _ds.SendToast("Abort", "User denied sending toast", NotificationIcon.Error);
            }

            Random _rndm = new Random();
            Type _nIcon = typeof(NotificationIcon);
            var _values = _nIcon.GetEnumValues();

            for (int i = 0; i < 30; i++)
            {
                var _iconInt = _rndm.Next(_values.Length);
                NotificationIcon _icon = (NotificationIcon)_values.GetValue(_iconInt);
                _ds.SendToast("Test", $@"Current number is {i}", _icon);
            }

            var res = _ds.ShowContainerView<LocalView2>("Test View");
            var vmobj = res.ContainerViewModel;


            MainVM _vm = new MainVM();
            _vm.something = new ObservableCollection<Person>();

            _vm.something.Add(new Person("Johnson and Johnson", 45));
            _vm.something.Add(new Person("Lux", 35));
            _vm.something.Add(new Person("Medimix", 15));
            _vm.something.Add(new Person("Cinthol", 145));
            _vm.something.Add(new Person("Lifebouy", 63));

            var _jesh = _ds.ShowContainerView<LocalView2>("Now new view", _vm);
            var vmjesh = _jesh.ContainerViewModel;
            _ds.SendToast("Processing Error2", "Error while doing this");
            _ds.SendToast("Processing Error 3", "Error while doing this");
            _ds.SendToast("Processing Error 3", "Error while doing this");
            _ds.SendToast("Processing Erro 5 r", "Error while doing this");
            _ds.SendToast("Processing Error 6", "Error while doing this");
        }

        private void btnPaginationTest_Click(object sender, RoutedEventArgs e)
        {
            WndwPagination _wndw3 = new WndwPagination();
            _wndw3.ShowDialog();
        }

        private void btnCommonTest_Click(object sender, RoutedEventArgs e)
        {
            MainWindow _wndwo = new MainWindow();
            _wndwo.ShowDialog();
        }

        private void btnPlainWindowTest_Click(object sender, RoutedEventArgs e)
        {
            PlainWindowTest _wndw = new PlainWindowTest();
            _wndw.ShowDialog();
        }

        private void btnContainerViewerTest_Click(object sender, RoutedEventArgs e)
        {
            ContainerViewerTest _wndw = new ContainerViewerTest();
            _wndw.ShowDialog();
        }
    }
}
