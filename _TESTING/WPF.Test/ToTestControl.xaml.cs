﻿using System.Windows.Controls;
using System.Windows;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for ToTestControl.xaml
    /// </summary>
    public partial class ToTestControl : UserControl
    {
        public ToTestControl()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
            
        }
    }
}
