﻿using Haley.Models;
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

namespace WPF.Test
{
    public class MainVM :BaseVM
    {
        #region Attributes
        private IDialogService _dialogService;
        private ColorPickerDialog _cp;
        #endregion

        #region Properties

        private Dictionary<string,Color> _SystemDefaultColors;
        public Dictionary<string,Color> SystemDefaultColors
        {
            get { return _SystemDefaultColors; }
            set { SetProp(ref _SystemDefaultColors, value); }
        }

        private ObservableCollection<Person> _soemthing;
        public ObservableCollection<Person> something
        {
            get { return _soemthing; }
            set { SetProp(ref _soemthing, value); }
        }

        private ObservableCollection<object> _selecteditems;
        public ObservableCollection<object> selecteditems
        {
            get { return _selecteditems; }
            set { SetProp(ref _selecteditems, value); }
        }

        private ObservableCollection<Person> _choosentiems;
        public ObservableCollection<Person> choosenitems
        {
            get { return _choosentiems; }
            set { SetProp(ref _choosentiems, value); }
        }

        private SolidColorBrush _selectedBrush;
        public SolidColorBrush SelectedBrush
        {
            get { return _selectedBrush; }
            set { SetProp(ref _selectedBrush, value); }
        }
        private int _counter;
        public int counter
        {
            get { return _counter; }
            set { SetProp(ref _counter, value); }
        }

        private string _proxymessageholder;
        public string proxymessageholder
        {
            get { return _proxymessageholder; }
            set { SetProp(ref _proxymessageholder, value); }
        }

        private string _selectedDisplaymode;
        public string SelectedDisplaymode
        {
            get { return _selectedDisplaymode; }
            set { SetProp(ref _selectedDisplaymode, value); }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProp(ref _isVisible, value); }
        }

        private int _currentpage;
        public int currentpage
        {
            get { return _currentpage; }
            set { SetProp(ref _currentpage, value); }
        }

        #endregion

        #region Commands
        public ICommand Cmd_Login => new DelegateCommand<PlainPasswordBox>(_login);
        public ICommand Cmd_Notify => new DelegateCommand<string>(_localNotify);
        public ICommand Cmd_Toggle => new DelegateCommand(_toggle);
        public ICommand Cmd_IncreaseCounter => new DelegateCommand(_increaseCounter);
        public ICommand Cmd_search => new DelegateCommand<string>(_search);
        public ICommand Cmd_changetheme => new DelegateCommand(_changetheme);
        public ICommand Cmd_OpenColorDialog => new DelegateCommand(_openColorDialog);

        #endregion

        #region Private Methods

        void _login(PlainPasswordBox obj)
        {
            currentpage = 0;
        }

        void _toggle()
        {
            IsVisible = !IsVisible;
        }

        void _localNotify(string obj)
        {
           if (_dialogService == null)
            {
                _dialogService = ContainerStore.Singleton.DI.Resolve<IDialogService>();
            }

            if (_dialogService == null) return;

            _dialogService.Info("Command Initiated", obj);
        }
      
        void _increaseCounter()
        {
            counter++;
        }
        void _search(string obj)
        {
            //now we search
        }

        void _changetheme()
        {
            ThemeMode _mode = ThemeLoader.Singleton.current_internal_mode;

            switch(_mode)
            {
                case ThemeMode.Dark:
                    ThemeLoader.Singleton.changeInternalTheme(ThemeMode.Normal);
                    break;
                case ThemeMode.Normal:
                    ThemeLoader.Singleton.changeInternalTheme(ThemeMode.Mild);
                    break;
                case ThemeMode.Mild:
                    ThemeLoader.Singleton.changeInternalTheme(ThemeMode.Dark);
                    break;
            }
        }
        
        void _openColorDialog()
        {
            DisplayMode dmode = DisplayMode.Mini;
            if (! string.IsNullOrEmpty(SelectedDisplaymode))
            {
                //get the corresponding enum.
                dmode = SelectedDisplaymode.getValueFromDescription<DisplayMode>();
            }
            if (_cp == null)
            {
                _cp = new ColorPickerDialog();
            }
            if (ColorPickerDialog.Favourites == null || ColorPickerDialog.Favourites.Count == 0)
            { 
                ColorPickerDialog.SetFavourites(new List<Color>() { Colors.Purple, Colors.Orange, Colors.Yellow }); //this remains static across all implementations.
            }
            
            var _res = _cp.ShowDialog(SelectedBrush, dmode);
            
            if (_res.HasValue && _res.Value)
            {
                SelectedBrush = _cp.SelectedBrush;
            }
        }
        #endregion
        public MainVM()
        {
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
        }
    }
    public class Person : ChangeNotifier
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProp(ref _Name, value); }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set { SetProp(ref _age, value); }
        }


        public Person(string name, int age)
        {
            Name = name; Age = age;
        }
    }
}
