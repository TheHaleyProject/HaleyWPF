using System.Windows.Media;
using System.Windows;

namespace Haley.Abstractions
{
    public interface IHoverBase
    {
        Brush HoverBorderBrush { get; set; }
        Thickness HoverBorderThickness { get; set; }
        Brush HoverBackground { get; set; }
        bool DisableHover { get; set; }
    }
}
