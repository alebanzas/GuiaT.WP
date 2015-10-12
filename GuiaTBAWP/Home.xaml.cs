using System;
using System.Windows;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Helpers;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP
{
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();

            TxtVersion.Text = $"Versión {App.Configuration.Version}";

            StatusChecker.Check("Home");

            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;

            if (!App.Configuration.IsRated && App.Configuration.OpenCount > 1)
            {
                if (MessageBox.Show("Queres calificar la aplicación?", "Ayudanos a mejorar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    App.Configuration.IsRated = true;
                    Config.Set(App.Configuration);

                    ShowReviewTask();
                }
            }
        }

        private void Button_Click_Ruta(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Ruta/Home.xaml", UriKind.Relative));
        }

        private void Button_Click_SUBE(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/HomeSUBE.xaml", UriKind.Relative));
        }

        private void Button_Click_Colectivos(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Colectivos/Home.xaml", UriKind.Relative));
        }

        private void Button_Click_Subtes(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/Subtes.xaml", UriKind.Relative));
        }

        private void Button_Click_Trenes(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Trenes.xaml", UriKind.Relative));
        }

        private void Button_Click_Bicicletas(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/Bicicletas.xaml", UriKind.Relative));
        }

        private void Button_Click_Taxis(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Taxis/Taxis.xaml", UriKind.Relative));
        }

        private void Button_Click_Autos(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Autos/Home.xaml", UriKind.Relative));
        }

        private void Button_Click_Aviones(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Aviones/Home.xaml", UriKind.Relative));
        }

        private void MenuRuta_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Ruta/Home.xaml", UriKind.Relative), "", new Uri("/Images/Home/ruta.png", UriKind.Relative));
        }

        private void MenuSubte_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Subtes/Subtes.xaml", UriKind.Relative), "", new Uri("/Images/Home/subtes.png", UriKind.Relative));
        }

        private void MenuColectivos_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Colectivos/Home.xaml", UriKind.Relative), "", new Uri("/Images/Home/colectivos.png", UriKind.Relative));
        }

        private void MenuBicicletas_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Bicicletas/Bicicletas.xaml", UriKind.Relative), "", new Uri("/Images/Home/bicicletas.png", UriKind.Relative));
        }

        private void MenuTrenes_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Trenes/Trenes.xaml", UriKind.Relative), "", new Uri("/Images/Home/trenes.png", UriKind.Relative));
        }

        private void MenuSube_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/SUBE/HomeSUBE.xaml", UriKind.Relative), "", new Uri("/Images/Home/sube.png", UriKind.Relative));
        }

        private void MenuTaxi_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Taxis/Taxis.xaml", UriKind.Relative), "", new Uri("/Images/Home/taxis.png", UriKind.Relative));
        }

        private void MenuAutos_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Autos/Home.xaml", UriKind.Relative), "", new Uri("/Images/Home/autos.png", UriKind.Relative));
        }

        private void MenuAviones_OnClick(object sender, RoutedEventArgs e)
        {
            TileManager.Set(new Uri("/Views/Aviones/Home.xaml", UriKind.Relative), "", new Uri("/Images/Home/aviones.png", UriKind.Relative));
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
            ShowReviewTask();
        }

        private static void ShowReviewTask()
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void ButtonComentarios_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Comments.xaml", UriKind.Relative));
        }
    }
}