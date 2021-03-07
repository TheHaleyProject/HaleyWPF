using System.Windows.Media;

namespace Haley.Abstractions
{
    public interface IShadow
    {
        bool ShadowOnlyOnMouseOver { get; set; }
        bool ShowShadow { get; set; }
        Brush ShadowColor { get; set; }
    }
}
