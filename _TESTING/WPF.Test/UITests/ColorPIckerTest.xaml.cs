using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using Haley.WPF.Controls;
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
    /// Interaction logic for ColorPIckerTest.xaml
    /// </summary>
    public partial class ColorPIckerTest : PlainWindow
    {
        public ColorPIckerTest()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
        }
    }
}
