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
    /// Interaction logic for BadgeTesting.xaml
    /// </summary>
    public partial class BadgeTesting : Window
    {
        public BadgeTesting()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
        }
    }
}
