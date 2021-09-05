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

namespace Haley.WPF.GroupControls
{
     /// <summary>
     /// Fleximenu for both left/right and topbottom docking.s
     /// </summary>
    public class MenuItem : DependencyObject, IMenuItem
    {
        public string Id { get; private set; }

        public MenuItem()
        { Id = Guid.NewGuid().ToString(); }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MenuItem), new FrameworkPropertyMetadata(default(ICommand)));

        public string CommandName
        {
            get { return (string)GetValue(CommandNameProperty); }
            set { SetValue(CommandNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(nameof(CommandName), typeof(string), typeof(MenuItem), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(MenuItem), new FrameworkPropertyMetadata(default(object)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(defaultValue: "Button"));

        public MenuAction Action
        {
            get { return (MenuAction)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Action.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register(nameof(Action), typeof(MenuAction), typeof(MenuItem), new PropertyMetadata(MenuAction.RaiseCommand));

        public UserControl View
        {
            get { return (UserControl)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register(nameof(View), typeof(UserControl), typeof(MenuItem), new PropertyMetadata(null));

        public string ContainerKey
        {
            get { return (string)GetValue(ContainerKeyProperty); }
            set { SetValue(ContainerKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContainerKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContainerKeyProperty =
            DependencyProperty.Register(nameof(ContainerKey), typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(null));

        public bool IgnoreLocalContainer
        {
            get { return (bool)GetValue(IgnoreLocalContainerProperty); }
            set { SetValue(IgnoreLocalContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreLocalContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreLocalContainerProperty =
            DependencyProperty.Register(nameof(IgnoreLocalContainer), typeof(bool), typeof(MenuItem), new PropertyMetadata(false));

        public bool IgnoreGlobalContainer
        {
            get { return (bool)GetValue(IgnoreGlobalContainerProperty); }
            set { SetValue(IgnoreGlobalContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreGlobalContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreGlobalContainerProperty =
            DependencyProperty.Register(nameof(IgnoreGlobalContainer), typeof(bool), typeof(MenuItem), new PropertyMetadata(false));
    }
}
