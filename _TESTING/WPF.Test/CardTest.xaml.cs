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

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for CardTest.xaml
    /// </summary>
    public partial class CardTest : Window
    {
        public CardTest()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
        }

        private void PlainCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
