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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Haley.WPF.Internal
{
    /// <summary>
    /// Interaction logic for ColorPickerWindow.xaml
    /// </summary>
    internal partial class ColorPickerWindow : PlainWindow
    {
        public SolidColorBrush SelectedBrush { get; set; }
        public List<Color> SavedColors { get; set; }
        public ColorPickerWindow()
        {
            InitializeComponent();
            SavedColors = new List<Color>();
        }

        public void SetOptions(List<Color> savedColors, SolidColorBrush oldBrush = null, int maxSavedColrs = 9, bool showminiInfo = true, DisplayMode display = DisplayMode.Compact)
        {
            if (savedColors != null && savedColors.Count > 0)
            {
                clrpckr.SavedColors = new ObservableCollection<Color>(savedColors); //Set the saved colors.
            }

            if (oldBrush != null)
            {
                clrpckr.OldBrush = oldBrush;
            }

            if (maxSavedColrs > 1)
            {
                clrpckr.MaxStoredColors = maxSavedColrs;
            }

            switch (display)
            {
                case DisplayMode.Compact:
                    //Hide RGB
                    clrpckr.HideRGBComponents = true;
                    clrpckr.HideColorPalette = false;
                    break;
                case DisplayMode.Full:
                    //Show RGB, Palette
                    clrpckr.HideRGBComponents = false;
                    clrpckr.HideColorPalette = false;
                    break;
                case DisplayMode.Mini:
                    //Hide RGB and Palette
                    clrpckr.HideRGBComponents = true;
                    clrpckr.HideColorPalette = true;
                    break;
            }

            clrpckr.ShowMiniInfo = showminiInfo;
        }

        public ColorPickerWindow(List<Color> savedColors, SolidColorBrush oldBrush = null, int maxSavedColrs = 9, bool showminiInfo = true, DisplayMode display = DisplayMode.Compact) :this()
        {
            SetOptions(savedColors, oldBrush, maxSavedColrs, showminiInfo, display);
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            //When confirm buton is clicked.
            SelectedBrush = clrpckr.SelectedBrush;
            this.DialogResult = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //if the window is closed, (meaning dialogresult is null), other properties are empty. So, set whatever prop you need here.
            base.OnClosing(e);
            SavedColors = clrpckr.SavedColors.ToList(); //To retrieve and store locally (if required).
        }
    }
}
