using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP
{
    public partial class Subtes : PhoneApplicationPage
    {
        public Subtes()
        {
            InitializeComponent();
        }
        
        private void Button_Click_SubteLineas(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SubteLineas.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteMapa(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SubteMapa.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteEstado(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SubteEstado.xaml", UriKind.Relative));
        }
    }
}