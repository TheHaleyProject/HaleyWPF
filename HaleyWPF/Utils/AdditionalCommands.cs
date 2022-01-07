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
        public readonly static RoutedUICommand Filter = new RoutedUICommand("To filter something", nameof(Filter), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Toggle = new RoutedUICommand("To Toggle some object", nameof(Toggle), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Increase = new RoutedUICommand("To Increase something", nameof(Increase), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Decrease = new RoutedUICommand("To Decrease something", nameof(Decrease), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Reset = new RoutedUICommand("To Reset something", nameof(Reset), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Add = new RoutedUICommand("To add something", nameof(Add), typeof(AdditionalCommands));
        public readonly static RoutedUICommand ChangeColor = new RoutedUICommand("To change the Color", nameof(ChangeColor), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Minimize = new RoutedUICommand("To minimize something", nameof(Minimize), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Maximize = new RoutedUICommand("To maximize something", nameof(Maximize), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Close = new RoutedUICommand("To close something", nameof(Close), typeof(AdditionalCommands));
        public readonly static RoutedUICommand DragMove = new RoutedUICommand("To drag and move something", nameof(DragMove), typeof(AdditionalCommands));

        #region Generic
        public readonly static RoutedUICommand ExecuteAction = new RoutedUICommand("To execute some action", nameof(ExecuteAction), typeof(AdditionalCommands));
        public readonly static RoutedUICommand ExecuteAction2 = new RoutedUICommand("To execute some action-set2", nameof(ExecuteAction2), typeof(AdditionalCommands));

        public readonly static RoutedUICommand ProcessMenuAction = new RoutedUICommand("To process menu action", nameof(ProcessMenuAction), typeof(AdditionalCommands));
        public readonly static RoutedUICommand ProcessMenuAction2 = new RoutedUICommand("To process menu action 2", nameof(ProcessMenuAction2), typeof(AdditionalCommands));

        public readonly static RoutedUICommand WindowAction = new RoutedUICommand("To filter something", nameof(WindowAction), typeof(AdditionalCommands));
        public readonly static RoutedUICommand WindowAction2 = new RoutedUICommand("To filter something", nameof(WindowAction2), typeof(AdditionalCommands));
        #endregion
    }
}
