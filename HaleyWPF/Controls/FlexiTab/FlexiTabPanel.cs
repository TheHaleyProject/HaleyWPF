using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Haley.Events;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Haley.WPF.Controls
{
    public class FlexiTabPanel : Panel
    {
        public FlexiTabPanel()
        {
            Loaded += (s, e) =>
            {
                if (_isLoaded) return;
                ForceUpdate = true;
                Measure(new Size(DesiredSize.Width, ActualHeight));
                ForceUpdate = false;
                foreach (var item in ItemDic.Values)
                {
                    item.PlainTabPanel = this;
                }
                _isLoaded = true;
            };
        }

            private int _itemCount;
            internal bool CanUpdate = true;
            internal Dictionary<int, FlexiTabItem> ItemDic = new Dictionary<int, FlexiTabItem>();

            public static readonly DependencyPropertyKey FluidMoveDurationPropertyKey =
                DependencyProperty.RegisterReadOnly("FluidMoveDuration", typeof(Duration), typeof(FlexiTabPanel),
                    new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(0))));
            public static readonly DependencyProperty FluidMoveDurationProperty =
                FluidMoveDurationPropertyKey.DependencyProperty;

            public Duration FluidMoveDuration
            {
                get => (Duration)GetValue(FluidMoveDurationProperty);
                set => SetValue(FluidMoveDurationProperty, value);
            }

            public static readonly DependencyProperty IsTabFillEnabledProperty = DependencyProperty.Register(
                "IsTabFillEnabled", typeof(bool), typeof(FlexiTabPanel), new PropertyMetadata(ValueBoxes.FalseBox));

            public bool IsTabFillEnabled
            {
                get => (bool)GetValue(IsTabFillEnabledProperty);
                set => SetValue(IsTabFillEnabledProperty, ValueBoxes.BooleanBox(value));
            }

            public static readonly DependencyProperty PlainTabItemWidthProperty = DependencyProperty.Register(
                "PlainTabItemWidth", typeof(double), typeof(FlexiTabPanel), new PropertyMetadata(200.0));

            public double PlainTabItemWidth
            {
                get => (double)GetValue(PlainTabItemWidthProperty);
                set => SetValue(PlainTabItemWidthProperty, value);
            }

            public static readonly DependencyProperty PlainTabItemHeightProperty = DependencyProperty.Register(
                "PlainTabItemHeight", typeof(double), typeof(FlexiTabPanel), new PropertyMetadata(30.0));

            public double PlainTabItemHeight
            {
                get => (double)GetValue(PlainTabItemHeightProperty);
                set => SetValue(PlainTabItemHeightProperty, value);
            }

            internal bool ForceUpdate;

            private Size _oldSize;

            private bool _isLoaded;

            protected override Size MeasureOverride(Size constraint)
            {
                if ((_itemCount == InternalChildren.Count || !CanUpdate) && !ForceUpdate && !IsTabFillEnabled) return _oldSize;
                constraint.Height = PlainTabItemHeight;
                _itemCount = InternalChildren.Count;

                var size = new Size();

                ItemDic.Clear();

                var count = InternalChildren.Count;
                if (count == 0)
                {
                    _oldSize = new Size();
                    return _oldSize;
                }
                constraint.Width += InternalChildren.Count;

                var itemWidth = .0;
                var arr = new int[count];

                if (!IsTabFillEnabled)
                {
                    itemWidth = PlainTabItemWidth;
                }
                else
                {
                    if (TemplatedParent is TabControl tabControl)
                    {
                        arr = ArithmeticHelper.DivideInt2Arr((int)tabControl.ActualWidth + InternalChildren.Count, count);
                    }
                }

                for (var index = 0; index < count; index++)
                {
                    if (IsTabFillEnabled)
                    {
                        itemWidth = arr[index];
                    }
                    if (InternalChildren[index] is FlexiTabItem PlainTabItem)
                    {
                        PlainTabItem.RenderTransform = new TranslateTransform();
                        PlainTabItem.MaxWidth = itemWidth;
                        var rect = new Rect
                        {
                            X = size.Width - PlainTabItem.BorderThickness.Left,
                            Width = itemWidth,
                            Height = PlainTabItemHeight
                        };
                        PlainTabItem.Arrange(rect);
                        PlainTabItem.ItemWidth = itemWidth - PlainTabItem.BorderThickness.Left;
                        PlainTabItem.CurrentIndex = index;
                        PlainTabItem.TargetOffsetX = 0;
                        ItemDic[index] = PlainTabItem;
                        size.Width += PlainTabItem.ItemWidth;
                    }
                }
                size.Height = constraint.Height;
                _oldSize = size;
                return _oldSize;
            }
        }
    }
