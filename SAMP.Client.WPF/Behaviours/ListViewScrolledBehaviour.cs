using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SAMP.Client.WPF.Extensions;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SAMP.Client.WPF.Behaviours
{
    public static class ListViewScrolledBehaviour
    {
        public static ICommand GetScrolledCommand(DependencyObject target)
        {
            var scrolledCommandName = (string)target.GetValue(ScrolledCommandProperty);
            var listView = target as ListView;

            if (listView != null)
            {
                var dataContext = listView.DataContext;

                if (dataContext == null)
                    return null;

                if (scrolledCommandName == null)
                    return null;

                var contextType = dataContext.GetType();
                var commandProperty = contextType.GetProperty(scrolledCommandName);

                return
                    (ICommand)commandProperty.GetValue(dataContext);
            }

            return null;
        }

        public static void SetScrolledCommand(DependencyObject target, string value)
        {
            target.SetValue(ScrolledCommandProperty, value);
        }

        public static readonly DependencyProperty ScrolledCommandProperty
            = DependencyProperty.RegisterAttached("ScrolledCommand", typeof(string), typeof(ListViewScrolledBehaviour), new UIPropertyMetadata(string.Empty, OnScrolledCommandChanged));

        private static void OnScrolledCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView == null)
                return;

            listView.Loaded += (_sender, _e) =>
            {
                var scrollViewer = listView.GetVisualChild<ScrollViewer>();

                if (scrollViewer == null)
                    return;

                var scrollBar = scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer) as ScrollBar;

                if (scrollBar == null)
                    return;

                var oldValueString = e.OldValue as string;
                var newValueString = e.NewValue as string;

                if (String.IsNullOrEmpty(oldValueString))
                {
                    scrollBar.ValueChanged += scrollBar_ValueChanged;
                }
                else if (String.IsNullOrEmpty(newValueString))
                {
                    scrollBar.ValueChanged -= scrollBar_ValueChanged;
                }
            };
        }

        static void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var scrollBar = sender as ScrollBar;

            if (scrollBar == null)
                return;

            var scrollViewer = scrollBar.GetVisualParent<ScrollViewer>();

            if (scrollViewer == null)
                return;

            var listView = scrollViewer.GetVisualParent<ListView>();

            if (listView == null)
                return;

            var command = GetScrolledCommand(listView);
           
            if (command != null && command.CanExecute(scrollViewer.VerticalOffset))
                command.Execute(new { scrollViewer.VerticalOffset, scrollViewer.ViewportHeight });
        }
    }
}
