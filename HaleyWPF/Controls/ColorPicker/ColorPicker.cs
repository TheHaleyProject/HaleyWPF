using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.WPF.Controls
{

    //Main concept behind creating a color gradient is simple: We mix and play three primary colors. Red, Green, Blue (RGB)

    // Look at below combinations.
    //     R     G     B     COLOR
    //     255   0     0     RED
    //     0     255   0     GREEN
    //     0     0     255   BLUE
    //     255   255   0     YELLOW
    //     0     255   255   CYAN (LIKE LIGHT SKY BLUE)
    //     255   0     255   FUSCHIA (LIKE DARK PURPLE; 128,0,128 GIVES PURPLE :) )
    //     255   255   255   WHITE
    //     0     0     0     BLACK

    //So, if we mix different values we can get different shades of color.. Our gradient should have  16.7 million shades of colors (256 *256 * 256 = 16,777,216). Oh wait! why 256? because, even "0" will give us a color. So, "0" also has to be taken in to consideration. So we take 256 instead of 255.

    //Now, that we know by mixing the numbers, we can generate different colors. We can use different methods to generate it. We an create 16.7 million small borders and each holding a color or we can use the Xaml to directly generate the color gradient. 
    public class ColorPicker : Control , ICornerRadius
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainButton), new FrameworkPropertyMetadata(typeof(PlainButton)));
        }

        public ColorPicker() { }

        #region Corner Radius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainButton), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));
        #endregion

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(PlainButton), new PropertyMetadata(null));
    }
}
