﻿using System;
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

namespace Haley.WPF.Controls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class CommandMenuItem : DependencyObject, ICommandMenuItem
    {
        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(nameof(CommandName), typeof(string), typeof(CommandMenuItem), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(CommandMenuItem), new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(CommandMenuItem), new FrameworkPropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(CommandMenuItem), new FrameworkPropertyMetadata(defaultValue: "Button"));

        public static readonly DependencyProperty RemoveIconProperty =
            DependencyProperty.Register("RemoveIcon", typeof(bool), typeof(CommandMenuItem), new PropertyMetadata(false));

        private string _id;
        public CommandMenuItem() { UId = Guid.NewGuid().ToString(); }

        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public string CommandName {
            get { return (string)GetValue(CommandNameProperty); }
            set { SetValue(CommandNameProperty, value); }
        }

        public object CommandParameter {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public string Id {
            get {
                //We always return user set id. If not valid, then we send unique id.
                if (string.IsNullOrWhiteSpace(_id)) {
                    return UId;
                }
                return _id;
            }

            set { _id = value; }
        }

        public string Label {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public bool RemoveIcon {
            get { return (bool)GetValue(RemoveIconProperty); }
            set { SetValue(RemoveIconProperty, value); }
        }

        public string UId { get; private set; }
        public override string ToString()
        {
            return this.Id;
        }
    }
}
