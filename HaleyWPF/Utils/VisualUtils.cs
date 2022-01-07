using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Media;

namespace Haley.Utils
{
    public static class VisualUtils
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                    //If first child is of target type, yield return
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //Loop through the children.
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static DependencyObject FindVisualChildren(DependencyObject obj,string target_name)
        {
            try
            {
                if (string.IsNullOrEmpty(target_name)) return null;
                if (obj != null)
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                        //If first child has the required name
                        if (child is FrameworkElement fele)
                        {
                            if (fele.Name == target_name) return child;
                        }

                        var childOfChild = FindVisualChildren(child, target_name);
                        if (childOfChild != null) return childOfChild;
                    }
                }
                return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T FindVisualParent<T>(DependencyObject input, int parentlevel) where T : DependencyObject
        {
            try
            {
                if (parentlevel == 0 || input == null) return null;

                var _parent = VisualTreeHelper.GetParent(input);
                //if parent is canvas, return it.
                if (_parent is T _target) return _target;

                //If not, loop again up.
                return FindVisualParent<T>(_parent, parentlevel - 1);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
