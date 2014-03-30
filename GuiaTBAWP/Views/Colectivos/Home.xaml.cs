using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Home
    {
        private readonly GetColectivoMapService _getColectivoMapService;

        public Home()
        {
            InitializeComponent();

            StatusChecker.Check("Colectivos");

            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;

            AcBox.ItemsSource = DataColectivos.Repository.Select(x => x.Title);
            AcBox.FilterMode = AutoCompleteFilterMode.Contains;
            _getColectivoMapService = new GetColectivoMapService();
        }
        
        private void ButtonBuscar_OnClick(object sender, RoutedEventArgs e)
        {
            ProcesarBusqueda();
        }

        private void AcBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcesarBusqueda();
            }
        }

        private void ProcesarBusqueda()
        {
            try
            {
                NoResults.Visibility = Visibility.Collapsed;
                BtnBuscar.IsEnabled = false;
                App.MapViewModel.Reset();
                var bus = DataColectivos.Repository.First(x => x.Title.Equals(AcBox.Text)).Title.Split(' ')[1];
                _getColectivoMapService.SuccessFunc = () =>
                {
                    BtnBuscar.IsEnabled = true;
                    NavigationService.Navigate(new Uri("/Views/Mapa.xaml", UriKind.Relative));
                    return 0;
                };
                _getColectivoMapService.ErrorFunc = () =>
                {
                    NoResults.Visibility = Visibility.Visible;
                    BtnBuscar.IsEnabled = true;
                    return 0;
                };
                _getColectivoMapService.GetColectivo(bus);
            }
            catch (Exception)
            {
                NoResults.Visibility = Visibility.Visible;
                BtnBuscar.IsEnabled = true;
            }
        }

        private void Button_Click_Colectivos(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Colectivos/Colectivos.xaml", UriKind.Relative));
        }

        private void Button_Click_ColectivosCercanos(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Colectivos/Cercanos.xaml", UriKind.Relative));
        }

        private void Button_Click_ColectivosRuta(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Ruta/Home.xaml", UriKind.Relative));
        }

        private void AcBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AcBox.Text = string.Empty;
        }
        
        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }
    }
}