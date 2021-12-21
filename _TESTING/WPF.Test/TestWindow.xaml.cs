﻿using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using dwg=System.Drawing;

namespace WPF.Test
{
    //READ : https://en.wikipedia.org/wiki/HSL_and_HSV

    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private static List<Color> _lastusedColors = new List<Color>();
        private UniAxisPickerAdorner _hueAdorner;
        private BiAxisPickerAdorner _svAdorner;
        private static Brush _startbrush = new SolidColorBrush(Colors.White);
        private bool _freezeRGBChange = false;
        private bool _freezeHSVChange = false;

        public static void AddColor(Color newColor)
        {
            if (!_lastusedColors.Contains(newColor))
            {
                _lastusedColors.Add(newColor);
            }
        }

        public List<Color> FavouriteColors { get; set; }

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register(nameof(SelectedBrush), typeof(Brush), typeof(TestWindow), new PropertyMetadata(_startbrush,OnSelectedBrushChanged));

        public Brush OldBrush
        {
            get { return (Brush)GetValue(OldBrushProperty); }
            set { SetValue(OldBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OldBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OldBrushProperty =
            DependencyProperty.Register(nameof(OldBrush), typeof(Brush), typeof(TestWindow), new PropertyMetadata(_startbrush));

        internal Color SelectedHueColor
        {
            get { return (Color)GetValue(SelectedHueColorProperty); }
            set { SetValue(SelectedHueColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedHueColor.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty SelectedHueColorProperty =
            DependencyProperty.Register(nameof(SelectedHueColor), typeof(Color), typeof(TestWindow), new PropertyMetadata(Colors.Red,OnSelectedHueChanged));

        internal double Alpha
        {
            get { return (double)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Alpha.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register(nameof(Alpha), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,propertyChangedCallback: OnComponentsChanged));

        internal double RedComponent
        {
            get { return (double)GetValue(RedComponentProperty); }
            set { SetValue(RedComponentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RedComponent.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty RedComponentProperty =
            DependencyProperty.Register(nameof(RedComponent), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

        internal double GreenComponent
        {
            get { return (double)GetValue(GreenComponentProperty); }
            set { SetValue(GreenComponentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GreenComponent.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty GreenComponentProperty =
            DependencyProperty.Register(nameof(GreenComponent), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

        internal double BlueComponent
        {
            get { return (double)GetValue(BlueComponentProperty); }
            set { SetValue(BlueComponentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlueComponent.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty BlueComponentProperty =
            DependencyProperty.Register(nameof(BlueComponent), typeof(double), typeof(TestWindow), new FrameworkPropertyMetadata(255.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnComponentsChanged));

        internal string HexComponent
        {
            get { return (string)GetValue(HexComponentProperty); }
            set { SetValue(HexComponentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HexComponent.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty HexComponentProperty =
            DependencyProperty.Register(nameof(HexComponent), typeof(string), typeof(TestWindow), new PropertyMetadata(string.Empty));


        private HSV _selected_hsv;
        private HSV _old_hsv;

        public TestWindow()
        {
            InitializeComponent();

            _initialize();
        }

        private void _initialize()
        {
            FavouriteColors = new List<Color>(); //No need to use observable collection, as we will update the UI only on loading.

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
            _selected_hsv.Value = 1- e.Value.percent.Y;
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
            var hsvColor =  ColorUtils.HsvToColor(_selected_hsv.Hue, _selected_hsv.Saturation, _selected_hsv.Value, Alpha);
            SelectedBrush = new SolidColorBrush(hsvColor);
        }
        void _setRGBComponents()
        {
            //Meaning we are going to change the RGB values. While we do that, we should not enter into a loop.
            _freezeRGBChange = true;
            //Based upon the selected HSV, we set the components value.
            var _selectedColor = ColorUtils.HsvToColor(_selected_hsv.Hue, _selected_hsv.Saturation, _selected_hsv.Value, Alpha);
            SetCurrentValue(RedComponentProperty,(double) _selectedColor.R);
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
                _hueAdorner.SetPosition(_huePercent,0.0);
            }
            else
            {
                _hueAdorner.SetPosition(0.0,_huePercent);
            }
            //Update SATURATION & VALUE
            _svAdorner.SetPosition(_selected_hsv.Saturation, (1 - _selected_hsv.Value));
            _freezeHSVChange = false;
        }
        static void OnComponentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if (d is TestWindow twdw)
            {
                if (twdw._freezeRGBChange)return; //Don't do anything if freeze is in motion
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
    }
}