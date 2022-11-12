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

namespace WPF.Test.Models {
    public class IconInfo:ChangeNotifier  {

        private Enum _source;
        public Enum Source {
            get { return _source; }
            set { SetProp(ref _source, value); }
        }
        public string Name { get; set; }
        public string Type { get; set; }

        public IconInfo() {

        }
    }
}
