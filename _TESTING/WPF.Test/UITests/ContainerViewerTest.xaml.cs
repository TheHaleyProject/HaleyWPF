using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.Test;

namespace WPF.Test.UITests
{
    /// <summary>
    /// Interaction logic for ContainerViewerTest.xaml
    /// </summary>
    public partial class ContainerViewerTest : Window
    {
        public ContainerViewerTest()
        {
            InitializeComponent();
            var _mainvm = new MainVM();
            this.DataContext = _mainvm;
        }

        private void ContainerViewer_ViewChanging(object sender, RoutedEventArgs e)
        {

        }
    }
}
