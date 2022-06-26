using System;
using System.Windows.Input;

namespace Haley.Utils
{
    public static class PlainWindowCommands
    {
        public readonly static RoutedUICommand WindowAction = new RoutedUICommand("To filter something", nameof(WindowAction), typeof(AdditionalCommands));
        public readonly static RoutedUICommand WindowDragMove = new RoutedUICommand("To drag and move", nameof(WindowDragMove), typeof(AdditionalCommands));
        public readonly static RoutedUICommand ContentPresenterClicked = new RoutedUICommand("To preview left mouse down", nameof(ContentPresenterClicked), typeof(PlainWindowCommands));
    }
}
