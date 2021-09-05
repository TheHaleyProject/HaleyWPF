using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Test
{
    public class SubVM:BaseVM
    {
        private string _displayMessage;
        public string DisplayMessage
        {
            get { return _displayMessage; }
            set { SetProp(ref _displayMessage, value); }
        }

        public SubVM() { DisplayMessage = "Hello World"; }
    }
}
