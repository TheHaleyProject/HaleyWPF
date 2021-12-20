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
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private UniAxisPickerAdorner _hueAdorner;
        private BiAxisPickerAdorner _slAdorner;

        public TestWindow()
        {
            InitializeComponent();

            //Initate the Adorners
            _hueAdorner = new UniAxisPickerAdorner(HueRectangle);
            _slAdorner = new BiAxisPickerAdorner(SLRectangle);

            //Hooks the element's events with adorners.
            PickerAdornerEventsHook.Hook(_hueAdorner); //In case we need to remove or get hook or unsubcribe, we can get the hook id from the return value of the method.
            PickerAdornerEventsHook.Hook(_slAdorner);
        }
    }
}
