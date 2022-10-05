using System.Windows;
using Haley.Models;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;

namespace Haley.WPF.Controls
{
    public class HotKeyEditor : PlainTextBox
    {
        #region Constructors
        static HotKeyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HotKeyEditor), new FrameworkPropertyMetadata(typeof(HotKeyEditor)));
        }

        public HotKeyEditor()
        {
            try {
                var _behaviours = Interaction.GetBehaviors(this);
                var _handler = new HotKeyHandler();
                _handler.KeyDown += HandleKeyDown;
                _behaviours.Add(_handler);
            } catch (System.Exception) {
            }
        }

        private void HandleKeyDown(object sender, IEnumerable<Key> e) {
            var newhk = new HotKey(e);
            // Update the value
            this.SetCurrentValue(HotKeyProperty, newhk);
        }
        #endregion

        public HotKey HotKey
        {
            get { return (HotKey)GetValue(HotKeyProperty); }
            set { SetValue(HotKeyProperty, value); }
        }

        public static readonly DependencyProperty HotKeyProperty =
            DependencyProperty.Register(nameof(HotKey), typeof(HotKey), typeof(HotKeyEditor), new FrameworkPropertyMetadata(default(HotKey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //If we need to fetch any UI element from theme and modify it, we do it here
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            //e.Handled = true;

            //// Get modifiers and key data
            //var modifiers = Keyboard.Modifiers;
            //var key = e.Key;

            //// When Alt is pressed, SystemKey is used instead
            //if (key == Key.System)
            //{
            //    key = e.SystemKey;
            //}

            //// Pressing delete, backspace or escape without modifiers clears the current value
            //if (modifiers == ModifierKeys.None &&
            //    (key == Key.Delete || key == Key.Back || key == Key.Escape))
            //{
            //    HotKey = null;
            //    return;
            //}

            //// If no actual key was pressed - return
            //if (key == Key.LeftCtrl ||
            //    key == Key.RightCtrl ||
            //    key == Key.LeftAlt ||
            //    key == Key.RightAlt ||
            //    key == Key.LeftShift ||
            //    key == Key.RightShift ||
            //    key == Key.LWin ||
            //    key == Key.RWin ||
            //    key == Key.Clear ||
            //    key == Key.OemClear ||
            //    key == Key.Apps)
            //{
            //    return;
            //}

            //var newhk = new HotKey(key, modifiers);
            //// Update the value
            //this.SetCurrentValue(HotKeyProperty, newhk);
        }
    }
}
