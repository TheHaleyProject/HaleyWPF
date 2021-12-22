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
using System.Windows.Media;
using System.Windows.Threading;
using System;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //ContainerRegistrations(); //For notification test
            ////ExpanderTest _wndw2sd = new ExpanderTest();

            //var mw = Application.Current.MainWindow;
            //if (mw == null)
            //{
            //    //First invoke.
            //    mw = new Window(); //Dummy initiation
            //}

            ColorPIckerTest cpkrTest = new ColorPIckerTest();
            cpkrTest.ShowDialog();


            //MainWindow _wndwo = new MainWindow();
            //_wndwo.ShowDialog();

            //ExpanderTest _wndw2 = new ExpanderTest();
            //_wndw2.ShowDialog();

            //WndwPagination _wndw3 = new WndwPagination();
            //_wndw3.ShowDialog();

            //ThemeTest wndw = new ThemeTest();
            //wndw.ShowDialog();

            //ScrollViewerTest wndwscroll = new ScrollViewerTest();
            //wndwscroll.ShowDialog();

            //FlexiMenuTest wndwFlexiTst = new FlexiMenuTest();
            //wndwFlexiTst.ShowDialog();

            //RibbonTest wnddgtest = new RibbonTest();
            //wnddgtest.ShowDialog();

            //NewFlexiMenu flexiNewMenuTest = new NewFlexiMenu();
            //flexiNewMenuTest.ShowDialog();

            //CardTest cardtestnew = new CardTest();
            //cardtestnew.ShowDialog();

            //BadgeTesting badgetst = new BadgeTesting();
            //badgetst.ShowDialog();

            //notificationTest();

            //var mw = Application.Current.MainWindow;
            //if (mw == null)
            //{
            //    //First invoke.
            //    mw = new Window(); //Dummy initiation
            //}

            //NotificationTest _wndwNotification = new NotificationTest();
            //_wndwNotification.ShowDialog();
        }


        private void notificationTest()
        {
            var mw = Application.Current.MainWindow;
            if (mw == null)
            {
                //First invoke.
                mw = new Window(); //Dummy initiation
            }
            IDialogService _ds = ContainerStore.Singleton.DI.Resolve<IDialogService>();
            //_ds.ShowDialog("Test", "hello world");
            //_ds.ShowDialog("What", "this is to hide the icon", hideIcon: true);

            //var _baseAccent = (SolidColorBrush)new BrushConverter().ConvertFromString("Purple");

            //_ds.ChangeAccentColors(_baseAccent);
            //_ds.ShowDialog("MyGoodness", "Warning, you are going to die at 90", NotificationIcon.Warning);
            //var _data = _ds.ShowDialog("Confirm", "Do you know that you are an idiot", mode: DialogMode.Confirmation);
            //var _data2 = _ds.ShowDialog("Name", "please write your name ", NotificationIcon.Error, DialogMode.GetInput);
            //var _res =_ds.ShowDialog("Send toast", "Should send a toast?", NotificationIcon.Warning, DialogMode.Confirmation);
            //if (_res.DialogResult.Value)
            //{
            //    Application.Current.Dispatcher.BeginInvoke(new Action(() => { _ds.SendToast("Proceed", "User Requested to send toast", NotificationIcon.Success); }), DispatcherPriority.Send);

            //}
            //else
            //{
            //    _ds.SendToast("Abort", "User denied sending toast",NotificationIcon.Error);
            //}

            Random _rndm = new Random();
            Type _nIcon = typeof(NotificationIcon);
            var _values =  _nIcon.GetEnumValues();

            for (int i = 0; i < 30; i++)
            {
                var _iconInt = _rndm.Next(_values.Length);
                NotificationIcon _icon  = (NotificationIcon)_values.GetValue(_iconInt);
                _ds.SendToast("Test", $@"Current number is {i}", _icon);
            }

            var res = _ds.ShowContainerView<LocalView2>("Test View");
            var vmobj = res.ContainerViewModel;


            MainVM _vm = new MainVM();
            _vm.something = new ObservableCollection<Person>();

            _vm.something.Add(new Person("Johnson and Johnson", 45));
            _vm.something.Add(new Person("Lux", 35));
            _vm.something.Add(new Person("Medimix",15));
            _vm.something.Add(new Person("Cinthol", 145));
            _vm.something.Add(new Person("Lifebouy", 63));

            var _jesh = _ds.ShowContainerView<LocalView2>("Now new view", _vm);
            var vmjesh = _jesh.ContainerViewModel;
                //_ds.SendToast("Processing Error2", "Error while doing this");
            //_ds.SendToast("Processing Error 3", "Error while doing this");
            //_ds.SendToast("Processing Error 3", "Error while doing this");
            //_ds.SendToast("Processing Erro 5 r", "Error while doing this");
            //_ds.SendToast("Processing Error 6", "Error while doing this");
        }

        private void ContainerRegistrations()
        {
            var _key = ContainerStore.Singleton.Controls.Register<MainVM, LocalView2>("localdemokey", mode: RegisterMode.Transient);
        }
    }
}
