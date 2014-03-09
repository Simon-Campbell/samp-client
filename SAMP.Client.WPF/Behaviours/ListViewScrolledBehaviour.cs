using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SAMP.Client.WPF.Behaviours
{
    public static class ListViewScrolledBehaviour
    {
        public static ICommand GetScrolledCommand(DependencyObject target)
        {
            var scrolledCommandName = (string) target.GetValue(ScrolledCommandProperty);
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
                    (ICommand) commandProperty.GetValue(dataContext);
            }

            return null;
        }

        public static void SetScrolledCommand(DependencyObject target, string value)
        {
            target.SetValue(ScrolledCommandProperty, value);
        }

        public static readonly DependencyProperty ScrolledCommandProperty 
            = DependencyProperty.RegisterAttached("ScrolledCommand", typeof(string), typeof(ListViewScrolledBehaviour), new UIPropertyMetadata(string.Empty, OnScrolledCommandChanged));

        private static readonly ScrollChangedEventHandler ScrollChangedEventHandler
            = new ScrollChangedEventHandler(OnScrolled);

        private static void OnScrolledCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView == null)
                return;

            var oldValueString = e.OldValue as string;
            var newValueString = e.NewValue as string;

            if (String.IsNullOrEmpty(oldValueString))
            {
                listView.AddHandler(ScrollViewer.ScrollChangedEvent, ScrollChangedEventHandler);
            } 
            else if (String.IsNullOrEmpty(newValueString))
            {
                listView.RemoveHandler(ScrollViewer.ScrollChangedEvent, ScrollChangedEventHandler);
            }
        }

        private static void OnScrolled(object sender, ScrollChangedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null)
            {
                var command = GetScrolledCommand(listView);

                if (command != null && command.CanExecute(e.VerticalOffset))
                    command.Execute(e.VerticalOffset);
            }
        }
    }
}
