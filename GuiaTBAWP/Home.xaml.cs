using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP
{
    public partial class Home : PhoneApplicationPage
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click_SUBE(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/HomeSUBE.xaml", UriKind.Relative));
        }

        private void Button_Click_Colectivos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Colectivos.xaml", UriKind.Relative));
        }

        private void Button_Click_Subtes(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Subtes.xaml", UriKind.Relative));
        }

        private void Button_Click_Trenes(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Trenes.xaml", UriKind.Relative));
        }
    }
}