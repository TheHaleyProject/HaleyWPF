using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Haley.Enums;
using Haley.Utils;
using Haley.MVVM;
using Haley.Services;

namespace Haley.WPF.Controls
{
    public class ColorPickerButton : ContentControl, ICornerRadius
    {
        private IColorPickerService _cpService;
        static ColorPickerButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPickerButton), new FrameworkPropertyMetadata(typeof(ColorPickerButton)));
        }

        public ColorPickerButton() 
        {
            _cpService = new ColorPickerDialog();
            _cpService.SetOptions(true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            //When left button is pressed, we open up the dialog and display.
            _cpService.SetOptions(ShowMiniInfo);
            var _res = _cpService.ShowDialog(OldBrush, Mode);
            if (_res.HasValue && _res.Value)
            {
                SetCurrentValue(SelectedColorProperty, _cpService.SelectedColor);
                SetCurrentValue(SelectedBrushProperty, _cpService.SelectedBrush);
                //Favourites setting is taken care by the service.
            }
        }

        #region Corner Radius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ColorPickerButton), new FrameworkPropertyMetadata(default(CornerRadius)));
        #endregion

        public SolidColorBrush OldBrush
        {
            get { return (SolidColorBrush)GetValue(OldBrushProperty); }
            set { SetValue(OldBrushProperty, value); }
        }

        public static readonly DependencyProperty OldBrushProperty =
            DependencyProperty.Register(nameof(OldBrush), typeof(SolidColorBrush), typeof(ColorPickerButton), new FrameworkPropertyMetadata(null));

        public SolidColorBrush SelectedBrush
        {
            get { return (SolidColorBrush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register(nameof(SelectedBrush), typeof(SolidColorBrush), typeof(ColorPickerButton), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorPickerButton), new FrameworkPropertyMetadata(Colors.Transparent,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DisplayMode Mode
        {
            get { return (DisplayMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(DisplayMode), typeof(ColorPickerButton), new FrameworkPropertyMetadata(DisplayMode.Compact));

        public bool ShowMiniInfo
        {
            get { return (bool)GetValue(ShowMiniInfoProperty); }
            set { SetValue(ShowMiniInfoProperty, value); }
        }

        public static readonly DependencyProperty ShowMiniInfoProperty =
            DependencyProperty.Register(nameof(ShowMiniInfo), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));
    }
}
