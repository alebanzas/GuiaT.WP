using System;
using System.Windows;
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
            this.NavigationService.Navigate(new Uri("/Views/Subtes/Subtes.xaml", UriKind.Relative));
        }

        private void Button_Click_Trenes(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Trenes.xaml", UriKind.Relative));
        }
    }
}