using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP
{
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();

            TxtVersion.Text = string.Format("Versión {0}", App.Configuration.Version);
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
    }

    public static class TileManager
    {
        public static void Set(Uri navigationUrl, string title, Uri backgroundImage)
        {
            var standardTileData = new StandardTileData
                {
                    BackgroundImage = backgroundImage, 
                    Title = title,
                };

            ShellTile tiletopin = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(navigationUrl.ToString()));
            
            if (tiletopin == null)
            {
                ShellTile.Create(navigationUrl, standardTileData);
            }
            else
            {
                MessageBox.Show("Ya está anclado.");
            }
        }
    }
}