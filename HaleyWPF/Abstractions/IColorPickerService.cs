using System.Windows.Media;
using Haley.Enums;
using System.Collections.Generic;
using System.Collections;

namespace Haley.Abstractions
{
    public interface IColorPickerService
    {
        SolidColorBrush SelectedBrush { get; }
        Color SelectedColor { get;}
        bool? ShowDialog(DisplayMode mode = DisplayMode.Compact);
        bool? ShowDialog(SolidColorBrush oldBrush, DisplayMode mode = DisplayMode.Compact);
        void SetOptions(bool showminiInfo, int maxfavourites = 0);
    }
}
