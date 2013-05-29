using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Home : PhoneApplicationPage
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click_Colectivos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Colectivos/Colectivos.xaml", UriKind.Relative));
        }

        private void Button_Click_ColectivosCercanos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Colectivos/Cercanos.xaml", UriKind.Relative));
        }
    }
}