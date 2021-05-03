using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.Enums;
using Haley.Abstractions;

namespace Haley.Abstractions
{
    public interface IMenuItem
    {
        ICommand Command { get; set; }

        object CommandParameter { get; set; }

        string Label { get; set; }

        MenuAction Action { get; set; }

        UserControl View { get; set; }

        string ContainerKey { get; set; }
    }
}
