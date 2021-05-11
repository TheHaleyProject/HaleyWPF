using System;

namespace Haley.Enums
{
    public enum MenuAction
    {
        /// <summary>
        /// This will ignore views and will merely raise the command which is associated with it.
        /// </summary>
        RaiseCommand,
        /// <summary>
        /// This will use the local view attached with the MenuItem and will display it. For this to work, view should directly be specified in the menuitem.
        /// </summary>
        ShowLocalView,
        /// <summary>
        /// This will check the HaleyIOC for any views associated and will retrieve and show them. For this to work, a specific key should be associated with the Menu Item.
        /// </summary>
        ShowContainerView
    }
}
