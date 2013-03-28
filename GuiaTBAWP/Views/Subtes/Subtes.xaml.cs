using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Subtes
{
    public partial class Subtes : PhoneApplicationPage
    {
        public Subtes()
        {
            InitializeComponent();
        }
        
        private void Button_Click_SubteLineas(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Subtes/SubteLineas.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteHorarios(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Subtes/SubteHorarios.xaml", UriKind.Relative));
        }

        private void Button_Click_SubtePrecio(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Subtes/SubtePrecio.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteMapa(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Subtes/SubteMapa.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteEstado(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Subtes/SubteEstado.xaml", UriKind.Relative));
        }
    }
}