using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SAMP.Client.WPF.Extensions
{
    public static class ViewExtensions
    {
        public static T GetVisualParent<T>(this Visual childObject) where T : Visual
        {
            DependencyObject child = childObject as DependencyObject;

            while ((child != null) && !(child is T))
            {
                child = VisualTreeHelper.GetParent(child);
            }

            return child as T;
        }

        public static T GetVisualChild<T>(this Visual referenceVisual) where T : Visual
        {
            Visual child = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(referenceVisual);

            for (int i = 0; i < childrenCount; i++)
            {
                child = VisualTreeHelper.GetChild(referenceVisual, i) as Visual;

                if (child != null && child is T)
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetVisualChild<T>(child);

                    if (child != null && child is T)
                    {
                        break;
                    }
                }
            }

            return child as T;
        }
    }
}
