using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Trenes : PhoneApplicationPage
    {
        public Trenes()
        {
            InitializeComponent();

            StatusChecker.Check("TrenesHome");
        }

        private void Button_Click_BelgranoNorte(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/BelgranoNorte.xaml", UriKind.Relative));
        }

        private void Button_Click_BelgranoSur(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/BelgranoSur.xaml", UriKind.Relative));
        }

        private void Button_Click_Mitre(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Mitre.xaml", UriKind.Relative));
        }

        private void Button_Click_Roca(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Roca.xaml", UriKind.Relative));
        }

        private void Button_Click_SanMartin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/SanMartin.xaml", UriKind.Relative));
        }

        private void Button_Click_Sarmiento(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Sarmiento.xaml", UriKind.Relative));
        }

        private void Button_Click_Urquiza(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Urquiza.xaml", UriKind.Relative));
        }

        private void Button_Click_Mapa(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Mapa.xaml", UriKind.Relative));
        }
    }
}