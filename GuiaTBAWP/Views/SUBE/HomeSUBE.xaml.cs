using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class HomeSUBE
    {
        public HomeSUBE()
        {
            InitializeComponent();

            StatusChecker.Check("SUBE");
        }

        private void Button_Click_Carga(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/PuntosSUBE.xaml?titulo=¿dónde cargar?&tipo=recarga", UriKind.Relative));
        }

        private void Button_Click_Venta(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/PuntosSUBE.xaml?titulo=¿dónde comprar?&tipo=venta", UriKind.Relative));
        }

        private void Button_Click_PerdidaRoboDanio(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/PerdidaRoboDanio.xaml", UriKind.Relative));
        }

        private void Button_Click_DondeUsarSUBE(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/DondeUsarSUBE.xaml", UriKind.Relative));
        }

        private void Button_Click_PreguntasFrecuentes(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/PreguntasFrecuentes.xaml", UriKind.Relative));
        }

        private void Button_Click_AtencionAlUsuario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/AtencionAlUsuario.xaml", UriKind.Relative));
        }
    }
}