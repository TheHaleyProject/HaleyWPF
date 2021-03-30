using System;
using System.Windows.Input;

namespace Haley.Utils
{
    public static class AdditionalCommands
    {
        public readonly static RoutedUICommand ChangeCount = new RoutedUICommand("To change count of a value", nameof(ChangeCount), typeof(AdditionalCommands));
        public readonly static RoutedUICommand ChangeSelection = new RoutedUICommand("To change the selection", nameof(ChangeSelection), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Highlight = new RoutedUICommand("To highlight something", nameof(Highlight), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Show = new RoutedUICommand("To show something", nameof(Show), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Hide = new RoutedUICommand("To Hide something", nameof(Hide), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Initiate = new RoutedUICommand("To Initiate something", nameof(Initiate), typeof(AdditionalCommands));
    }
}
