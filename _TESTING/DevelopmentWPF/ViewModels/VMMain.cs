using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;
using Haley.Models;

namespace DevelopmentWPF.ViewModels
{
    public class VMMain : ChangeNotifier, IHaleyWindowVM, IHaleyControlVM
    {
        private bool _ischecked;
        public bool ischecked
        {
            get { return _ischecked; }
            set {SetProp(ref _ischecked, value); }
        }


        private string _content;

        public event EventHandler<FrameClosingEventArgs> OnWindowsClosed;

        public string content
        {
            get { return _content; }
            set { _content = value; onPropertyChanged(); }
        }

        public VMMain() { ischecked = true; content = $@"this is from {nameof(VMMain)}"; }
    }
}
