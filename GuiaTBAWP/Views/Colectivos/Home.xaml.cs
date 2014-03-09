using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GuiaTBA.Common;
using GuiaTBAWP.BusData;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Home
    {
        private string _lineaBusqueda;
        WebRequest _httpReq;

        public Home()
        {
            InitializeComponent();

            StatusChecker.Check("Colectivos");

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
                _lineaBusqueda = bus;
                App.MapViewModel.Reset();
                GetColectivo();
            }
            catch (Exception)
            {
                NoResults.Visibility = Visibility.Visible;
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

        private void AcBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AcBox.Text = string.Empty;
        }
        void GetColectivo()
        {
            //Esta en file system
            if (Config.Get<List<TransporteViewModel>>("linea-" + _lineaBusqueda) != null)
            {
                UpdateList(Config.Get<List<TransporteViewModel>>("linea-" + _lineaBusqueda));
                return;
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }

            GeoPosition<GeoCoordinate> currentLocation = PositionService.GetCurrentLocation();

            if (!App.Configuration.IsLocationEnabled)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar recorrido de colectivos, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }
            if (currentLocation == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar recorrido de colectivos, por favor, active la función de localización."));
                return;
            }

            ProgressBar.Show(string.Format("Obteniendo recorrido línea {0}...", _lineaBusqueda));
            
            var param = new Dictionary<string, object>
                {
                    {"linea", _lineaBusqueda},
                    {"puntos", true},
                };

            var client = new HttpClient();
            _httpReq = client.Get("/api/transporte".ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<TransporteViewModel>));
                var o = (List<TransporteViewModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateList(UpdateList), o);
            }
            catch (Exception ex)
            {
                //TODO
                //ex.Log(ResetUI, () => { NoConnection.Visibility = Visibility.Visible; return 0; });
            }
        }

        delegate void DelegateUpdateList(List<TransporteViewModel> local);
        private void UpdateList(List<TransporteViewModel> ls)
        {
            if (Config.Get<List<TransporteViewModel>>("linea-" + _lineaBusqueda) == null && ls.Any())
                Config.Set("linea-" + _lineaBusqueda, ls);

            ls = ls.OrderBy(y => y.Nombre).ToList();

            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            //MiMapa.Layers.Clear();

            try
            {
                for (int index = 0; index < ls.Count; index++)
                {
                    TransporteViewModel transporteViewModel = ls[index];
                    var routeLine = new MapPolyline
                    {
                        Path = new GeoCoordinateCollection(),
                        StrokeColor = GetRandomColor(index),
                        StrokeThickness = 5.0,
                    };

                    foreach (var location in transporteViewModel.Puntos)
                    {
                        routeLine.Path.Add(new GeoCoordinate(location.Y, location.X));
                    }
                    App.MapViewModel.AddElement(new MapReference { Nombre = transporteViewModel.Nombre }, routeLine);
                }

                NavigationService.Navigate(new Uri("/Views/Mapa.xaml", UriKind.Relative));
            }
            catch
            {
                NoResults.Visibility = Visibility.Visible;
                Config.Remove("linea-" + _lineaBusqueda);
                return;
            }
        }

        private readonly Random _random = new Random();
        private Color GetRandomColor(int index)
        {
            var colors = new[] { 
                Colors.Red, 
                Colors.Blue, 
                Colors.Yellow,
                Colors.Orange, 
                Colors.Magenta, 
                Colors.Cyan, 
                ColorTranslator.FromHtml("#FF3C00"),
                ColorTranslator.FromHtml("#33FF00"),
                ColorTranslator.FromHtml("#FF0055"),
            };

            if (index < colors.Length)
            {
                return colors[index];
            }

            var red = _random.Next(1, 175);
            var green = _random.Next(1, 175);
            var blue = _random.Next(1, 175);

            return Color.FromArgb(255, (byte)red, (byte)green, (byte)blue);
        }
        
    }
}