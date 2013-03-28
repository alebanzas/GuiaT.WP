using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class HomeSUBE : PhoneApplicationPage
    {
        public HomeSUBE()
        {
            InitializeComponent();
        }

        private void Button_Click_PuntosDeVentaYCarga(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/PuntosDeVentaYCarga.xaml", UriKind.Relative));
        }

        private void Button_Click_PerdidaRoboDanio(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/PerdidaRoboDanio.xaml", UriKind.Relative));
        }

        private void Button_Click_DondeUsarSUBE(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/DondeUsarSUBE.xaml", UriKind.Relative));
        }

        private void Button_Click_PreguntasFrecuentes(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/PreguntasFrecuentes.xaml", UriKind.Relative));
        }

        private void Button_Click_AtencionAlUsuario(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/AtencionAlUsuario.xaml", UriKind.Relative));
        }
    }
}