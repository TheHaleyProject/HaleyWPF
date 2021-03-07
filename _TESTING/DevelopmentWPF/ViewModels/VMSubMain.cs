using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using System.ComponentModel;
using Haley.Models;
using Haley.Events;
using System.Windows.Input;

namespace DevelopmentWPF.ViewModels
{
    public class VMSubMain : ChangeNotifier, IHaleyControlVM
    {

        private bool _ischecked;
        public bool ischecked
        {
            get { return _ischecked; }
            set { _ischecked = value; onPropertyChanged(); }
        }

        private string _content;
        public event EventHandler<FrameClosingEventArgs> OnControlClosed;

        public string content
        {
            get { return _content; }
            set { _content = value; onPropertyChanged(); }
        }
        public VMSubMain() { ischecked = false; content = $@"this is from {nameof(VMSubMain)}"; }
    }
}
