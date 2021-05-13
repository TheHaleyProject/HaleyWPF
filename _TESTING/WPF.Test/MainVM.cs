using Haley.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Haley.WPF.BaseControls;
using System;
using Haley.Enums;
using Haley.Utils;
using Haley.Abstractions;
using Haley.IOC;
using Haley.MVVM;

namespace WPF.Test
{
    public class MainVM : ChangeNotifier, IHaleyControlVM
    {
        private IDialogService _dialogService;
        private int _counter;
        public int counter
        {
            get { return _counter; }
            set { SetProp(ref _counter, value); }
        }

        void _login(PlainPasswordBox obj)
        {
            currentpage = 0;
        }

        private int _currentpage;
        public int currentpage
        {
            get { return _currentpage; }
            set { SetProp(ref _currentpage, value); }
        }

        public ICommand Cmd_Login => new DelegateCommand<PlainPasswordBox>(_login);
        void _localNotify(string obj)
        {
           if (_dialogService == null)
            {
                _dialogService = ContainerStore.Singleton.DI.Resolve<IDialogService>();
            }

            if (_dialogService == null) return;

            _dialogService.send("Command Initiated", obj);
        }
        public ICommand Cmd_Notify => new DelegateCommand<string>(_localNotify);
        void _increaseCounter()
        {
            counter++;
        }
        public ICommand Cmd_IncreaseCounter => new DelegateCommand(_increaseCounter);
        void _search(string obj)
        {
            //now we search
        }
        public ICommand Cmd_search => new DelegateCommand<string>(_search);

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
        public ICommand Cmd_changetheme => new DelegateCommand(_changetheme);

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
