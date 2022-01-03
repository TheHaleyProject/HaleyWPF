using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;

namespace Haley.WPF.Internal
{
    internal class FlexiBorder : Border,ICornerRadius
    {
        #region Constructors
        static FlexiBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexiBorder), new FrameworkPropertyMetadata(typeof(FlexiBorder)));
        }

        public FlexiBorder()
        {

        }
        #endregion

        #region Initiation
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

    }  
}
