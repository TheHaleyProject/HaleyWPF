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

namespace Haley.WPF.BaseControls
{
    public class ImageComboButton : ImageButton
    {
        #region Constructors
        static ImageComboButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageComboButton), new FrameworkPropertyMetadata(typeof(ImageComboButton)));
        }

        public ImageComboButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            //Only in case of top or bottom, re arrange the dock children
            if (ImageLocation == Dock.Top || ImageLocation == Dock.Bottom)
            {
                //Try to get the template.
                var _dock = GetTemplateChild("PART_maindock") as DockPanel;
                var _imagebox = GetTemplateChild("PART_ImageViewBox") as Viewbox;
                var _txtbx = GetTemplateChild("PART_TextHolder") as TextBlock;

                //In this case, Textbox holder should be the first child
                _dock.Children.Clear();
                _txtbx.SetValue(DockPanel.DockProperty, ImageLocation == Dock.Top? Dock.Bottom : Dock.Top); //Since we are using ImageLocation to specify Text location, we need to invert it.
                _dock.Children.Add(_txtbx);
                _dock.Children.Add(_imagebox);
            }
            base.OnApplyTemplate();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ImageComboButton), new FrameworkPropertyMetadata("Button"));

        public Dock ImageLocation
        {
            get { return (Dock)GetValue(ImageLocationProperty); }
            set { SetValue(ImageLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageLocationProperty =
            DependencyProperty.Register(nameof(ImageLocation), typeof(Dock), typeof(ImageComboButton), new FrameworkPropertyMetadata(Dock.Top));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(ImageComboButton), new PropertyMetadata(TextAlignment.Center));
    }
}
