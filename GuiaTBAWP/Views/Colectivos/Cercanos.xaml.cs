using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Bing.Geocode;
using GuiaTBAWP.BusData;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Cercanos : PhoneApplicationPage
    {
        private static ColectivoCercanoViewModel _viewModel;
        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static ColectivoCercanoViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new ColectivoCercanoViewModel());
            }
        }

        public GeoCoordinateWatcher Ubicacion { get; set; }

        readonly ProgressIndicator _progress = new ProgressIndicator();
        private HttpWebRequest _httpReq;

        public Cercanos()
        {
            InitializeComponent();

            ResetUI();

            InitializeGPS();
            SetLocationService();

            Unloaded += DondeCargar_Unloaded;

            if (!NetworkInterface.GetIsNetworkAvailable() ||
                (Ubicacion.Permission.Equals(GeoPositionPermission.Denied) ||
                 Ubicacion.Permission.Equals(GeoPositionPermission.Unknown)))
            {
                Loaded += (s, e) =>
                {
                    var ns = NavigationService;
                    ns.Navigate(new Uri("/Views/SUBE/Error.xaml", UriKind.Relative));
                };
                return;
            }
            Loaded += (s, e) =>
                {
                    DataContext = ViewModel;

                    _progress.IsVisible = true;
                    _progress.IsIndeterminate = true;
                    
                    StartLocationService();
                };
            Unloaded += (s, e) =>
            {
                Ubicacion.Dispose();
            };
        }

        private void DondeCargar_Unloaded(object sender, RoutedEventArgs e)
        {
            if(_httpReq !=null)
                _httpReq.Abort();
        }

        private void SetProgressBar(string msj, bool showProgress = true)
        {
            if (string.IsNullOrEmpty(msj))
            {
                SystemTray.SetProgressIndicator(this, null);
            }
            else
            {
                _progress.Text = msj;
                _progress.IsIndeterminate = showProgress;
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetProgressIndicator(this, _progress);
            }
        }


        //TODO: Extract to class
        #region Location Service

        private void ResetLocationService()
        {
            InitializeGPS();
            SetLocationService();
            StartLocationService();
        }
        
        public void InitializeGPS()
        {
            Ubicacion = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            Ubicacion.StatusChanged += Ubicacion_StatusChanged;

            Ubicacion.MovementThreshold = 100;
        }

        void Ubicacion_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (Ubicacion.Permission == GeoPositionPermission.Denied)
                    {
                        MessageBox.Show("El servicio de localización se encuentra deshabilitado. Por favor asegúrese de habilitarlo en las Opciones del dispositivo para ubicarlo en el mapa.");
                        //this.ApplicationTitle.Text = "Estado: Sin permisos de localización";
                    }
                    else
                    {
                        MessageBox.Show("El servicio de localización se encuentra sin funcionamiento.");
                        //this.ApplicationTitle.Text = "Estado: Servicio de localización sin funcionamiento";
                    }
                    StopLocationService();
                    break;

                case GeoPositionStatus.Initializing:
                    //this.ApplicationTitle.Text = "Estado: Inicializando";
                    break;

                case GeoPositionStatus.NoData:
                    //this.ApplicationTitle.Text = "Estado: Datos no disponibles";
                    break;

                case GeoPositionStatus.Ready:
                    //this.ApplicationTitle.Text = "Estado: Servicio de localización disponible";
                    break;
            }
        }

        /// <summary>
        /// Helper method to start up the location data acquisition
        /// </summary>
        private void SetLocationService()
        {
            Ubicacion.PositionChanged += watcher_PositionChanged;
        }

        /// <summary>
        /// Helper method to start up the location data acquisition
        /// </summary>
        private void StartLocationService()
        {
            SetProgressBar("Buscando posición...");
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = false;
            if ((bool) IsolatedStorageSettings.ApplicationSettings["localizacion"])
            {
                Ubicacion.Start();
            }
            else
            {
                SetProgressBar(null);
                MessageBox.Show("El servicio de localización se encuentra deshabilitado. Por favor asegúrese de habilitarlo en las Opciones del dispositivo para ubicarlo en el mapa.");
                StopLocationService();
            }
                
        }

        /// <summary>
        /// Helper method to stop up the location data acquisition
        /// </summary>
        private void StopLocationService()
        {
            // Stop data acquisition
            Ubicacion.Stop();
        }

        /// <summary>
        /// Handler for the PositionChanged event. This invokes MyStatusChanged on the UI thread and
        /// passes the GeoPositionStatusChangedEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyPositionChanged(e));
        }

        /// <summary>
        /// Custom method called from the PositionChanged event handler
        /// </summary>
        /// <param name="e"></param>
        void MyPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var location = new GeocodeLocation
            {
                Latitude = e.Position.Location.Latitude,
                Longitude = e.Position.Location.Longitude
            };

            if (Math.Abs(location.Latitude - ViewModel.CurrentLocation.Latitude) < App.MinDiffGeography && Math.Abs(location.Longitude - ViewModel.CurrentLocation.Longitude) < App.MinDiffGeography)
            {
                StopLocationService();
                ResetUI();
                return;
            }

            ViewModel.CurrentLocation = e.Position.Location;

            //SetLocation(location);
            //StopLocationService();
            SetProgressBar(null);

            GetMasCercanos(location);
        }

        private int _pendingRequests;
        private void GetMasCercanos(GeocodeLocation location)
        {
            SetProgressBar("Buscando más cercano...");

            _httpReq = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("http://servicio.abhosting.com.ar/transporte/cercano/?lat={0}&lon={1}&version=" + App.Version, location.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."), location.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."))));
            _httpReq.Method = "POST";
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
            _pendingRequests++;
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<TransporteModel>));
                var o = (List<TransporteModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        Loading.Visibility = Visibility.Collapsed;
                        ConnectionError.Visibility = Visibility.Visible;
                        FinishRequest();
                    });
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }

        delegate void DelegateUpdateWebBrowser(List<TransporteModel> local);
        private void UpdateWebBrowser(List<TransporteModel> l)
        {
            ViewModel.Items.Clear();
            foreach (IGrouping<string, TransporteModel> transporteModel in l.Where(x => x.TipoNickName == "colectivo").GroupBy(x => x.Linea))
            {
                ViewModel.AddLinea(new ColectivoItemViewModel { 
                    Nombre = "Línea " + transporteModel.Key, 
                    Detalles = SetDetalleByLinea(transporteModel.Key, l),
                    //Detalles = DataColectivos.ByCode(transporteModel.Key), 
                });
            }
            
            Loading.Visibility = Visibility.Collapsed;
            ConnectionError.Visibility = Visibility.Collapsed;
            FinishRequest();
        }

        private string SetDetalleByLinea(string key, IEnumerable<TransporteModel> transporteModels)
        {
            return string.Join("\n", transporteModels.Where(x => x.Linea.Equals(key)).OrderBy(x => x.Ramal).Select(x => x.Ramal));
        }

        private void FinishRequest()
        {
            _pendingRequests--;
            if (_pendingRequests != 0) return;

            StopLocationService();
            ResetUI();
        }

        private void ResetUI()
        {
            if (_pendingRequests != 0) return;
            SetProgressBar(null);
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = true;
        }

        #endregion

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            ResetLocationService();
        }
    }
}