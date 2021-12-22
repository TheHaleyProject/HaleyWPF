using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using dwg = System.Drawing;
using System.Collections.ObjectModel;

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
    public class ColorPicker : FrameworkElement
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker() { }

        #region Attributes
        private UniAxisPickerAdorner _hueAdorner;
        private BiAxisPickerAdorner _svAdorner;
        private static Brush _startbrush = new SolidColorBrush(Colors.White);
        private bool _freezeRGBChange = false;
        private bool _freezeHSVChange = false;
        #endregion

        #region Internal Properties
        internal List<Color> CommonColors { get; set; }

        #endregion


        #region Properties
        public int MaxStoredColors { get; set; }

        #endregion

        #region Dependency Properties
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register(nameof(SelectedBrush), typeof(Brush), typeof(TestWindow), new PropertyMetadata(_startbrush, OnSelectedBrushChanged));

        public Brush OldBrush
        {
            get { return (Brush)GetValue(OldBrushProperty); }
            set { SetValue(OldBrushProperty, value); }
        }

        public static readonly DependencyProperty OldBrushProperty =
            DependencyProperty.Register(nameof(OldBrush), typeof(Brush), typeof(TestWindow), new PropertyMetadata(_startbrush));

        public ObservableCollection<Color> SavedColors
        {
            get { return (ObservableCollection<Color>)GetValue(SavedColorsProperty); }
            set { SetValue(SavedColorsProperty, value); }
        }

        public static readonly DependencyProperty SavedColorsProperty =
            DependencyProperty.Register(nameof(SavedColors), typeof(ObservableCollection<Color>), typeof(TestWindow), new FrameworkPropertyMetadata(new ObservableCollection<Color>(), propertyChangedCallback: SavedColorsChanged));
        #endregion




    }
}

//READ : https://en.wikipedia.org/wiki/HSL_and_HSV

/// <summary>
/// Interaction logic for TestWindow.xaml
/// </summary>
public partial class TestWindow : Window
{
    
   

    internal Color SelectedHueColor
    {
        get { return (Color)GetValue(SelectedHueColorProperty); }
        set { SetValue(SelectedHueColorProperty, value); }
    }

    internal static readonly DependencyProperty SelectedHueColorProperty =
        DependencyProperty.Register(nameof(SelectedHueColor), typeof(Color), typeof(TestWindow), new PropertyMetadata(Colors.Red, OnSelectedHueChanged));

    internal double Alpha
    {
        get { return (double)GetValue(AlphaProperty); }
        set { SetValue(AlphaProperty, value); }
    }

    internal static readonly DependencyProperty AlphaProperty =
        DependencyProperty.Register(nameof(Alpha), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

    internal double RedComponent
    {
        get { return (double)GetValue(RedComponentProperty); }
        set { SetValue(RedComponentProperty, value); }
    }

    internal static readonly DependencyProperty RedComponentProperty =
        DependencyProperty.Register(nameof(RedComponent), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

    internal double GreenComponent
    {
        get { return (double)GetValue(GreenComponentProperty); }
        set { SetValue(GreenComponentProperty, value); }
    }

    internal static readonly DependencyProperty GreenComponentProperty =
        DependencyProperty.Register(nameof(GreenComponent), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

    internal double BlueComponent
    {
        get { return (double)GetValue(BlueComponentProperty); }
        set { SetValue(BlueComponentProperty, value); }
    }

    internal static readonly DependencyProperty BlueComponentProperty =
        DependencyProperty.Register(nameof(BlueComponent), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

    internal string HexComponent
    {
        get { return (string)GetValue(HexComponentProperty); }
        set { SetValue(HexComponentProperty, value); }
    }

    internal static readonly DependencyProperty HexComponentProperty =
        DependencyProperty.Register(nameof(HexComponent), typeof(string), typeof(TestWindow), new PropertyMetadata(string.Empty));

    public string ErrorMessage
    {
        get { return (string)GetValue(ErrorMessageProperty); }
        set { SetValue(ErrorMessageProperty, value); }
    }

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(TestWindow), new PropertyMetadata(""));


    private HSV _selected_hsv;
    private IDialogService _ds;
    public TestWindow()
    {
        CommonColors = new List<Color>() { Colors.Crimson, Colors.DodgerBlue, Colors.ForestGreen, Colors.Yellow, Colors.SandyBrown, Colors.DarkMagenta, Colors.Orchid, Colors.CadetBlue };
        InitializeComponent();
        lstCommonColors.ItemsSource = CommonColors;
        MaxStoredColors = 12;

        SavedColors.Add(Colors.BlanchedAlmond);
        _initialize();

        _ds = ContainerStore.Singleton.DI.Resolve<IDialogService>();

    }

    private void _initialize()
    {
        //Initate the Adorners
        _hueAdorner = new UniAxisPickerAdorner(HueRectangle, Orientation.Vertical);
        _svAdorner = new BiAxisPickerAdorner(SLRectangle);
        _hueAdorner.PositionChangedEvent += _hueAdorner_PositionChangedEvent;
        _svAdorner.PositionChangedEvent += _svAdorner_PositionChangedEvent;

        //For red color, hue value is zero.
        //hue_value = dwg.Color.FromArgb(SelectedHueColor.R, SelectedHueColor.G, SelectedHueColor.B).GetHue();

        //Hooks the element's events with adorners.
        PickerAdornerEventsHook.Hook(_hueAdorner); //In case we need to remove or get hook or unsubcribe, we can get the hook id from the return value of the method.
        PickerAdornerEventsHook.Hook(_svAdorner);

        _initiateHSV();

    }

    private void _initiateHSV()
    {
        //From the selected hue and saturation values, get the HSV.
        _selected_hsv = new HSV(0.0, 1.0, 1.0);
    }

    private void _svAdorner_PositionChangedEvent(object sender, ValueChangedArgs<(Point position, Point percent)> e)
    {
        if (_freezeHSVChange) return; //If HSV is getting changed, we should not act upon it.
                                      //Information on Saturation & Brightness  (the below information on HSL taken from http://csharphelper.com/blog/2016/08/convert-between-rgb-and-hls-color-models-in-c/ )
                                      //Lightness (Brightness) indicates how much light is in the color. When lightness = 0, the color is black. When lightness = 1, the color is white. When lightness = 0.5, the color is as “pure” as possible.
                                      //Saturation indicates the amount of color added.You can think of this as the opposite of “grayness.” When saturation = 0, the color is pure gray. In this case, if lightness = 0.5 you get a neutral color.When saturation is 1, the color is “pure.”

        //Saturation is intensity: LEFT TO RIGHT :we have white to selected hue . It is like S=0 to S=1.
        //So, X value can be used to identify saturation.
        _selected_hsv.Saturation = e.Value.percent.X;

        //Lightness is amount of light. if it is zero, then black. In our case,TOP TO BOTTOM: we have white to black. Its like L=1 to L=0. So, we need to get the Y value for lightness/brightness and reverse it.
        //Difference between brightness and lightness is that at value 1.0, lightness returns white color, brightness returns the Hue color with more white shade.
        _selected_hsv.Value = 1 - e.Value.percent.Y;
        _setRGBComponents();
        changeSelectedBrush();
    }

    void _hueAdorner_PositionChangedEvent(object sender, ValueChangedArgs<(Point position, Point percent)> e)
    {
        if (_freezeHSVChange) return; //If HSV is getting changed, we should not act upon it.
                                      //Based upon the position, get the color at offset in the hue gradient (this is vertical, get the Y )
        double offset = (_hueAdorner.Orientation == Orientation.Horizontal) ? e.Value.percent.X : e.Value.percent.Y;
        var _newcolor = ColorUtils.GetColorAtOffset(HueRectangleGradients.GradientStops, offset);
        SetCurrentValue(SelectedHueColorProperty, _newcolor);
        _selected_hsv.Hue = ColorUtils.ColorToHSV(SelectedHueColor).Hue; //We don't need the whole HSV value. We only need the hue (as we have a separate method to get the S and V Values).
        _setRGBComponents();
        changeSelectedBrush();
    }
    void changeSelectedBrush()
    {
        //Get new RGB from HSV.
        var hsvColor = ColorUtils.HsvToColor(_selected_hsv.Hue, _selected_hsv.Saturation, _selected_hsv.Value, Alpha);
        SelectedBrush = new SolidColorBrush(hsvColor);
    }
    void _setRGBComponents()
    {
        //Meaning we are going to change the RGB values. While we do that, we should not enter into a loop.
        _freezeRGBChange = true;
        //Based upon the selected HSV, we set the components value.
        var _selectedColor = ColorUtils.HsvToColor(_selected_hsv.Hue, _selected_hsv.Saturation, _selected_hsv.Value, Alpha);
        SetCurrentValue(RedComponentProperty, (double)_selectedColor.R);
        SetCurrentValue(GreenComponentProperty, (double)_selectedColor.G);
        SetCurrentValue(BlueComponentProperty, (double)_selectedColor.B);
        _freezeRGBChange = false;
    }

    void _setHSVComponents()
    {
        //Meaning we are going to change the HSV values. This means that the user has manually modified the RGB slider or the RGB values. Now, based on this we need to act upon.
        _freezeHSVChange = true;
        //Based upon the user selected RGB values, we now, change the HSV and also the adorner location.
        var _newcolor = Color.FromArgb((byte)Alpha, (byte)RedComponent, (byte)GreenComponent, (byte)BlueComponent);

        //With this new color, get the HSV value.
        var _newHSV = ColorUtils.ColorToHSV(_newcolor);
        _selected_hsv.Saturation = _newHSV.Saturation;
        _selected_hsv.Hue = _newHSV.Hue;
        _selected_hsv.Value = _newHSV.Value;

        //Change the Selected Hue.
        SetCurrentValue(SelectedHueColorProperty, _newcolor);
        //Update HUE
        var _huePercent = _selected_hsv.Hue / 360.0;
        if (_hueAdorner.Orientation == Orientation.Horizontal)
        {
            _hueAdorner.SetPosition(_huePercent, 0.0);
        }
        else
        {
            _hueAdorner.SetPosition(0.0, _huePercent);
        }
        //Update SATURATION & VALUE
        _svAdorner.SetPosition(_selected_hsv.Saturation, (1 - _selected_hsv.Value));
        _freezeHSVChange = false;
    }
    static void OnComponentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TestWindow twdw)
        {
            if (twdw._freezeRGBChange) return; //Don't do anything if freeze is in motion
                                               //When components changes, 
            twdw._setHSVComponents(); //Based upon the component change, set the hsv value.
            twdw.changeSelectedBrush();
        }
    }
    static void OnSelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TestWindow twdw)
        {
            var _color = (twdw.SelectedBrush as SolidColorBrush)?.Color;
            if (_color != null)
            {
                var _hexvalue = ColorUtils.ColorToHex(_color.Value);
                twdw.SetCurrentValue(HexComponentProperty, _hexvalue);
            }
        }
    }
    static void OnSelectedHueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //This we can use to update the adorner fill color.
        if (d is TestWindow twdw)
        {
            twdw._hueAdorner.FillColor = twdw.SelectedHueColor;
        }
    }
    static void SavedColorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //When a new color is changed, then add it to the favourites.
    }

    private void ImageButton_Click(object sender, RoutedEventArgs e)
    {
        //Get the selected color and add it to the saved colors.
        SavedColors.Insert(0, SelectedHueColor);
        var _toremove = SavedColors.Skip(MaxStoredColors).ToList();
        if (_toremove != null && _toremove.Count() > 0)
        {
            foreach (var item in _toremove)
            {
                SavedColors.Remove(item);
            }
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (SavedColors == null || SavedColors.Count == 0) return;
        //Raise a dialog for confirmation.
        if (_ds != null)
        {
            var ds_result = _ds.Warning("Clear Favourites", "Clear all the stored favourite colors from memory?", DialogMode.Confirmation, true);
            if (ds_result.DialogResult.HasValue && !ds_result.DialogResult.Value)
            {
                return; //If user says not to proceed, exit here.
            }
        }
        SavedColors.Clear(); //clear all
    }

    private void deleteLatestClick(object sender, RoutedEventArgs e)
    {
        if (SavedColors.Count > 0)
        {
            SavedColors.RemoveAt(0);
        }
    }
}