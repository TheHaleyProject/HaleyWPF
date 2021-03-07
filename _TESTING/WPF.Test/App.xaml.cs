using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //MainWindow2 _wndw2sd = new MainWindow2();


            MainWindow _wndwo = new MainWindow();
            _wndwo.ShowDialog();

            //MainWindow2 _wndw2 = new MainWindow2();
            //_wndw2.ShowDialog();

            //WndwPagination _wndw3 = new WndwPagination();
            //_wndw3.ShowDialog();


        }
    }
}
