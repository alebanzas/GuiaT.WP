using System;
using System.Windows;
using Microsoft.Phone.Tasks;

namespace BicicletaBAWP
{
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;

            StatusChecker.Check("Bicicletas");

            TxtVersion.Text = string.Format("Versión {0}", App.Configuration.Version);
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

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void RateReview_Click(object sender, EventArgs e)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void FullApp_OnClick(object sender, EventArgs e)
        {
            //Show an application, using the default ContentType.
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "98250785-9804-4439-af3f-63ef88c5998c";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }
    }
}