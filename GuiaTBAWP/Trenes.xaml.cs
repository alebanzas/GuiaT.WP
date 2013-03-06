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
    public partial class Trenes : PhoneApplicationPage
    {
        public Trenes()
        {
            InitializeComponent();
        }

        private void Button_Click_BelgranoNorte(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/BelgranoNorte.xaml", UriKind.Relative));
        }

        private void Button_Click_BelgranoSur(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/BelgranoSur.xaml", UriKind.Relative));
        }

        private void Button_Click_Mitre(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Mitre.xaml", UriKind.Relative));
        }

        private void Button_Click_Roca(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Roca.xaml", UriKind.Relative));
        }

        private void Button_Click_SanMartin(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SanMartin.xaml", UriKind.Relative));
        }

        private void Button_Click_Sarmiento(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Sarmiento.xaml", UriKind.Relative));
        }

        private void Button_Click_Urquiza(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Urquiza.xaml", UriKind.Relative));
        }
    }
}