using System.Windows.Media;

namespace Haley.Abstractions
{
    public interface IItemsSelection
    {
        Brush ItemSelectedColor { get; set; }
        Brush ItemHoverColor { get; set; }
    }
}
