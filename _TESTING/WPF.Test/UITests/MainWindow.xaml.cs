using Haley.Models;
using Haley.Utils;
using System.Windows;
using System;
using Haley.Enums;
using System.Collections.ObjectModel;
using Haley.Abstractions;
using Haley.Services;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool is_dark_theme = true;
        private IThemeService _ts;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
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

            var _imageinfo = ImageUtils.GetImageInfo(_source);
            var _newsource = ImageUtils.ChangeImageColor(_imageinfo, _red, _green, _blue);
            Haley.Models.Icon.SetDefault(imgeChanger, _newsource);

        }

        private void PlainButton_Click_1(object sender, RoutedEventArgs e)
        {
            changeTheme();
        }

        private void changeTheme()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PlainButton_Click_2(object sender, RoutedEventArgs e)
        {
            changeTheme();
        }

        private void pBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //Event after changed.
            if (!string.IsNullOrEmpty(pBox.Text) && pBox.Text?.Length == 3)
            {
                if (pBox.Text.StartsWith("sen"))
                {
                    var _sugg = new ObservableCollection<Suggestion>();
                    _sugg.Add(new Suggestion("senguttuvan", null));
                    _sugg.Add(new Suggestion("Shivanya", null));
                    _sugg.Add(new Suggestion("Bhadrinarayanan", null));
                    pBox.Suggestions = _sugg;
                    return;
                }
            }
            pBox.Suggestions = null;
        }

        private void sBar_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //Event after changed.
            if (!string.IsNullOrEmpty(sBar.Text) && sBar.Text?.Length == 3)
            {
                if (sBar.Text.StartsWith("sen"))
                {
                    var _sugg = new ObservableCollection<Suggestion>();
                    _sugg.Add(new Suggestion("senguttuvan",null));
                    _sugg.Add(new Suggestion("Latha", null));
                    _sugg.Add(new Suggestion("Bhadrinarayanan", null));
                    sBar.Suggestions = _sugg;
                    return;
                }
            }
            sBar.Suggestions = null;
        }

        private void PlainButton_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void asdfwe(object sender, System.Windows.Data.DataTransferEventArgs e) {

        }
    }
}
