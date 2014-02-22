using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAMP.Client.WPF
{
    public sealed class MetroWindowManager : WindowManager
    {
        static readonly ResourceDictionary[] resources = new ResourceDictionary[] 
        {
            new ResourceDictionary
            { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml", UriKind.RelativeOrAbsolute) },
            new ResourceDictionary
            { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute) },
            new ResourceDictionary
            { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute) },
            new ResourceDictionary
            { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml", UriKind.RelativeOrAbsolute) },
            new ResourceDictionary
            { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml", UriKind.RelativeOrAbsolute) },
            new ResourceDictionary
            { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml", UriKind.RelativeOrAbsolute) }
        };

        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            MetroWindow window = view as MetroWindow;
            if (window == null)
            {
                window = new MetroWindow()
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                window.MinHeight = 150;
                window.MinWidth = 500;
                foreach (ResourceDictionary resourceDictionary in resources)
                {
                    window.Resources.MergedDictionaries.Add(resourceDictionary);
                }
                window.SetValue(View.IsGeneratedProperty, true);
                Window owner = this.InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                Window owner2 = this.InferOwnerOf(window);
                if (owner2 != null && isDialog)
                {
                    window.Owner = owner2;
                }
            }
            return window;
        }
    }
}
