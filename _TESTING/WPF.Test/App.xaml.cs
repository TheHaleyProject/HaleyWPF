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

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LangUtils.Register("WPF.Test.Properties.Resources");
            LangUtils.ChangeCulture("ta");
            //TempMethod();
            ContainerRegistrations(); //For notification test

            var _basewndw = new BaseWindow();
            _basewndw.ShowDialog();
        }

        private void ContainerRegistrations()
        {
            var _key = ContainerStore.Singleton.Controls.Register<MainVM, LocalView2>("localdemokey", mode: RegisterMode.Transient);
            var _key2 = ContainerStore.Singleton.Controls.Register<MainVM, VanakkamView>("testkitty2", mode: RegisterMode.Transient);
        }
    }
}
