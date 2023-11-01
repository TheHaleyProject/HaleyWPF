using Haley.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Haley.WPF.Controls;
using System;
using Haley.Enums;
using Haley.Utils;
using Haley.Abstractions;
using Haley.IOC;
using Haley.MVVM;
using Haley.Events;
using System.Windows.Media;
using Haley.Services;
using System.Collections.Generic;
using WPF.Test.Models;
using System.Diagnostics;

namespace WPF.Test
{
    public class MainVM :BaseVM
    {
        #region Attributes
        private IColorPickerService _cp;
        private IDialogService _dialogService;
        private IThemeService _ts;
        #endregion

        #region Properties

        private ObservableCollection<Person> _choosentiems;
        private int _counter;
        private int _currentpage;
        private bool _isVisible;
        private string _proxymessageholder;
        private SolidColorBrush _selectedBrush;
        private string _selectedDisplaymode;
        private ObservableCollection<object> _selecteditems;
        private string _selectedText;
        private ObservableCollection<Person> _soemthing;
        private Dictionary<string, Color> _SystemDefaultColors;
        public ObservableCollection<Person> choosenitems {
            get { return _choosentiems; }
            set { SetProp(ref _choosentiems, value); }
        }

        public int counter {
            get { return _counter; }
            set { SetProp(ref _counter, value); }
        }

        public int currentpage {
            get { return _currentpage; }
            set { SetProp(ref _currentpage, value); }
        }

        public bool IsVisible {
            get { return _isVisible; }
            set { SetProp(ref _isVisible, value); }
        }

        public Func<object, string, bool> PersonFilter { get; set; }
        public string proxymessageholder {
            get { return _proxymessageholder; }
            set { SetProp(ref _proxymessageholder, value); }
        }

        public SolidColorBrush SelectedBrush {
            get { return _selectedBrush; }
            set { SetProp(ref _selectedBrush, value); }
        }

        public string SelectedDisplaymode {
            get { return _selectedDisplaymode; }
            set { SetProp(ref _selectedDisplaymode, value); }
        }

        public ObservableCollection<object> selecteditems {
            get { return _selecteditems; }
            set { SetProp(ref _selecteditems, value); }
        }

        public string SelectedText {
            get { return _selectedText; }
            set { SetProp(ref _selectedText, value); }
        }
        public ObservableCollection<Person> something {
            get { return _soemthing; }
            set { SetProp(ref _soemthing, value); }
        }

        public Dictionary<string, Color> SystemDefaultColors {
            get { return _SystemDefaultColors; }
            set { SetProp(ref _SystemDefaultColors, value); }
        }
        #endregion

        private IconInfo _basesource;
        Random _random = new Random();
        Random _randomBase = new Random();
        IconInfo _source;
        Array _values = Enum.GetValues(typeof(BrandKind));
        Array _valuesBase = Enum.GetValues(typeof(IconKind));
        public MainVM() {
            ObservableCollection<Person> hello = new ObservableCollection<Person>();
            hello.Add(new Person("Senguttuvan", 32));
            hello.Add(new Person("Latha", 32));
            hello.Add(new Person("Bhadri", 32));
            hello.Add(new Person("Pranav", 32));
            hello.Add(new Person("Mahalingam", 32));
            hello.Add(new Person("Ramasamy", 32));
            hello.Add(new Person("Buna", 32));
            something = hello;
            selecteditems = null;
            choosenitems = new ObservableCollection<Person>();
            selecteditems = new ObservableCollection<object>();
            selecteditems.Add(hello[0]);
            selecteditems.Add(hello[3]);
            proxymessageholder = "New test from proxy binding. Success";
            IsVisible = false;
            SelectedBrush = new SolidColorBrush(Colors.PaleVioletRed);
            SystemDefaultColors = new Dictionary<string, Color>();
            SystemDefaultColors = ColorUtils.GetSystemColors();
            PersonFilter = _filterSearch;
            _ts = ThemeService.Singleton;
            //CmdRelayCheck = new DelegateCommandBase(_commandCheck, _relayChecker);
            CmdRelayCheck = new DelegateCommandBase<bool>((p) => _commandCheck(), _relayChecker2);

        }

        public IconInfo BasePack {
            get { return _basesource; }
            set { SetProp(ref _basesource, value); }
        }

        public IconInfo IconsPack {
            get { return _source; }
            set {
                _source = value;
                OnPropertyChanged(); //Raise notification
            }
        }

        private bool _flag1;
        public bool Flag1 {
            get { return _flag1; }
            set { SetProp(ref _flag1, value); }
        }

        #region Commands
        public ICommand Cmd_ChangeContainerKey => new DelegateCommand<object>(_changeContainerView);
        public ICommand Cmd_changetheme => new DelegateCommand(_changetheme);
        public ICommand Cmd_GetRandomImage => new DelegateCommand<bool>(_getRandomImage);
        public ICommand Cmd_IncreaseCounter => new DelegateCommand(_increaseCounter);
        public ICommand Cmd_Login => new DelegateCommand<PlainPasswordBox>(_login);
        public ICommand Cmd_MouseHover => new DelegateCommand<object>(_mouseOver);
        public ICommand Cmd_Notify => new DelegateCommand<string>(_localNotify);
        public ICommand Cmd_OpenColorDialog => new DelegateCommand(_openColorDialog);
        public ICommand Cmd_search => new DelegateCommand<string>(_search);
        public ICommand Cmd_Toggle => new DelegateCommand(_toggle);
        public ICommand CmdRelayCheck { get; set; } 
        public ICommand CmdDirectRelayInvoke => new DelegateCommandBase(raiseRelay);

        private void raiseRelay() {
            //var cmd = CmdRelayCheck as DelegateCommandBase;
            var cmd = CmdRelayCheck as DelegateCommandBase<bool>;
            if (cmd == null) return;
            cmd?.Invalidate();
        }

        private void _commandCheck() {
           //
        }

        private bool _relayChecker() {
            return Flag1;
        }

        bool _relayChecker2(bool result) {
            return result;
        }

        private bool _delegatechecker() {
            return Flag1;
        }

        private bool _delegatechecker2(bool flag) {
            return flag;
        }

        //public ICommand CmdDelegateCheck => new DelegateCommand(_commandCheck,_delegatechecker);
        public ICommand CmdDelegateCheck => new DelegateCommand<bool>((p)=>_commandCheck(),_delegatechecker2);

        public ICommand CmdMakePropsNull => new DelegateCommand(() => { 
            BasePack = null;
            IconsPack = null; 
        });

        private void _getRandomImage(bool obj) {
            var newinfo = new IconInfo() { };
            if (obj) {
                newinfo.Source = (IconKind)_valuesBase.GetValue(_randomBase.Next(_valuesBase.Length));
                BasePack = newinfo;
            } else {
                newinfo.Source = (BrandKind)_values.GetValue(_random.Next(_values.Length));
                IconsPack = newinfo;
            }
        }

        private void _mouseOver(object obj) {
            //
        }
        #endregion

        #region Private Methods
        private void _changeContainerView(object obj)
        {
            DataVM.Singleton.CommonView = obj; //Setting the value to a singleton class.
        }
        void _changetheme() {
            switch (_ts.ActiveTheme) {
                case null:
                case "Theme1":
                    //If null, assume we are already at startuptheme.
                    _ts.ChangeTheme("Theme2");
                    break;
                case "Theme2":
                    _ts.ChangeTheme("Theme3");
                    break;
                case "Theme3":
                    _ts.ChangeTheme("Theme1");
                    break;
            }
        }

        void _increaseCounter() {
            counter++;
        }

        void _localNotify(string obj) {
            if (_dialogService == null) {
                _dialogService = ContainerStore.DI.Resolve<IDialogService>();
            }

            if (_dialogService == null) return;

            _dialogService.Info("Command Initiated", obj);
        }

        void _login(PlainPasswordBox obj) {
            currentpage = 0;
        }

        void _openColorDialog() {
            DisplayMode dmode = DisplayMode.Mini;
            if (!string.IsNullOrEmpty(SelectedDisplaymode)) {
                //get the corresponding enum.
                dmode = SelectedDisplaymode.GetValueFromDescription<DisplayMode>();
            }
            if (_cp == null) {
                _cp = new ColorPickerDialog();
            }
            if (ColorPickerDialog.Favourites == null || ColorPickerDialog.Favourites.Count == 0) {
                ColorPickerDialog.SetFavourites(new List<Color>() { Colors.Purple, Colors.Orange, Colors.Yellow }); //this remains static across all implementations.
            }

            var _res = _cp.ShowDialog(SelectedBrush, dmode);

            if (_res.HasValue && _res.Value) {
                SelectedBrush = _cp.SelectedBrush;
            }
        }

        void _search(string obj) {
            //now we search
        }

        void _toggle() {
            IsVisible = !IsVisible;
        }
        #endregion
        private bool _filterSearch(object item, string filter_key)
        {
            if (item is Person p)
            {
                return p.Name.ToLower().StartsWith(filter_key.ToLower());
            }
            return true;
        }
    }

   
    public class Person : ChangeNotifier
    {
        private int _age;
        private string _Name;
        public Person(string name, int age) {
            Name = name; Age = age;
        }

        public int Age {
            get { return _age; }
            set { SetProp(ref _age, value); }
        }

        public string Name {
            get { return _Name; }
            set { SetProp(ref _Name, value); }
        }
    }
}
