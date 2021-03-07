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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.Utils;
using Haley.Models;
using Haley.Enums;
using System.Windows.Threading;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool is_dark_theme = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleButton_OnClicked(object sender, RoutedEventArgs e)
        {

        }

        private void PlainButton_Click(object sender, RoutedEventArgs e)
        {
            int _red = string.IsNullOrEmpty(redValue.Text) ?  0 : int.Parse(redValue.Text);
            int _green = string.IsNullOrEmpty(greenValue.Text) ? 0 : int.Parse(greenValue.Text);
            int _blue = string.IsNullOrEmpty(blueValue.Text) ? 0 : int.Parse(blueValue.Text);

            var _source = imgeChanger.DefaultImage;

            var _imageinfo = ImageUtils.getImageInfo(_source);
            var _newsource = ImageUtils.changeImageColor(_imageinfo, _red, _green, _blue);
            imgeChanger.DefaultImage = _newsource;

        }

        private void PlainButton_Click_1(object sender, RoutedEventArgs e)
        {
            /*_changeTheme();*/ //DIRECTLY CHANGE.
            GlobalData.Singleton.current_theme = _getTheme();
        //Old theme will be set by themeloader.
        }

        private Theme _getTheme()
        {

            Theme activeTheme = new Theme() { sender = this };
            //Switch theme.
            switch (is_dark_theme)
            {
                case true:
                    is_dark_theme = false;
                    activeTheme.base_dictionary_name = "DicRD";
                    activeTheme.theme_to_replace = "ThemeDark";
                    activeTheme.theme_PackURI = $@"pack://application:,,,/WPF.Test;component/Resources/ThemeLight.xaml";
                    break;
                case false:
                    is_dark_theme = true;
                    activeTheme.base_dictionary_name = "DicRD";
                    activeTheme.theme_to_replace = "ThemeLight";
                    activeTheme.theme_PackURI = $@"pack://application:,,,/WPF.Test;component/Resources/ThemeDark.xaml";
                    break;
            }
            return activeTheme;
            //this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(()=>
            //{
            //    ThemeLoader.changeTheme(this, activeTheme.theme_PackURI, activeTheme.theme_to_replace, activeTheme.base_dictionary_name);
            //}));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var theme = _getTheme();
            ThemeLoader.changeTheme(this, theme.theme_PackURI, theme.theme_to_replace, theme.base_dictionary_name);
        }
    }
}
