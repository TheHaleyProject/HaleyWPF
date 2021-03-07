using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Models;
using Haley.MVVM;
using Haley.Abstractions;
using DevelopmentWPF.ViewModels;
using System.ComponentModel;
using Haley.Events;
using System.Windows.Input;
using System.Collections.ObjectModel;
using DevelopmentWPF.Model;
using Haley.Utils;

namespace DevelopmentWPF
{
    public class CoreVM : ChangeNotifier, IHaleyWindowVM
    {
        private TestApp _controlEnum;
        public TestApp controlEnum
        {
            get { return _controlEnum; }
            set { _controlEnum = value; onPropertyChanged(); }
        }

        private IHaleyControlVM _current_viewModel;
        public IHaleyControlVM current_viewModel
        {
            get { return _current_viewModel; }
            set { _current_viewModel = value; onPropertyChanged(); }
        }

        private ObservableCollection<Person> _personCollection;
        public ObservableCollection<Person> personCollection
        {
            get { return _personCollection; }
            set { SetProp(ref _personCollection, value); }
        }

        public ICommand EmptyCommand => new DelegateCommand(_emptyCommand, canemptycommand);
        public ICommand NewCommand => new DelegateCommand<string>(_newcommand, cannewcommand);

        private void _newcommand(string obj)
        {
            //Test me
            personCollection.Add(new Person("Pranav" , HashUtils.getRandomString(256)));
        }
        bool cannewcommand(string arg)
        {
            return true;
        }

        private void _emptyCommand()
        {
            //This is empty.
            personCollection.FirstOrDefault().key = HashUtils.getRandomString(256);
        }
        bool canemptycommand()
        {
            return true;
        }

        private bool _ischecked;
        public bool ischecked
        {
            get { return _ischecked; }
            set { _ischecked = value; onPropertyChanged(); }
        }

        private string _content;

        public event EventHandler<FrameClosingEventArgs> OnWindowsClosed;

        public string content
        {
            get { return _content; }
            set { _content = value; onPropertyChanged(); }
        }

        public CoreVM()
        {
            ischecked = false;
            content = $@"this is from {nameof(CoreVM)}";
            current_viewModel = null;
            //this.PropertyChanged += CoreVM_PropertyChanged;
            personCollection = new ObservableCollection<Person>();
            personCollection.Add(new Person("Senguttuvan", HashUtils.getRandomString(256)));
            personCollection.Add(new Person("Bhadri", HashUtils.getRandomString(256)));
            personCollection.Add(new Person("Latha", HashUtils.getRandomString(256)));
        }

        private void CoreVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(controlEnum))
            {
                var _vm = ContainerStore.Singleton.controls.generateViewModel(controlEnum,Haley.Enums.ResolveMode.Transient);
                current_viewModel = _vm;
            }
        }
    }
}
