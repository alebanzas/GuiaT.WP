using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP
{
    public partial class Error : PhoneApplicationPage
    {
        public Error()
        {
            InitializeComponent();
        }

        private void Button_Click_PuntosDeVentaYCarga(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/PuntosDeVentaYCarga.xaml", UriKind.Relative));
        }

        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }
    }
}