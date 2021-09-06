using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    public class LinkedText : Button
    {
        static LinkedText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkedText), new FrameworkPropertyMetadata(typeof(LinkedText)));
        }

        public LinkedText() { }

        public Brush HoverForeground
        {
            get { return (Brush)GetValue(HoverForegroundProperty); }
            set { SetValue(HoverForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register(nameof(HoverForeground), typeof(Brush), typeof(LinkedText), new FrameworkPropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LinkedText), new FrameworkPropertyMetadata("Button"));


        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextDecorations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register(nameof(TextDecorations), typeof(TextDecorationCollection), typeof(LinkedText));

        public bool TurnOffTextDecorations
        {
            get { return (bool)GetValue(TurnOffTextDecorationsProperty); }
            set { SetValue(TurnOffTextDecorationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TurnOffTextDecorations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TurnOffTextDecorationsProperty =
            DependencyProperty.Register(nameof(TurnOffTextDecorations), typeof(bool), typeof(LinkedText), new FrameworkPropertyMetadata(false));
    }
}
