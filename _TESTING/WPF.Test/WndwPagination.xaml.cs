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
using System.Collections.ObjectModel;
using Haley.Events;
using System.Collections;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for WndwPagination.xaml
    /// </summary>
    public partial class WndwPagination : Window
    {
        public WndwPagination()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
        }

        private void CollectionSelector_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            //UIRoutedEventArgs<IEnumerable> handler = (UIRoutedEventArgs<IEnumerable>)e;
            //var vm = this.DataContext as MainVM;
            //var persons = new ObservableCollection<Person>();
            //foreach (var item in handler.value)
            //{
            //    persons.Add((Person)item);
            //}
        }
    }
}
