using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System.Windows;
using WPF.Test.Controls;
using Haley.WPF.BaseControls;
using System.Windows.Media;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //ContainerRegistrations();
            //ExpanderTest _wndw2sd = new ExpanderTest();

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
            notificationTest();
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

            _ds.SendToast("Processing Error 1", "Error while doing this");
            _ds.SendToast("Processing Error asdfe", "Errorasdfa  while doing this asdfaewadasdf");
            //_ds.SendToast("Processing Error2", "Error while doing this");
            //_ds.SendToast("Processing Error 3", "Error while doing this");
            //_ds.SendToast("Processing Error 3", "Error while doing this");
            //_ds.SendToast("Processing Erro 5 r", "Error while doing this");
            //_ds.SendToast("Processing Error 6", "Error while doing this");
        }

        private void ContainerRegistrations()
        {
            var _key = ContainerStore.Singleton.controls.register<MainVM, LocalView2>(key: "localDemoKey", mode: RegisterMode.Transient);
        }
    }
}
