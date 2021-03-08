using System.Windows.Media;
using System.Windows;

namespace Haley.Abstractions
{
    public interface IHoverBorder
    {
        Brush HoverBorderBrush { get; set; }
        Thickness HoverBorderThickness { get; set; }
    }
}
