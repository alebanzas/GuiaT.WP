using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class Bicicletas : PhoneApplicationPage
    {
        public Bicicletas()
        {
            InitializeComponent();

            StatusChecker.Check("Bicicletas");
        }

        private void Button_Click_ElSistema(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/ElSistema.xaml", UriKind.Relative));
        }

        private void Button_Click_Inscripcion(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/Inscripcion.xaml", UriKind.Relative));
        }

        private void Button_Click_ElServicio(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/ElServicio.xaml", UriKind.Relative));
        }

        private void Button_Click_Estaciones(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/Estaciones.xaml", UriKind.Relative));
        }

        private void Button_Click_Mapa(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/Mapa.xaml", UriKind.Relative));
        }
    }
}