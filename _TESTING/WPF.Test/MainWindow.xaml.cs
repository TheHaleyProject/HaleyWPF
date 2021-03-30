using Haley.Models;
using Haley.Utils;
using System.Windows;
using System;
using Haley.Enums;

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
            int _red = string.IsNullOrEmpty(redValue.Text) ? 0 : int.Parse(redValue.Text);
            int _green = string.IsNullOrEmpty(greenValue.Text) ? 0 : int.Parse(greenValue.Text);
            int _blue = string.IsNullOrEmpty(blueValue.Text) ? 0 : int.Parse(blueValue.Text);

            var _source = Haley.Models.Icon.GetDefault(imgeChanger);

            var _imageinfo = ImageUtils.getImageInfo(_source);
            var _newsource = ImageUtils.changeImageColor(_imageinfo, _red, _green, _blue);
            Haley.Models.Icon.SetDefault(imgeChanger, _newsource);

        }

        private void PlainButton_Click_1(object sender, RoutedEventArgs e)
        {
            /*_changeTheme();*/ //DIRECTLY CHANGE.
            ThemeLoader.Singleton.changeTheme(_getTheme());
            //Old theme will be set by themeloader.
        }

        private Theme _getTheme()
        {
            var _lightTheme = new Uri($@"pack://application:,,,/WPF.Test;component/Resources/ThemeLight.xaml", UriKind.RelativeOrAbsolute);
            var _darkTheme = new Uri($@"pack://application:,,,/WPF.Test;component/Resources/ThemeDark.xaml", UriKind.RelativeOrAbsolute);
            //var _base_dic = new Uri($@"pack://application:,,,/WPF.Test;component/Resources/DicRD.xaml", UriKind.RelativeOrAbsolute);
            Uri _base_dic = null;
            Theme activeTheme = null;
            //Switch theme.
            switch (is_dark_theme)
            {
                case true:
                    activeTheme = new Theme(_lightTheme, _darkTheme, _base_dic);
                    is_dark_theme = false;
                    break;
                case false:
                    activeTheme = new Theme(_darkTheme, _lightTheme, _base_dic);
                    is_dark_theme = true;
                    break;
            }
            return activeTheme;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var theme = _getTheme();
            ThemeLoader.Singleton.changeTheme(this, theme,Haley.Enums.SearchPriority.FrameworkElement);
        }

        private void PlainButton_Click_2(object sender, RoutedEventArgs e)
        {
            var _currentmode = ThemeLoader.Singleton.current_internal_mode;
            ThemeMode _newmode = ThemeMode.Dark;
            switch(_currentmode)
            { 
                case ThemeMode.Dark:
                    _newmode = ThemeMode.Normal;
                    break;
                case ThemeMode.Normal:
                    _newmode = ThemeMode.Dark;
                    break;
            }
            ThemeLoader.Singleton.changeInternalTheme(_newmode);
        }
    }
}
