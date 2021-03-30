using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Haley.WPF.BaseControls
{
    [TemplatePart(Name = UIEButtonJump, Type = typeof(TextBox))]
    [TemplatePart(Name = UIEMainPanel, Type = typeof(StackPanel))]
    public class Pagination : Control, ICornerRadius
    {
        #region Attributes
        //private const string UIEButtonLeft = "PART_btn_left";
        //private const string UIEButtonRight = "PART_btn_right";
        private const string UIEButtonJump = "PART_btn_jump";
        //private const string UIEButtonFirst = "PART_btn_first";
        //private const string UIEButtonLast = "PART_btn_last";
        private const string UIEButtonMoreLeft = "PART_btn_more_left";
        private const string UIEButtonMoreRight = "PART_btn_more_right";
        private const string UIEMainPanel = "PART_MainPanel";
        private const string UIEDirectBtnsList = "PART_direct_btns";

        private TextBox _jumpBtn;
        private StackPanel _mainPanel;
        private Button _btnMoreLeft;
        private Button _btnMoreRight;
        private ItemsControl _itemsCtrlDirectButtons;
        private ObservableCollection<int> _directPages = new ObservableCollection<int>();
        #endregion

        #region Events
        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent(nameof(PageChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Pagination));

        public event RoutedEventHandler PageChanged
        {
            add { AddHandler(PageChangedEvent, value); }
            remove { RemoveHandler(PageChangedEvent, value); }
        }
        #endregion

        #region Initiations
        static Pagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(typeof(Pagination)));
        }

        public Pagination()
        {
            _registerCommands();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _jumpBtn = GetTemplateChild(UIEButtonJump) as TextBox;
            _mainPanel = GetTemplateChild(UIEMainPanel) as StackPanel;
            _btnMoreLeft = GetTemplateChild(UIEButtonMoreLeft) as Button;
            _btnMoreRight = GetTemplateChild(UIEButtonMoreRight) as Button;
            _itemsCtrlDirectButtons = GetTemplateChild(UIEDirectBtnsList) as ItemsControl;
            _setMainPanelVisibility();
            _prepareDirectButtons();
        }
        #endregion

        #region Properties

        public PaginationMode Mode
        {
            get { return (PaginationMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(PaginationMode), typeof(Pagination), new PropertyMetadata(PaginationMode.Extended));

        public int DirectButtonsCount
        {
            get { return (int)GetValue(DirectButtonsCountProperty); }
            set { SetValue(DirectButtonsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DirectButtonsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectButtonsCountProperty =
            DependencyProperty.Register(nameof(DirectButtonsCount), typeof(int), typeof(Pagination), new PropertyMetadata(3));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Pagination), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public int ItemsCountTotal
        {
            get { return (int)GetValue(ItemsCountTotalProperty); }
            set { SetValue(ItemsCountTotalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsCountTotal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCountTotalProperty =
            DependencyProperty.Register(nameof(ItemsCountTotal), typeof(int), typeof(Pagination), new FrameworkPropertyMetadata(0, ItemsCountTotalPropertyChanged));

        public int TotalPages
        {
            get { return (int)GetValue(TotalPagesProperty); }
            set { SetValue(TotalPagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalPages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalPagesProperty =
            DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(Pagination), new FrameworkPropertyMetadata(0));

        public int ItemsCountPerPage
        {
            get { return (int)GetValue(ItemsCountPerPageProperty); }
            set { SetValue(ItemsCountPerPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsCountPerPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCountPerPageProperty =
            DependencyProperty.Register(nameof(ItemsCountPerPage), typeof(int), typeof(Pagination), new FrameworkPropertyMetadata(10, ItemsCountPerPagePropertyChanged));

        public bool HideCounter
        {
            get { return (bool)GetValue(HideCounterProperty); }
            set { SetValue(HideCounterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideCounter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideCounterProperty =
            DependencyProperty.Register(nameof(HideCounter), typeof(bool), typeof(Pagination), new PropertyMetadata(false));

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pagination), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CurrentPagePropertyChanged));

        public Brush PrimaryColor
        {
            get { return (Brush)GetValue(PrimaryColorProperty); }
            set { SetValue(PrimaryColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrimaryColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrimaryColorProperty =
            DependencyProperty.Register(nameof(PrimaryColor), typeof(Brush), typeof(Pagination), new FrameworkPropertyMetadata());
        #endregion

        #region Command Methods
        private static void ItemsCountPerPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination pg = d as Pagination;
            if (pg != null)
            {
                pg.dataInitiation();
            }
        }
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination pg = d as Pagination;
            if (pg != null)
            {
                //A simple check to ensure that the user doesn't set some random values or zero.
                pg._validateCurrentPage();
                pg._prepareDirectButtons();
                //Then raise the event 
                pg.RaiseEvent(new UIRoutedEventArgs<int>(PageChangedEvent, pg) { value = pg.CurrentPage });
            }
        }
        private static void ItemsCountTotalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination pg = d as Pagination;
            if (pg != null)
            {
                pg.dataInitiation();
            }
        }
        void ExecuteCommand_PreviousPage(object sender, ExecutedRoutedEventArgs e)
        {
            //Current page should always be greater than one.
            var newpage = CurrentPage - 1;
            if (newpage > 0) this.SetCurrentValue(CurrentPageProperty, newpage);
        }
        void ExecuteCommand_NextPage(object sender, ExecutedRoutedEventArgs e)
        {
            //Current page should always be lesser than the total pages.
            var newpage = CurrentPage + 1;
            if (newpage < TotalPages + 1) this.SetCurrentValue(CurrentPageProperty, newpage);
        }
        void ExecuteCommand_GoToPage(object sender, ExecutedRoutedEventArgs e)
        {
            //if the gotopage is with in the range, go to that page.
            //The plaintext box is constrained to have only integers.

            int newindex;
            if (e.Parameter is int)
            {
                newindex = (int)e.Parameter;
            }
            else
            {
                var input = (string)e.Parameter;
                if (string.IsNullOrEmpty(input)) return;
                newindex = int.Parse(input);
            }

            if (newindex > 0 && newindex < TotalPages + 1)
            {
                this.SetCurrentValue(CurrentPageProperty, newindex);
                //Then clear the textbox
                _jumpBtn.Text = string.Empty;
            }
        }

        void ExecuteCommand_GotoFirstPage(object sender, ExecutedRoutedEventArgs e)
        {
            //Goto first page
            this.SetCurrentValue(CurrentPageProperty, 1);
        }
        void ExecuteCommand_GoToLastPage(object sender, ExecutedRoutedEventArgs e)
        {
            //Goto last page
            this.SetCurrentValue(CurrentPageProperty, TotalPages);
        }
        void ExecuteCommand_ChangeCount(object sender, ExecutedRoutedEventArgs e)
        {
            //We now change the count but one is minimum
            //The plaintext box is constrained to have only integers.
            var input = (string)e.Parameter;
            if (string.IsNullOrEmpty(input)) return;
            var newindex = int.Parse(input);

            if (newindex > 0)
            {
                this.SetCurrentValue(ItemsCountPerPageProperty, newindex);
            }
        }
        #endregion

        #region Helper Methods
        void _validateCurrentPage()
        {
            //Current page should be above zero. 
            //Current page should be below total pages.
            if (CurrentPage < 1 || CurrentPage > TotalPages)
            {
                this.SetCurrentValue(CurrentPageProperty, 1);
            }
        }
        private void _registerCommands()
        {
            //Add command bindings for this pagination control
            CommandBindings.Add(new CommandBinding(NavigationCommands.PreviousPage, ExecuteCommand_PreviousPage));
            CommandBindings.Add(new CommandBinding(NavigationCommands.NextPage, ExecuteCommand_NextPage));
            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, ExecuteCommand_GoToPage));
            CommandBindings.Add(new CommandBinding(NavigationCommands.FirstPage, ExecuteCommand_GotoFirstPage));
            CommandBindings.Add(new CommandBinding(NavigationCommands.LastPage, ExecuteCommand_GoToLastPage));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.ChangeCount, ExecuteCommand_ChangeCount));
        }
        void dataInitiation()
        {
            int remainder_items = 0;
            //Get the remainder after dividing
            Math.DivRem(ItemsCountTotal, ItemsCountPerPage, out remainder_items);
            var _tot_pages = ItemsCountTotal / ItemsCountPerPage;
            this.SetCurrentValue(TotalPagesProperty, _tot_pages);
            if (remainder_items != 0)
            {
                this.SetCurrentValue(TotalPagesProperty, TotalPages + 1);
                //Increment by 1. Because, whatever the remainder is can be accommdaed in a single page.
            }
            this.SetCurrentValue(CurrentPageProperty, 1); //When current page is set, direct buttons are prepared.

            _setMainPanelVisibility(); //check and set ui visibilities.
            _prepareDirectButtons();
        }
        void _setMainPanelVisibility()
        {
            //CHECK IF MAIN PANEL NEEDS TO BE SHOWN OR NOT..
            if (_mainPanel == null) return;

            if (TotalPages == 1)
            {
                _mainPanel.Visibility = Visibility.Collapsed; //If pages is just one, then better hide the main panel. If visibility is set directly, then it will affect whole pagination control
            }
            else
            {
                _mainPanel.Visibility = Visibility.Visible;
            }
        }
        void _prepareMoreButtons()
        {
            if (_btnMoreLeft != null && _btnMoreRight != null)
            {
                if (_directPages == null || _directPages.Count == 0)
                {
                    _btnMoreLeft.Visibility = Visibility.Collapsed;
                    _btnMoreRight.Visibility = Visibility.Collapsed;
                    return;
                }

                //If compare directpages values for turning on/off more buttons
                int first_directpage, last_directpage;
                first_directpage = _directPages.First();
                last_directpage = _directPages.Last();

                //For left
                if (first_directpage > 2) //First button 1
                {
                    //Show the more left button
                    _btnMoreLeft.Visibility = Visibility.Visible;
                }
                else
                {
                    _btnMoreLeft.Visibility = Visibility.Collapsed;
                }

                //For right
                if (last_directpage != (TotalPages - 1)) //Because we show lastbutton already
                {
                    //Show the more left button
                    _btnMoreRight.Visibility = Visibility.Visible;
                }
                else
                {
                    _btnMoreRight.Visibility = Visibility.Collapsed;
                }
            }
        }
        void _prepareDirectButtons()
        {
            //PREPARE THE DIRECT BUTTONS
            if (_itemsCtrlDirectButtons == null) return; //If we change mode, we won't get this control.
            //If current page is already in the direct page, or else we only thave  then don't prepare again.
            if (_directPages.Contains(CurrentPage)) return;

            //Now, based on the current page and also the total pages, prepare the direct buttons.
            //Let us assume, direct buttons needs to be 3 numbers
            _directPages = new ObservableCollection<int>();

            //Set the limits
            int _startCount, _maxCount, _allowedButtons;
            _startCount = CurrentPage; //Starting count
            _allowedButtons = DirectButtonsCount; //Number of buttons allowed to be created

            //If directbuttonscount is greater than totalpages, then reset it
            if (DirectButtonsCount > TotalPages)
            {
                _allowedButtons = TotalPages - 2; //Subtracting first and last button count
                //If we dnt' have atleast one allowed button, then don't prceed because we dont need to show them anymore. Meaning, we need one first, one last button and then min one button to show up
                if (_allowedButtons < 1)
                {
                    _itemsCtrlDirectButtons.ItemsSource = _directPages;
                    _prepareMoreButtons();
                    return; //Don't proceed.
                }
            }

            //In case our current page is closer to the extreme right, check the differences
            if ((TotalPages - _startCount) < _allowedButtons)
            {
                _startCount = TotalPages - _allowedButtons; //Reset start count but ensure that it doesn't go below 1
                if ((_startCount == 0 || _startCount == 1) && TotalPages > 2)
                {
                    _startCount = 2;
                    if (CurrentPage == 1) _startCount = 1;
                }
            }

            _maxCount = _startCount + _allowedButtons; //Possible maximum count


            for (int i = _startCount; i < TotalPages && i < _maxCount; i++)
            {
                int newvalue = i;

                //When we are extreme left
                if (CurrentPage == 1) newvalue = i + 1;
                if (newvalue == TotalPages) break; //We should never add the last page as direct button because we already have a dedicated button for that purpose
                _directPages.Add(newvalue);
            }

            _itemsCtrlDirectButtons.ItemsSource = _directPages;

            _prepareMoreButtons();
        }
        #endregion
    }
}
