using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class Error : PhoneApplicationPage
    {
        public Error()
        {
            InitializeComponent();
        }

        private void Button_Click_PuntosDeVentaYCarga(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/PuntosDeVentaYCarga.xaml", UriKind.Relative));
        }

        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SUBE/HomeSUBE.xaml", UriKind.Relative));
        }
    }
}