﻿using Haley.Abstractions;
using Haley.Events;
using Haley.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Haley.WPF.BaseControls
{
    public class PlainListView : ListView, ICornerRadius, IItemsSelection
    {
        private bool collectionChanging;
        static PlainListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainListView), new FrameworkPropertyMetadata(typeof(PlainListView)));
        }

        public PlainListView()
        {
            collectionChanging = false;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll, ExecuteSelectAll));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //SetCurrentValue(SelectedItemsProperty, new ObservableCollection<object>());
        }

        //void RaiseSelectionChanged()
        //{
        //    RaiseEvent(new UIRoutedEventArgs<IEnumerable>(SelectionChangedEvent, this) { value = SourceSelectedItems });
        //}

        public Visibility ControlAreaVisibility
        {
            get { return (Visibility)GetValue(ControlAreaVisibilityProperty); }
            set { SetValue(ControlAreaVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ControlAreaVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlAreaVisibilityProperty =
            DependencyProperty.Register(nameof(ControlAreaVisibility), typeof(Visibility), typeof(PlainListView), new PropertyMetadata(Visibility.Collapsed));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainListView), new PropertyMetadata(ResourceHelper.cornerRadius));

        public Brush ItemSelectedColor
        {
            get { return (Brush)GetValue(ItemSelectedColorProperty); }
            set { SetValue(ItemSelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedColorProperty =
            DependencyProperty.Register(nameof(ItemSelectedColor), typeof(Brush), typeof(PlainListView), new PropertyMetadata(null));

        public Brush ItemHoverColor
        {
            get { return (Brush)GetValue(ItemHoverColorProperty); }
            set { SetValue(ItemHoverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemHoverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemHoverColorProperty =
            DependencyProperty.Register(nameof(ItemHoverColor), typeof(Brush), typeof(PlainListView), new PropertyMetadata(null));


        /// <summary>
        /// Always bind observablecollection<Object>. Else it will return null.
        /// </summary>
        public new IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(PlainListView), new FrameworkPropertyMetadata(default(IList), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,new PropertyChangedCallback(SelectedItemsPropertyChanged)));

        //When base selected items gets changed, add it to this as well.
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (collectionChanging) return;
            IList choosen = new ObservableCollection<object>();
            foreach (var item in base.SelectedItems)
            {
                choosen.Add(item);
            }
        }
        void ExecuteSelectAll(object sender, ExecutedRoutedEventArgs e)
        {
            //Select or unselect all
            if (e.Parameter is bool shouldSelectAll)
            {
                if(shouldSelectAll)
                {
                    //Select all
                    this.SelectAll();
                }
                else
                {
                    //Unselect all
                    this.UnselectAll();
                }
            }
        }
        static void SelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                //Based on the new values, set the base value as well
                PlainListView pview = d as PlainListView;
                ListView base_view = (ListView)pview;
                if (pview.SelectionMode == SelectionMode.Single) return; //We don't set selected items when selectio mode is sinlge
                if (pview == null || base_view == null || pview.Items == null || e.NewValue == null) return;

                IList newvalues = new List<object>();
                //Get new newvalues that are available in the itemssource.
                foreach (var item in e.NewValue as IList<object>)
                {
                    if (pview.Items.Contains(item))
                    {
                        newvalues.Add(item);
                    }
                }

                if (newvalues.Count == 0)
                {
                    base_view.SelectedItems.Clear();
                    return;
                }

                //Get unique values to add.
                IList uniquevalues = new List<object>();
                if (base_view.SelectedItems != null)
                {
                    foreach (var item in newvalues)
                    {
                        if (!base_view.SelectedItems.Contains(item))
                        {
                            uniquevalues.Add(item);
                        }
                    }
                }

                if (uniquevalues.Count == 0) return;

                pview.collectionChanging = true;
                //Compare and get only new items
                foreach (var item in uniquevalues)
                {
                    base_view.SelectedItems.Add(item);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                //Based on the new values, set the base value as well
                PlainListView pview = d as PlainListView;
                pview.collectionChanging = false;
            }
        }
    }
}
