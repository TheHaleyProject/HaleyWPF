using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
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
    /// Interaction logic for NotificationTest.xaml
    /// </summary>
    public partial class NotificationTest : Window
    {
        private IDialogServiceEx ds;
        public NotificationTest()
        {
            ds =ContainerStore.DI.Resolve<IDialogServiceEx>();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ds.TopMost = false;
            ds.ShowInTaskBar = chkbxShwInTaskBar.IsChecked.HasValue ? chkbxShwInTaskBar.IsChecked.Value : false;
            ds.StartupLocation = WindowStartupLocation.CenterOwner;
            ds.ShowDialog("Test", tbxMain.Text, blurOtherWindows: chkbx.IsChecked.Value);
        }
    }
}
