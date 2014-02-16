using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBA.WP7.Extensions;
using GuiaTBA.WP7.Models;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GuiaTBA.WP7.Views.SUBE
{
    public partial class DondeCargar
    {
        private static MainViewModel _viewModel;
        public static MainViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new MainViewModel(App.Configuration.BingMapApiKey)); }
        }

        private HttpWebRequest _httpReq;

        public DondeCargar()
        {
            InitializeComponent();
            
            Loaded += (s, e) =>
                {
                    DataContext = ViewModel;
                    
                    GetDondeCargar();
                };
            Unloaded += DondeCargar_Unloaded;
        }

        private void DondeCargar_Unloaded(object sender, RoutedEventArgs e)
        {
            if(_httpReq !=null)
                _httpReq.Abort();
        }
        
        void GetDondeCargar()
        {
            ResetUI();
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                ConnectionError.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }

            GeoPosition<GeoCoordinate> currentLocation = PositionService.GetCurrentLocation();

            if (!App.Configuration.IsLocationEnabled)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar donde cargar SUBE, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }
            if (currentLocation == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar donde cargar SUBE, por favor, active la función de localización."));
                return;
            }

            ProgressBar.Show("Buscando más cercanos...");
            ViewModel.CurrentPosition = currentLocation.Location;
            if (ViewModel.PuntosRecarga.Count == 0) Refreshing.Visibility = Visibility.Visible;
            SetApplicationBarEnabled(false);
            CancelarRequest();

            var param = new Dictionary<string, object>
                {
                    {"cant", 10},
                };

            var client = new HttpClient();
            _httpReq = client.Get("/api/recargasube".ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<SUBEPuntoModel>));
                var o = (List<SUBEPuntoModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
            }
            catch (Exception ex)
            {
                ex.Log(ResetUI, () => { ConnectionError.Visibility = Visibility.Visible; return 0; });
            }
        }

        delegate void DelegateUpdateWebBrowser(List<SUBEPuntoModel> local);
        private void UpdateWebBrowser(List<SUBEPuntoModel> l)
        {
            ViewModel.LoadPuntosRecarga(l.Select(x => new ItemViewModel
                {
                    Punto = new GeoCoordinate(x.Latitud, x.Longitud),
                    Titulo = x.Nombre,
                }));

            var pp = from ll in ViewModel.Pushpins
                    select ll.Location;
            Mapa.SetView(LocationRect.CreateLocationRect(pp));

            ResetUI();
            if (ViewModel.PuntosRecarga.Count == 0)
            {
                NoResults.Visibility = Visibility.Visible;
            }
        }

        private void CancelarRequest()
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }

        void SetApplicationBarEnabled(bool isEnabled)
        {
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;
        }

        private int ResetUI()
        {
            NoResults.Visibility = Visibility.Collapsed;
            Refreshing.Visibility = Visibility.Collapsed;
            ConnectionError.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
            return 0;
        }

        private void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pushpin = sender as Pushpin;

            // Center the map on a pushpin when touched.
            if (pushpin != null) ViewModel.Center = pushpin.Location;
        }
        
        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void MiMapa_Tap(object sender, GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/SUBE/Mapa.xaml", UriKind.Relative));
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            GetDondeCargar();
        }
    }
}