using System.Windows;

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
