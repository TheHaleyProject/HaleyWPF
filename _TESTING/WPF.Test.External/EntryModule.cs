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

namespace WPF.Test.External
{
    public static class EntryModule
    {
        private static bool _initiated;
        
        public static void Initiate(IThemeService ts)
        {
            if(!_initiated)
            {
                ts.Register(new AssemblyThemeBuilder()
                    .Add("Theme1", new Uri("pack://application:,,,/WPF.Test.External;component/Resources/ThemeDark.xaml", UriKind.RelativeOrAbsolute))
                    .Add("Theme3", new Uri("pack://application:,,,/WPF.Test.External;component/Resources/ThemeLight.xaml", UriKind.RelativeOrAbsolute)));
                _initiated = true;
            }
        }
    }
}
