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

            FlexiMenuTest wndwFlexiTst = new FlexiMenuTest();
            wndwFlexiTst.ShowDialog();

            //RibbonTest wnddgtest = new RibbonTest();
            //    wnddgtest.ShowDialog();

        }
    }
}
