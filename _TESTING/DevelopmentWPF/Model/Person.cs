using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevelopmentWPF.Model
{
    public class Person :ChangeNotifier
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { SetProp(ref _name, value); }
        }

        private string _key;
        public string key
        {
            get { return _key; }
            set { SetProp(ref _key, value); }
        }

        public override string ToString()
        {
            return this.name + this.key;
        }

        public Person(string title, string titlekey) { name = title; key = titlekey; }
    }
}
