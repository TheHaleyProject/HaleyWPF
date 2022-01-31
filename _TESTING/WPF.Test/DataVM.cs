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
    public class DataVM :BaseVM
    {
        private object _commonView;
        public object CommonView
        {
            get { return _commonView; }
            set { SetProp(ref _commonView, value,_callback,true); }
        }

        private bool _callback(object arg1, object arg2)
        {
            return true;
        }

        #region SingletonImplementation
        public static DataVM Singleton = new DataVM();
        public static DataVM getSingleton()
        {
            if (Singleton == null) Singleton = new DataVM();
            return Singleton;
        }

        public static void Clear()
        {
            Singleton = new DataVM();
        }
        private DataVM() { CommonView = "testkitty2"; }

        #endregion

    }
}
