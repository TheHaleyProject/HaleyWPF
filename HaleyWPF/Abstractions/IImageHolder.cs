using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Haley.Abstractions
{
    public interface IImageHolder
    {
        ImageSource DefaultImage { get; set; }
        ImageSource HoverImage { get; set; }
        SolidColorBrush DefaultImageColor { get; set; }
        SolidColorBrush HoverImageColor { get; set; }
    }
}
