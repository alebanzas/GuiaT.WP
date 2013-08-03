using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Aviones
{
    public partial class Home : PhoneApplicationPage
    {
        public Home()
        {
            InitializeComponent();
        }

        //AEROPARQUE

        private void Button_Click_AeroparqueComoLlegar(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Aeroparque/ComoLlegar.xaml", UriKind.Relative));
        }

        private void Button_Click_AeroparqueArribos(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Estado.xaml?tipo=arribos&aeroestacion=aeroparque&titulo=arribos&nickname=AEP", UriKind.Relative));
        }

        private void Button_Click_AeroparquePartidas(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Estado.xaml?tipo=partidas&aeroestacion=aeroparque&titulo=partidas&nickname=AEP", UriKind.Relative));
        }

        private void Button_Click_AeroparquePlano(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Aeroparque/Plano.xaml", UriKind.Relative));
        }

        private void Button_Click_AeroparqueOperadoras(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Aeroparque/Operadoras.xaml", UriKind.Relative));
        }

        private void Button_Click_AeroparqueInfoTecnica(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Aeroparque/InfoTecnica.xaml", UriKind.Relative));
        }


        //EZEIZA

        private void Button_Click_EzeizaComoLlegar(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Ezeiza/ComoLlegar.xaml", UriKind.Relative));
        }

        private void Button_Click_EzeizaArribos(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Estado.xaml?tipo=arribos&aeroestacion=ezeiza&titulo=arribos&nickname=EZE", UriKind.Relative));
        }

        private void Button_Click_EzeizaPartidas(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Estado.xaml?tipo=partidas&aeroestacion=ezeiza&titulo=partidas&nickname=EZE", UriKind.Relative));
        }

        private void Button_Click_EzeizaPlano(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Ezeiza/Plano.xaml", UriKind.Relative));
        }

        private void Button_Click_EzeizaOperadoras(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Ezeiza/Operadoras.xaml", UriKind.Relative));
        }

        private void Button_Click_EzeizaInfoTecnica(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Ezeiza/InfoTecnica.xaml", UriKind.Relative));
        }
    }
}