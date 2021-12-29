using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Haley.Events;
using Haley.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Haley.Models;
using System.Reflection;

namespace Haley.WPF.Controls
{
    public class PlainTextBox : TextBox, ICornerRadius
    {
        private const string UIEContentHost = "PART_ContentHost";
        private const string UIEPopUp = "PART_popup_content";

        private FrameworkElement _popupContent;
        private FrameworkElement _contentHost;
        //private DataTemplate _defaultPopupTemplate;
        static PlainTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainTextBox), new FrameworkPropertyMetadata(typeof(PlainTextBox)));
        }

        public bool DisplaySuggestions { get; set; }
        public PlainTextBox()
        {
            DisplaySuggestions = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popupContent = GetTemplateChild(UIEPopUp) as FrameworkElement;
            _contentHost = GetTemplateChild(UIEContentHost) as FrameworkElement;
            ShowSuggestions = DisplaySuggestions;
            //_defaultPopupTemplate = TryFindResource("internal_default_popup") as DataTemplate;
            _setSuggestion();
            _setPopupTemplate();
        }

        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register(nameof(WaterMark), typeof(string), typeof(PlainTextBox), new PropertyMetadata("Enter Value"));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainTextBox), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public ObservableCollection<Suggestion> Suggestions
        {
            get { return (ObservableCollection<Suggestion>)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        public static readonly DependencyProperty SuggestionsProperty =
            DependencyProperty.Register(nameof(Suggestions), typeof(ObservableCollection<Suggestion>), typeof(PlainTextBox), new PropertyMetadata(null, propertyChangedCallback: OnSuggestionSourceChanged));

        internal bool ShowSuggestions
        {
            get { return (bool)GetValue(ShowSuggestionsProperty); }
            set { SetValue(ShowSuggestionsProperty, value); }
        }

        internal static readonly DependencyProperty ShowSuggestionsProperty =
            DependencyProperty.Register(nameof(ShowSuggestions), typeof(bool), typeof(PlainTextBox), new PropertyMetadata(false));

        internal bool SuggestionsAvailable
        {
            get { return (bool)GetValue(SuggestionsAvailableProperty); }
            set { SetValue(SuggestionsAvailableProperty, value); }
        }

        internal static readonly DependencyProperty SuggestionsAvailableProperty =
            DependencyProperty.Register(nameof(SuggestionsAvailable), typeof(bool), typeof(PlainTextBox), new PropertyMetadata(false));

        internal Suggestion SelectedSuggestion
        {
            get { return (Suggestion)GetValue(SelectedSuggestionProperty); }
            set { SetValue(SelectedSuggestionProperty, value); }
        }

        internal static readonly DependencyProperty SelectedSuggestionProperty =
            DependencyProperty.Register(nameof(SelectedSuggestion), typeof(Suggestion), typeof(PlainTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback: OnSelectedSuggestionChanged));

        public DataTemplate PopupTemplate
        {
            get { return (DataTemplate)GetValue(PopupTemplateProperty); }
            set { SetValue(PopupTemplateProperty, value); }
        }

        public static readonly DependencyProperty PopupTemplateProperty =
            DependencyProperty.Register(nameof(PopupTemplate), typeof(DataTemplate), typeof(PlainTextBox), new PropertyMetadata(null,OnPopupTemplatePropertyChanged));

        static void OnSelectedSuggestionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainTextBox ptbx) ptbx._changeText();
        }
        static void OnSuggestionSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainTextBox ptbx) ptbx._setSuggestion();
        }

        static void OnPopupTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlainTextBox ptbx) ptbx._setPopupTemplate();
        }

        void _setPopupTemplate()
        {
            if (_popupContent == null) return; //Not able to get the listview.
            if (_popupContent is ItemsControl itmsctrl)
            {
                if (PopupTemplate != null)
                {
                    itmsctrl.SetCurrentValue(ItemsControl.ItemTemplateProperty, PopupTemplate);
                }
            }
        }

        void _setSuggestion()
        {
            if (!DisplaySuggestions) return;
            //only then show the popup.
            SuggestionsAvailable = (Suggestions != null && Suggestions.Count > 0);
            ShowSuggestions = SuggestionsAvailable; // Show only if available.
        }

        void _changeText()
        {
            ShowSuggestions = false; //Because we have already selected some value from the pop up.
            if (SelectedSuggestion == null) return;

            //Set the text without raising the property changed or text changed event (by first setting the field and then the property. So that the text change is not monitored.
            SetCurrentValue(TextProperty, SelectedSuggestion.Text); //Will this start a loop of text change?
            this.Focus();
            CaretIndex = Text.Length; //So that the cursor is at end.
            //Even though suggestions are available, if selection is made, don't show anymore.
        }
    }
}
