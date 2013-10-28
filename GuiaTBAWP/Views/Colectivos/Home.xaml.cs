using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Home : PhoneApplicationPage
    {
        public Home()
        {
            InitializeComponent();
            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;

            AcBox.ItemsSource = DataColectivos.Repository.Select(x => x.Title);
            AcBox.FilterMode = AutoCompleteFilterMode.Contains;
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
                var bus = DataColectivos.Repository.First(x => x.Title.Equals(AcBox.Text)).Title.Split(' ')[1];

                Uri uri = new Uri(String.Format("/Views/Colectivos/Mapa.xaml?linea={0}", bus), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
            catch (Exception)
            {
                NoResults.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Colectivos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Colectivos/Colectivos.xaml", UriKind.Relative));
        }

        private void Button_Click_ColectivosCercanos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Colectivos/Cercanos.xaml", UriKind.Relative));
        }

        private void AcBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AcBox.Text = string.Empty;
        }
    }
}