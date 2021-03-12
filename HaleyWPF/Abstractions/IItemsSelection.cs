using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Haley.Abstractions
{
    public interface IItemsSelection 
    {
        Brush ItemSelectedColor { get; set; }
        Brush ItemHoverColor { get; set; }
    }
}
