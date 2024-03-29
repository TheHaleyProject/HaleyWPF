﻿using Haley.Events;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Haley.WPF.Controls
{
    public class SearchBar : PlainTextBox, ICommandSource
    {
        #region Events
        public static readonly RoutedEvent SearchStartedEvent = EventManager.RegisterRoutedEvent(nameof(SearchStarted), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBar));

        public event RoutedEventHandler SearchStarted
        {
            add { AddHandler(SearchStartedEvent, value); }
            remove { RemoveHandler(SearchStartedEvent, value); }
        }
        #endregion

        static SearchBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBar), new FrameworkPropertyMetadata(typeof(SearchBar)));
        }

        public SearchBar()
        { CommandBindings.Add(new CommandBinding(NavigationCommands.Search, Execute_Search)); }

        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(SolidColorBrush), typeof(SearchBar), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(SearchBar), new PropertyMetadata(default(ICommand), OnCommandPropertyChanged));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(SearchBar), new PropertyMetadata(default(object)));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(SearchBar), new PropertyMetadata(default(IInputElement)));

        void Execute_Search(object sender, ExecutedRoutedEventArgs e)
        {
            RaiseEvent(new UIRoutedEventArgs<string>(SearchStartedEvent, this) { Value = Text });
            switch (Command)
            {
                case null:
                    return;
                case RoutedCommand command: //same as  if (Command is RoutedCommand command)
                    command.Execute(CommandParameter, CommandTarget);
                    break;
                default:
                    Command.Execute(CommandParameter);
                    break;
            }
        }

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var searchbar = (SearchBar)d;
            if (e.OldValue is ICommand oldCommand)
            {
                oldCommand.CanExecuteChanged -= searchbar.CanExecuteChanged;
            }
            if (e.NewValue is ICommand newCommand)
            {
                newCommand.CanExecuteChanged += searchbar.CanExecuteChanged;
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command == null) return;

            IsEnabled = Command is RoutedCommand command
                ? command.CanExecute(CommandParameter, CommandTarget)
                : Command.CanExecute(CommandParameter);
        }

    }
}
