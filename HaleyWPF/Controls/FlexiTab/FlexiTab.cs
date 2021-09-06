using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Haley.Events;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Collections;
using System.Collections.Generic;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Utils;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Haley.WPF.Controls
{
    public class ContextMenuToggleButton : ToggleButton
    {
        public ContextMenu Menu { get; set; }

        protected override void OnClick()
        {
            base.OnClick();
            if (Menu != null)
            {
                if (IsChecked == true)
                {
                    Menu.PlacementTarget = this;
                    Menu.IsOpen = true;
                }
                else
                {
                    Menu.IsOpen = false;
                }
            }
        }
    }

    [TemplatePart(Name = OverflowButtonKey, Type = typeof(ContextMenuToggleButton))]
    [TemplatePart(Name = HeaderPanelKey, Type = typeof(TabPanel))]
    [TemplatePart(Name = OverflowScrollviewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ScrollButtonLeft, Type = typeof(ButtonBase))]
    [TemplatePart(Name = ScrollButtonRight, Type = typeof(ButtonBase))]
    [TemplatePart(Name = HeaderBorder, Type = typeof(Border))]
    public class FlexiTab : TabControl
    {
        #region Events
        public static readonly RoutedEvent SearchStartedEvent = EventManager.RegisterRoutedEvent(nameof(SearchStarted), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FlexiTab));

        public event RoutedEventHandler SearchStarted
        {
            add { AddHandler(SearchStartedEvent, value); }
            remove { RemoveHandler(SearchStartedEvent, value); }
        }
        #endregion

        static FlexiTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiTab), new FrameworkPropertyMetadata(typeof(FlexiTab)));
        }

        public FlexiTab()
        { }

            private const string OverflowButtonKey = "PART_OverflowButton";
            private const string HeaderPanelKey = "PART_HeaderPanel";
            private const string OverflowScrollviewer = "PART_OverflowScrollviewer";
            private const string HeaderBorder = "PART_HeaderBorder";
            private ContextMenuToggleButton _buttonOverflow;

            internal FlexiTabPanel HeaderPanel { get; private set; }
            private ScrollViewer _scrollViewerOverflow;
            private Border _headerBorder;
            internal bool IsInternalAction;

            public static readonly DependencyProperty CanDragProperty = DependencyProperty.Register(
                "CanDrag", typeof(bool), typeof(TabControl), new PropertyMetadata(false));

            public bool CanDrag
            {
                get => (bool)GetValue(CanDragProperty);
                set => SetValue(CanDragProperty, value);
            }

            public static readonly DependencyProperty CanCloseProperty = DependencyProperty.RegisterAttached(
                "CanClose", typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

            public static void SetCanClose(DependencyObject element, bool value)
                => element.SetValue(CanCloseProperty, value);

            public static bool GetCanClose(DependencyObject element)
                => (bool)element.GetValue(CanCloseProperty);

            public bool CanClose
            {
                get => (bool)GetValue(CanCloseProperty);
                set => SetValue(CanCloseProperty, value);
            }

            public static readonly DependencyProperty ShowContextMenuProperty = DependencyProperty.RegisterAttached(
                "ShowContextMenu", typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

            public static void SetShowContextMenu(DependencyObject element, bool value)
                => element.SetValue(ShowContextMenuProperty, value);

            public static bool GetShowContextMenu(DependencyObject element)
                => (bool)element.GetValue(ShowContextMenuProperty);

            public bool ShowContextMenu
            {
                get => (bool)GetValue(ShowContextMenuProperty);
                set => SetValue(ShowContextMenuProperty, value);
            }

            public static readonly DependencyProperty IsTabFillEnabledProperty = DependencyProperty.Register(
                "IsTabFillEnabled", typeof(bool), typeof(TabControl), new PropertyMetadata(false));

            public bool IsTabFillEnabled
            {
                get => (bool)GetValue(IsTabFillEnabledProperty);
                set => SetValue(IsTabFillEnabledProperty, value);
            }
            public static readonly DependencyProperty TabItemWidthProperty = DependencyProperty.Register(
                "TabItemWidth", typeof(double), typeof(TabControl), new PropertyMetadata(200.0));

            public double TabItemWidth
            {
                get => (double)GetValue(TabItemWidthProperty);
                set => SetValue(TabItemWidthProperty, value);
            }

            public static readonly DependencyProperty TabItemHeightProperty = DependencyProperty.Register(
                "TabItemHeight", typeof(double), typeof(TabControl), new PropertyMetadata(30.0));

            public double TabItemHeight
            {
                get => (double)GetValue(TabItemHeightProperty);
                set => SetValue(TabItemHeightProperty, value);
            }

            public static readonly DependencyProperty ShowOverflowButtonProperty = DependencyProperty.Register(
                "ShowOverflowButton", typeof(bool), typeof(TabControl), new PropertyMetadata(true));

            public bool ShowOverflowButton
            {
                get => (bool)GetValue(ShowOverflowButtonProperty);
                set => SetValue(ShowOverflowButtonProperty, value);
            }
            public static readonly DependencyProperty ShowScrollButtonProperty = DependencyProperty.Register(
                "ShowScrollButton", typeof(bool), typeof(TabControl), new PropertyMetadata(false));
            public bool ShowScrollButton
            {
                get => (bool)GetValue(ShowScrollButtonProperty);
                set => SetValue(ShowScrollButtonProperty, value);
            }

            private int _itemShowCount;

            protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
            {
                base.OnItemsChanged(e);

                if (HeaderPanel == null)
                {
                    IsInternalAction = false;
                    return;
                }

                UpdateOverflowButton();

                if (IsInternalAction)
                {
                    IsInternalAction = false;
                    return;
                }

                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    for (var i = 0; i < Items.Count; i++)
                    {
                        if (!(ItemContainerGenerator.ContainerFromIndex(i) is FlexiTabItem item)) return;
                        item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        item.PlainTabPanel = HeaderPanel;
                    }
                }

                _headerBorder?.InvalidateMeasure();
                IsInternalAction = false;
            }

            public override void OnApplyTemplate()
            {
                if (_buttonOverflow != null)
                {
                    if (_buttonOverflow.Menu != null)
                    {
                        _buttonOverflow.Menu.Closed -= Menu_Closed;
                        _buttonOverflow.Menu = null;
                    }

                    _buttonOverflow.Click -= ButtonOverflow_Click;
                }

                base.OnApplyTemplate();
                HeaderPanel = GetTemplateChild(HeaderPanelKey) as FlexiTabPanel;

                if (IsTabFillEnabled) return;

                _buttonOverflow = GetTemplateChild(OverflowButtonKey) as ContextMenuToggleButton;
                _scrollViewerOverflow = GetTemplateChild(OverflowScrollviewer) as ScrollViewer;
                _headerBorder = GetTemplateChild(HeaderBorder) as Border;

                if (_buttonOverflow != null)
                {
                    var menu = new ContextMenu
                    {
                        Placement = PlacementMode.Bottom,
                        PlacementTarget = _buttonOverflow
                    };
                    menu.Closed += Menu_Closed;
                    _buttonOverflow.Menu = menu;
                    _buttonOverflow.Click += ButtonOverflow_Click;
                }
            }

            protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
            {
                base.OnRenderSizeChanged(sizeInfo);
                UpdateOverflowButton();
            }

            private void UpdateOverflowButton()
            {
                if (!IsTabFillEnabled)
                {
                    _itemShowCount = (int)(ActualWidth / TabItemWidth);
                }
            }

            private void Menu_Closed(object sender, RoutedEventArgs e) => _buttonOverflow.IsChecked = false;

            private void ButtonOverflow_Click(object sender, RoutedEventArgs e)
            {
                if (_buttonOverflow.IsChecked == true)
                {
                    _buttonOverflow.Menu.Items.Clear();
                    for (var i = 0; i < Items.Count; i++)
                    {
                        if (!(ItemContainerGenerator.ContainerFromIndex(i) is FlexiTabItem item)) continue;

                        var menuItem = new MenuItem
                        {
                            HeaderStringFormat = ItemStringFormat,
                            HeaderTemplate = ItemTemplate,
                            HeaderTemplateSelector = ItemTemplateSelector,
                            Header = item.Header,
                            Width = TabItemWidth,
                            IsChecked = item.IsSelected,
                            IsCheckable = true,
                            IsEnabled = item.IsEnabled
                        };

                        menuItem.Click += delegate
                        {
                            _buttonOverflow.IsChecked = false;

                            var list = GetActualList();
                            if (list == null) return;

                            var actualItem = ItemContainerGenerator.ItemFromContainer(item);
                            if (actualItem == null) return;

                            var index = list.IndexOf(actualItem);
                            if (index >= _itemShowCount)
                            {
                                list.Remove(actualItem);
                                list.Insert(0, actualItem);
                                if (IsAnimationEnabled)
                                {
                                    HeaderPanel.SetValue(FlexiTabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromMilliseconds(200)));
                                }
                                else
                                {
                                    HeaderPanel.SetValue(FlexiTabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromMilliseconds(0)));
                                }
                                HeaderPanel.ForceUpdate = true;
                                HeaderPanel.Measure(new Size(HeaderPanel.DesiredSize.Width, ActualHeight));
                                HeaderPanel.ForceUpdate = false;
                                SetCurrentValue(SelectedIndexProperty, ValueBoxes.Int0Box);
                            }

                            item.IsSelected = true;
                        };
                        _buttonOverflow.Menu.Items.Add(menuItem);
                    }
                }
            }

            internal void CloseAllItems() => CloseOtherItems(null);

            internal void CloseOtherItems(FlexiTabItem currentItem)
            {
                var actualItem = currentItem != null ? ItemContainerGenerator.ItemFromContainer(currentItem) : null;

                var list = GetActualList();
                if (list == null) return;

                IsInternalAction = true;

                for (var i = 0; i < Items.Count; i++)
                {
                    var item = list[i];
                    if (!Equals(item, actualItem) && item != null)
                    {
                        var argsClosing = new CancelRoutedEventArgs(PlainTabItem.ClosingEvent, item);

                        if (!(ItemContainerGenerator.ContainerFromItem(item) is FlexiTabItem PlainTabItem)) continue;

                        PlainTabItem.RaiseEvent(argsClosing);
                        if (argsClosing.Cancel) return;

                        PlainTabItem.RaiseEvent(new RoutedEventArgs(FlexiTabItem.ClosedEvent, item));
                        list.Remove(item);

                        i--;
                    }
                }

                SetCurrentValue(SelectedIndexProperty, Items.Count == 0 ? -1 : 0);
            }

            internal IList GetActualList()
            {
                IList list;
                if (ItemsSource != null)
                {
                    list = ItemsSource as IList;
                }
                else
                {
                    list = Items;
                }

                return list;
            }

            protected override bool IsItemItsOwnContainerOverride(object item) => item is FlexiTabItem;

            protected override DependencyObject GetContainerForItemOverride() => new FlexiTabItem();
    }
}
