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
using Haley.Services;

namespace WPF.Test.External
{
    /// <summary>
    /// Interaction logic for WndwExternal.xaml
    /// </summary>
    public partial class WndwExternal : Window
    {
        IThemeService _ts;
        public WndwExternal()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ts = ThemeService.Singleton;
            switch (_ts.ActiveTheme)
            {
                case null:
                case "Theme1":
                    //If null, assume we are already at startuptheme.
                    _ts.ChangeTheme("Theme2");
                    break;
                case "Theme2":
                    _ts.ChangeTheme("Theme3");
                    break;
                case "Theme3":
                    _ts.ChangeTheme("Theme1");
                    break;
            }
        }
    }
}
