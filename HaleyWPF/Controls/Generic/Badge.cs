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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.Enums;
using Haley.Abstractions;

namespace Haley.WPF.Controls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class Badge : Freezable
    {
        public string Id { get; private set; }
        public Adorner Adorner { get; set; }
        public AdornerLayer AdornerLayer { get; set; }
        public FrameworkElement Parent { get; set; }
        public event EventHandler ValueChanged;
        public Badge()
        { 
            Id = Guid.NewGuid().ToString();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(Badge), new FrameworkPropertyMetadata(default(ICommand)));

        public string CommandName
        {
            get { return (string)GetValue(CommandNameProperty); }
            set { SetValue(CommandNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(nameof(CommandName), typeof(string), typeof(Badge), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(Badge), new FrameworkPropertyMetadata(default(object)));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(Badge), new PropertyMetadata(12.0, propertyChangedCallback: badgeValueChanged));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(Badge), new FrameworkPropertyMetadata(defaultValue: "New",propertyChangedCallback:badgeValueChanged));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Badge), new PropertyMetadata(null,propertyChangedCallback: badgeValueChanged));
        
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Badge), new PropertyMetadata(null, propertyChangedCallback: badgeValueChanged));
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Badge), new PropertyMetadata(null, propertyChangedCallback: badgeValueChanged));
        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(double), typeof(Badge), new PropertyMetadata(0.0, propertyChangedCallback: badgeValueChanged));

        public object CustomShape
        {
            get { return (object)GetValue(CustomShapeProperty); }
            set { SetValue(CustomShapeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomShapeProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Badge), new PropertyMetadata(null, propertyChangedCallback: badgeValueChanged));

        public BadgeShape Shape
        {
            get { return (BadgeShape)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Shape.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(BadgeShape), typeof(Badge), new PropertyMetadata(BadgeShape.Ellipse, propertyChangedCallback: badgeValueChanged));

        public BadgeType Kind
        {
            get { return (BadgeType)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Kind.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KindProperty =
            DependencyProperty.Register(nameof(Kind), typeof(BadgeType), typeof(Badge), new PropertyMetadata(BadgeType.Info, propertyChangedCallback: badgeValueChanged));

        public BadgeAlignment Alignment
        {
            get { return (BadgeAlignment)GetValue(AlignmentProperty); }
            set { SetValue(AlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Alignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlignmentProperty =
            DependencyProperty.Register(nameof(Alignment), typeof(BadgeAlignment), typeof(Badge), new PropertyMetadata(BadgeAlignment.TopRight, propertyChangedCallback: badgeValueChanged));

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(Badge), new PropertyMetadata(true, propertyChangedCallback: badgeValueChanged));

        public BadgeAnchor Anchor
        {
            get { return (BadgeAnchor)GetValue(AnchorProperty); }
            set { SetValue(AnchorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Anchor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorProperty =
            DependencyProperty.Register(nameof(Anchor), typeof(BadgeAnchor), typeof(Badge), new PropertyMetadata(BadgeAnchor.Center, propertyChangedCallback: badgeValueChanged));

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(Badge), new PropertyMetadata(0.0, propertyChangedCallback: badgeValueChanged));

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LengthX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(Badge), new PropertyMetadata(0.0, propertyChangedCallback: badgeValueChanged));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LengthY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(Badge), new PropertyMetadata(0.0, propertyChangedCallback: badgeValueChanged));

        public override string ToString()
        {
            return this.Id;
        }
        static void badgeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           //Whatever the value might be, raise the valuechanged event.
           if (d is Badge badge)
            {
                badge.ValueChanged?.Invoke(d,null);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Badge();
        }
    }
}
