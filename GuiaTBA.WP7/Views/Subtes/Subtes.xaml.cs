using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.Subtes
{
    public partial class Subtes : PhoneApplicationPage
    {
        public Subtes()
        {
            InitializeComponent();

            StatusChecker.Check("Subte");

            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;
        }
        
        private void Button_Click_SubteLineas(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteLineas.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteHorarios(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteHorarios.xaml", UriKind.Relative));
        }

        private void Button_Click_SubtePrecio(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubtePrecio.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteMapa(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteMapa.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteEstado(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteEstado.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteMapaReal(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/Mapa.xaml", UriKind.Relative));
        }
    }
}