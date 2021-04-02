using Haley.Abstractions;
using Haley.Events;
using Haley.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Haley.WPF.BaseControls
{
    [TemplatePart(Name = UIESourceControl, Type = typeof(ListView))]
    [TemplatePart(Name = UIESelectionControl, Type = typeof(ListView))]
    public class CollectionSelector : ItemsControl, ICornerRadius, IItemsSelection
    {
        #region Attributes
        private const string UIESourceControl = "PART_lstvew_source";
        private const string UIESelectionControl = "PART_lstvew_selection";

        private bool _collectionChanging = false;
        private ListView _sourceControl;
        private ListView _selectionControl;
        #endregion

        #region Events
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectionChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Pagination));

        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }
        #endregion

        static CollectionSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionSelector), new FrameworkPropertyMetadata(typeof(CollectionSelector)));
        }

        public CollectionSelector()
        {
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveRight, Execute_MoveRight));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveLeft, Execute_MoveLeft));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll, Execute_SelectAll));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Highlight, Execute_Highlight));
        }

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _sourceControl = GetTemplateChild(UIESourceControl) as ListView;
            _selectionControl = GetTemplateChild(UIESelectionControl) as ListView;
        }
        void Execute_MoveRight(object sender, ExecutedRoutedEventArgs e)
        {
            //Get selected items of primary listview and add it to the itemssource of the secondary
            if (_sourceControl == null || _selectionControl == null) return;

            //The main reason for using a intermediate list is to avoid binding both listviews. Else, both becomes binded and one change immediately affects the other.

            //Get selected items.
            IList newlist = new List<object>();
            foreach (var item in _sourceControl.SelectedItems)
            {
                newlist.Add(item);
            }
            //Get existing selected items.
            IList oldvalues = new List<object>();

            if (SourceSelectedItems != null)
            {
                foreach (var item in SourceSelectedItems)
                {
                    oldvalues.Add(item);
                }
            }
            //Compare and get only new items
            foreach (var item in newlist)
            {
                if (!oldvalues.Contains(item))
                {
                    oldvalues.Add(item);
                }
            }

            SourceSelectedItems = oldvalues;
            //Clear selection.
            _sourceControl.SelectedItems.Clear();
            RaiseSelectionChanged();
        }
        void Execute_MoveLeft(object sender, ExecutedRoutedEventArgs e)
        {
            //Get selected items of primary listview and add it to the itemssource of the secondary
            if (_sourceControl == null || _selectionControl == null) return;

            //Get selected items in Selection Host
            IList _toRemove = new List<object>();
            foreach (var item in _selectionControl.SelectedItems)
            {
                _toRemove.Add(item);
            }
            //Get existing selected items.
            IList oldvalues = new List<object>();
            if (SourceSelectedItems != null)
            {
                foreach (var item in SourceSelectedItems)
                {
                    oldvalues.Add(item);
                }
            }
            IList newvalues = new List<object>();
            //Compare and get only new items to remove
            foreach (var item in oldvalues)
            {
                if (!_toRemove.Contains(item))
                {
                    newvalues.Add(item);
                }
            }

            SourceSelectedItems = newvalues;
            //Clear selection.
            _selectionControl.SelectedItems.Clear();
            RaiseSelectionChanged();
        }
        void RaiseSelectionChanged()
        {
            RaiseEvent(new UIRoutedEventArgs<IEnumerable>(SelectionChangedEvent, this) { value = SourceSelectedItems });
        }
        void Execute_SelectAll(object sender, ExecutedRoutedEventArgs e)
        {
            if ((string)e.Parameter == "Source")
            {
                _sourceControl.SelectAll();
            }
            else
            {
                _selectionControl.SelectAll();
            }
        }
        void Execute_Highlight(object sender, ExecutedRoutedEventArgs e)
        {
            //Get all selected items
            //Get selected items in Selection Host
            IList _tohighlight = new List<object>();
            if (SourceSelectedItems == null) return;
            foreach (var item in SourceSelectedItems)
            {
                _tohighlight.Add(item);
            }

            _sourceControl.SelectedItems.Clear();
            foreach (var item in _tohighlight)
            {
                _sourceControl.SelectedItems.Add(item);
            }
        }
        static void SourceSelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollectionSelector _selector = d as CollectionSelector;
            if (_selector == null) return;
            if (_selector._collectionChanging) return;
            //Use this value, create a list and bind to the choosen items.
            IList _choosenlist = new ObservableCollection<object>();
            foreach (var item in _selector.SourceSelectedItems)
            {
                _choosenlist.Add(item);
            }
            _selector.SetCurrentValue(SelectedItemsProperty, _choosenlist);
        }
        static void SelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                //The selected items of this collection selector is base selected items.
                CollectionSelector selector = d as CollectionSelector;
                //Get selected items of primary listview and add it to the itemssource of the secondary
                if (selector == null || e.NewValue == null || selector.Items == null) return;

                //Get only available predefined items.
                IList newlist = new List<object>();
                foreach (var item in e.NewValue as IList<object>)
                {
                    if (selector.Items.Contains(item))
                    {
                        //Ensure that this new item is present in the itemssource
                        newlist.Add(item);
                    }
                }

                selector._collectionChanging = true;
                //Now that we have got the new values (that are actually a part of the itemssource), we do not need to validate anyfurther. Since this data is coming from viewmodel or code-behind, we directly set them.
                selector.SourceSelectedItems = newlist;
                //Clear selection.
                if (selector._sourceControl != null)
                {
                    selector._sourceControl.SelectedItems.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CollectionSelector selector = d as CollectionSelector;
                selector._collectionChanging = false;
            }
        }

        #endregion

        #region Properties
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CollectionSelector), new PropertyMetadata(ResourceHelper.cornerRadius));

        public Brush ItemSelectedColor
        {
            get { return (Brush)GetValue(ItemSelectedColorProperty); }
            set { SetValue(ItemSelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedColorProperty =
            DependencyProperty.Register(nameof(ItemSelectedColor), typeof(Brush), typeof(CollectionSelector), new PropertyMetadata(null));

        public Brush ItemHoverColor
        {
            get { return (Brush)GetValue(ItemHoverColorProperty); }
            set { SetValue(ItemHoverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemHoverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemHoverColorProperty =
            DependencyProperty.Register(nameof(ItemHoverColor), typeof(Brush), typeof(CollectionSelector), new PropertyMetadata(null));

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(CollectionSelector), new PropertyMetadata(null));

        public IEnumerable SourceSelectedItems
        {
            get { return (IEnumerable)GetValue(SourceSelectedItemsProperty); }
            set { SetValue(SourceSelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceSelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceSelectedItemsProperty =
            DependencyProperty.Register(nameof(SourceSelectedItems), typeof(IEnumerable), typeof(CollectionSelector), new FrameworkPropertyMetadata(default(IEnumerable), FrameworkPropertyMetadataOptions.NotDataBindable, SourceSelectedItemsPropertyChanged));

        /// <summary>
        /// Always bind observablecollection<Object>. Else it will return null.
        /// </summary>
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(CollectionSelector), new FrameworkPropertyMetadata(default(IList), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedItemsPropertyChanged)));
        #endregion

    }
}
