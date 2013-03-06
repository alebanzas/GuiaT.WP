using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP
{
    public partial class HomeSUBE : PhoneApplicationPage
    {
        public HomeSUBE()
        {
            InitializeComponent();
        }

        private void Button_Click_PuntosDeVentaYCarga(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/PuntosDeVentaYCarga.xaml", UriKind.Relative));
        }

        private void Button_Click_PerdidaRoboDanio(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/PerdidaRoboDanio.xaml", UriKind.Relative));
        }

        private void Button_Click_DondeUsarSUBE(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/DondeUsarSUBE.xaml", UriKind.Relative));
        }

        private void Button_Click_PreguntasFrecuentes(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/PreguntasFrecuentes.xaml", UriKind.Relative));
        }

        private void Button_Click_AtencionAlUsuario(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AtencionAlUsuario.xaml", UriKind.Relative));
        }
    }
}