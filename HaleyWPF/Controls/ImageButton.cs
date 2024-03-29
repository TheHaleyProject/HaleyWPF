﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Haley.Models;
using System.Windows.Controls.Primitives;
using Haley.Abstractions;

namespace Haley.WPF.Controls
{
    public class ImageButton : ButtonBase, ICornerRadius
    {
        #region Constructors
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Icon.InitiateImages(this);
        }

        #region Corner Radius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ImageButton), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));
        #endregion

    }
}
