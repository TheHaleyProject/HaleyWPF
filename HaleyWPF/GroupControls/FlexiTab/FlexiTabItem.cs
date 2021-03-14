using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Haley.Events;
using System.Windows.Controls;

namespace Haley.WPF.GroupControls
{
    public class FlexiTabItem : TabItem
    {
            private const int AnimationSpeed = 150;
            private static bool ItemIsDragging;
            private bool _isWaiting;
            private Point _dragPoint;
            private int _mouseDownIndex;
            private double _mouseDownOffsetX;
            private Point _mouseDownPoint;
            private double _maxMoveRight;
            private double _maxMoveLeft;
            public double ItemWidth { get; internal set; }
            private const double WaitLength = 20;
            private bool _isDragging;
            private bool _isDragged;
            internal double TargetOffsetX { get; set; }
            private int _currentIndex;
            private double _scrollHorizontalOffset;
            private FlexiTabPanel _tabPanel;
            internal FlexiTabPanel PlainTabPanel
            {
                get
                {
                    if (_tabPanel == null && TabControlParent != null)
                    {
                        _tabPanel = TabControlParent.HeaderPanel;
                    }

                    return _tabPanel;
                }
                set => _tabPanel = value;
            }
            internal int CurrentIndex
            {
                get => _currentIndex;
                set
                {
                    if (_currentIndex == value || value < 0) return;
                    var oldIndex = _currentIndex;
                    _currentIndex = value;
                    UpdateItemOffsetX(oldIndex);
                }
            }
            public static readonly DependencyProperty CanCloseProperty =
                FlexiTab.CanCloseProperty.AddOwner(typeof(FlexiTabItem));

            public bool CanClose
            {
                get => (bool)GetValue(CanCloseProperty);
                set => SetValue(CanCloseProperty, value);
            }

            public static void SetCanClose(DependencyObject element, bool value)
                => element.SetValue(CanCloseProperty, ValueBoxes.BooleanBox(value));

            public static bool GetCanClose(DependencyObject element)
                => (bool)element.GetValue(CanCloseProperty);

            public static readonly DependencyProperty ShowContextMenuProperty =
                FlexiTab.ShowContextMenuProperty.AddOwner(typeof(FlexiTabItem), new FrameworkPropertyMetadata(OnShowContextMenuChanged));

            private static void OnShowContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var ctl = (FlexiTabItem)d;
                if (ctl.Menu != null)
                {
                    var show = (bool)e.NewValue;
                    ctl.Menu.IsEnabled = show;
                    ctl.Menu.Show(show);
                }
            }
            public bool ShowContextMenu
            {
                get => (bool)GetValue(ShowContextMenuProperty);
                set => SetValue(ShowContextMenuProperty, ValueBoxes.BooleanBox(value));
            }

            public static void SetShowContextMenu(DependencyObject element, bool value)
                => element.SetValue(ShowContextMenuProperty, ValueBoxes.BooleanBox(value));

            public static bool GetShowContextMenu(DependencyObject element)
                => (bool)element.GetValue(ShowContextMenuProperty);

            public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(
                "Menu", typeof(ContextMenu), typeof(FlexiTabItem), new PropertyMetadata(default(ContextMenu), OnMenuChanged));

            private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var ctl = (FlexiTabItem)d;
                ctl.OnMenuChanged(e.NewValue as ContextMenu);
            }

            private void OnMenuChanged(ContextMenu menu)
            {
                if (IsLoaded && menu != null)
                {
                    var parent = TabControlParent;
                    if (parent == null) return;

                    var item = parent.ItemContainerGenerator.ItemFromContainer(this);

                    menu.DataContext = item;
                    menu.SetBinding(IsEnabledProperty, new Binding(ShowContextMenuProperty.Name)
                    {
                        Source = this
                    });
                    menu.SetBinding(VisibilityProperty, new Binding(ShowContextMenuProperty.Name)
                    {
                        Source = this,
                        Converter = ResourceHelper.GetResource<IValueConverter>(ResourceToken.Boolean2VisibilityConverter)
                    });
                }
            }

            public ContextMenu Menu
            {
                get => (ContextMenu)GetValue(MenuProperty);
                set => SetValue(MenuProperty, value);
            }
            private void UpdateItemOffsetX(int oldIndex)
            {
                if (!_isDragging) return;
                var moveItem = PlainTabPanel.ItemDic[CurrentIndex];
                moveItem.CurrentIndex -= CurrentIndex - oldIndex;
                var offsetX = moveItem.TargetOffsetX;
                var resultX = offsetX + (oldIndex - CurrentIndex) * ItemWidth;
                PlainTabPanel.ItemDic[CurrentIndex] = this;
                PlainTabPanel.ItemDic[moveItem.CurrentIndex] = moveItem;
                moveItem.CreateAnimation(offsetX, resultX);
            }

            public FlexiTabItem()
            {
                CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s, e) => Close()));
                CommandBindings.Add(new CommandBinding(ControlCommands.CloseAll,
                    (s, e) => { TabControlParent.CloseAllItems(); }));
                CommandBindings.Add(new CommandBinding(ControlCommands.CloseOther,
                    (s, e) => { TabControlParent.CloseOtherItems(this); }));

                Loaded += (s, e) => OnMenuChanged(Menu);
            }

            private FlexiTab TabControlParent => ItemsControl.ItemsControlFromItemContainer(this) as FlexiTab;

            protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
            {
                base.OnMouseRightButtonDown(e);

                if (VisualTreeHelper.HitTest(this, e.GetPosition(this)) == null) return;
                IsSelected = true;
                Focus();
            }

            protected override void OnHeaderChanged(object oldHeader, object newHeader)
            {
                base.OnHeaderChanged(oldHeader, newHeader);

                if (PlainTabPanel != null)
                {
                    PlainTabPanel.ForceUpdate = true;
                    InvalidateMeasure();
                    PlainTabPanel.ForceUpdate = true;
                }
            }

            internal void Close()
            {
                var parent = TabControlParent;
                if (parent == null) return;

                var item = parent.ItemContainerGenerator.ItemFromContainer(this);

                var argsClosing = new CancelRoutedEventArgs(ClosingEvent, item);
                RaiseEvent(argsClosing);
                if (argsClosing.Cancel) return;

                PlainTabPanel.SetValue(FlexiTabPanel.FluidMoveDurationPropertyKey, parent.IsAnimationEnabled
                        ? new Duration(TimeSpan.FromMilliseconds(200))
                        : new Duration(TimeSpan.FromMilliseconds(1)));

                parent.IsInternalAction = true;
                RaiseEvent(new RoutedEventArgs(ClosedEvent, item));

                var list = parent.GetActualList();
                list?.Remove(item);
            }

            protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
            {
                base.OnMouseLeftButtonDown(e);

                if (VisualTreeHelper.HitTest(this, e.GetPosition(this)) == null) return;
                var parent = TabControlParent;
                if (parent == null) return;

                if (parent.IsDraggable && !ItemIsDragging && !_isDragging)
                {
                    parent.UpdateScroll();
                    PlainTabPanel.SetValue(FlexiTabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromSeconds(0)));
                    _mouseDownOffsetX = RenderTransform.Value.OffsetX;
                    _scrollHorizontalOffset = parent.GetHorizontalOffset();
                    var mx = TranslatePoint(new Point(), parent).X + _scrollHorizontalOffset;
                    _mouseDownIndex = CalLocationIndex(mx);
                    var subIndex = _mouseDownIndex - CalLocationIndex(_scrollHorizontalOffset);
                    _maxMoveLeft = -subIndex * ItemWidth;
                    _maxMoveRight = parent.ActualWidth - ActualWidth + _maxMoveLeft;

                    _isDragging = true;
                    ItemIsDragging = true;
                    _isWaiting = true;
                    _dragPoint = e.GetPosition(parent);
                    _dragPoint = new Point(_dragPoint.X + _scrollHorizontalOffset, _dragPoint.Y);
                    _mouseDownPoint = _dragPoint;
                    CaptureMouse();
                }
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);

                if (ItemIsDragging && _isDragging)
                {
                    var parent = TabControlParent;
                    if (parent == null) return;

                    var subX = TranslatePoint(new Point(), parent).X + _scrollHorizontalOffset;
                    CurrentIndex = CalLocationIndex(subX);

                    var p = e.GetPosition(parent);
                    p = new Point(p.X + _scrollHorizontalOffset, p.Y);

                    var subLeft = p.X - _dragPoint.X;
                    var totalLeft = p.X - _mouseDownPoint.X;

                    if (Math.Abs(subLeft) <= WaitLength && _isWaiting) return;

                    _isWaiting = false;
                    _isDragged = true;

                    var left = subLeft + RenderTransform.Value.OffsetX;
                    if (totalLeft < _maxMoveLeft)
                    {
                        left = _maxMoveLeft + _mouseDownOffsetX;
                    }
                    else if (totalLeft > _maxMoveRight)
                    {
                        left = _maxMoveRight + _mouseDownOffsetX;
                    }

                    var t = new TranslateTransform(left, 0);
                    RenderTransform = t;
                    _dragPoint = p;
                }
            }

            protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
            {
                base.OnMouseLeftButtonUp(e);

                ReleaseMouseCapture();

                if (_isDragged)
                {
                    var parent = TabControlParent;
                    if (parent == null) return;

                    var subX = TranslatePoint(new Point(), parent).X + _scrollHorizontalOffset;
                    var index = CalLocationIndex(subX);
                    var left = index * ItemWidth;
                    var offsetX = RenderTransform.Value.OffsetX;
                    CreateAnimation(offsetX, offsetX - subX + left, index);
                }

                _isDragging = false;
                ItemIsDragging = false;
                _isDragged = false;
            }

            /// <summary>
            ///     创建动画
            /// </summary>
            internal void CreateAnimation(double offsetX, double resultX, int index = -1)
            {
                var parent = TabControlParent;

                void AnimationCompleted()
                {
                    RenderTransform = new TranslateTransform(resultX, 0);
                    if (index == -1) return;

                    var list = parent.GetActualList();
                    if (list == null) return;

                    var item = parent.ItemContainerGenerator.ItemFromContainer(this);
                    if (item == null) return;

                    PlainTabPanel.CanUpdate = false;
                    parent.IsInternalAction = true;

                    list.Remove(item);
                    parent.IsInternalAction = true;
                    list.Insert(index, item);
                    _tabPanel.SetValue(FlexiTabPanel.FluidMoveDurationPropertyKey, new Duration(TimeSpan.FromMilliseconds(0)));
                    PlainTabPanel.CanUpdate = true;
                    PlainTabPanel.ForceUpdate = true;
                    PlainTabPanel.Measure(new Size(PlainTabPanel.DesiredSize.Width, ActualHeight));
                    PlainTabPanel.ForceUpdate = false;

                    Focus();
                    IsSelected = true;

                    if (!IsMouseCaptured)
                    {
                        parent.SetCurrentValue(Selector.SelectedIndexProperty, _currentIndex);
                    }
                }

                TargetOffsetX = resultX;
                if (!parent.IsAnimationEnabled)
                {
                    AnimationCompleted();
                    return;
                }

                var animation = AnimationHelper.CreateAnimation(resultX, AnimationSpeed);
                animation.FillBehavior = FillBehavior.Stop;
                animation.Completed += (s1, e1) => AnimationCompleted();
                var f = new TranslateTransform(offsetX, 0);
                RenderTransform = f;
                f.BeginAnimation(TranslateTransform.XProperty, animation, HandoffBehavior.Compose);
            }

            /// <summary>
            ///     计算选项卡当前合适的位置编号
            /// </summary>
            /// <param name="left"></param>
            /// <returns></returns>
            private int CalLocationIndex(double left)
            {
                if (_isWaiting)
                {
                    return CurrentIndex;
                }

                var maxIndex = TabControlParent.Items.Count - 1;
                var div = (int)(left / ItemWidth);
                var rest = left % ItemWidth;
                var result = rest / ItemWidth > .5 ? div + 1 : div;

                return result > maxIndex ? maxIndex : result;
            }

            public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(EventHandler), typeof(FlexiTabItem));

            public event EventHandler Closing
            {
                add => AddHandler(ClosingEvent, value);
                remove => RemoveHandler(ClosingEvent, value);
            }

            public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(EventHandler), typeof(FlexiTabItem));

            public event EventHandler Closed
            {
                add => AddHandler(ClosedEvent, value);
                remove => RemoveHandler(ClosedEvent, value);
            }
    }
}
