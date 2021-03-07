using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Haley.Abstractions;
using Haley.MVVM;
using DevelopmentWPF.Controls;
using DevelopmentWPF.ViewModels;
using System.Windows.Data;
using System.Globalization;
using System.Threading;
using Haley.Enums;
using Haley.IOC;

namespace DevelopmentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var _di = ContainerStore.Singleton.DI;
                var _wndw = ContainerStore.Singleton.windows;
                var _ctrl = ContainerStore.Singleton.controls;
                MainWindow ms = new MainWindow();
                _wndw.register<CoreVM, MainWindow>();
                _wndw.register<CoreVM, MainWindow>(use_vm_as_key:false);
                _ctrl.register<VMSubMain, ctrl02>(TestApp.control02,mode:RegisterMode.Transient);
                _ctrl.register<VMMain, ctrl01>(TestApp.control01);
                _ctrl.register<VMSubMain, ctrl03>();

                //var dservice = _di.Resolve<IDialogService>();
                //bool flag = dservice.send("Test", "Hello");
                //flag = dservice.send("Test", "Hello",DialogMode.Confirmation);
                //flag = dservice.send("Test", "Hello", DialogMode.GetInput);
                //string user_message;
                //flag = dservice.receive("Test", "Hello",out user_message);

                
                MainWindow _newwindow = new MainWindow();
                _newwindow.ShowDialog();
                ContainerStore.Singleton.windows.showDialog<CoreVM>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public enum TestApp
    {
        none,
        control01,
        control02,
        control03
    }

    public enum TestApp02
    {
        Control4,
        control5
    }

    
}
