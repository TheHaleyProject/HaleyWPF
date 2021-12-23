using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Haley.Abstractions;
using Haley.Enums;
using Haley.WPF.BaseControls;
using Haley.Utils;
using System.Windows;
using System.Windows.Controls;
using Haley.WPF.Internal;

namespace Haley.Services
{
    public class ColorPickerDialog : IColorPickerService
    {
        private bool ShowMiniInfo;
        //the saved colors remain static
        private static List<Color> _storedColors = new List<Color>();
        public static List<Color> Favourites => _storedColors;
        private static int MaxFavouriteColors = 6;
        public SolidColorBrush SelectedBrush { get; private set; }
        public Color SelectedColor { get; private set; }
        public static void SetFavourites(List<Color> favouriteColors)
        {
            _storedColors = favouriteColors;
        }
        public static void AddFavourite(Color color)
        {
            if (!_storedColors.Contains(color)) _storedColors.Add(color);
        }
        public static void RemoveFavourite(Color color)
        {
            if (_storedColors.Contains(color))
            {
                _storedColors.Remove(color);
            }
        }
        public void SetOptions(bool showminiInfo, int maxfavourites = 0)
        {
            if (maxfavourites > 0) MaxFavouriteColors = maxfavourites;
            ShowMiniInfo = showminiInfo;
        }
        public bool? ShowDialog(DisplayMode mode = DisplayMode.Compact)
        {
            return ShowDialog(null, mode);
        }
        public bool? ShowDialog(SolidColorBrush oldBrush, DisplayMode mode = DisplayMode.Compact)
        {
            bool? result;
            var _clrpckWindow = new ColorPickerWindow(Favourites, oldBrush, MaxFavouriteColors, ShowMiniInfo, mode);
            result = _clrpckWindow.ShowDialog();

            //Show this dialog and retrieve the selected brush and savedcolors.
            if (result.HasValue && result.Value)
            {
                SelectedBrush = _clrpckWindow.SelectedBrush;
                SelectedColor = SelectedBrush.Color;
            }
            //Irrespective of the dialog result, get the favourites.
            SetFavourites(_clrpckWindow.SavedColors); //When the window dialog result is false, it will not store any property.
            return result;
        }

        public ColorPickerDialog()
        {
        }
    }
}
