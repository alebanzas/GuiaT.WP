using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Bing.Geocode;
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
            Ubicacion = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            Ubicacion.StatusChanged += Ubicacion_StatusChanged;

            Ubicacion.MovementThreshold = 20;
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
                    Ubicacion.Stop();
                    Ubicacion.Dispose();
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
                Ubicacion.Stop();
            }
                
        }

        /// <summary>
        /// Helper method to stop up the location data acquisition
        /// </summary>
        private void StopLocationService()
        {
            // Stop data acquisition
            Ubicacion.Stop();
            Ubicacion.Dispose();
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

            //if (location.Latitude == ViewModel.CurrentLocation.Latitude && location.Longitude == ViewModel.CurrentLocation.Longitude)
            //{
            //    return;
            //}

            //ViewModel.CurrentLocation = e.Position.Location;

            //SetLocation(location);
            //StopLocationService();
            SetProgressBar(null);

            GetMasCercanos(location);
        }

        private int _pendingRequests;
        private void GetMasCercanos(GeocodeLocation location)
        {
            SetProgressBar("Buscando más cercano...");

            _httpReq = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("http://servicio.abhosting.com.ar/sube/recarganear/?lat={0}&lon={1}&cant=10", location.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."), location.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."))));
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

                var serializer = new DataContractJsonSerializer(typeof(List<ColectivoModel>));
                var o = (List<ColectivoModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        //RecargaLoading.Visibility = Visibility.Collapsed;
                        //RecargaError.Visibility = Visibility.Visible;
                        FinishRequest();
                    });
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }

        delegate void DelegateUpdateWebBrowser(List<ColectivoModel> local);
        private void UpdateWebBrowser(List<ColectivoModel> l)
        {
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "12", Detalles = "A - Puente Pueyrredon", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "12", Detalles = "B - Constitución", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "64", Detalles = "A - Barrancas de Belgrano por Hospital Militar   ", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "64", Detalles = "B - Barrancas de Belgrano por Hipódromo", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "64", Detalles = "C - Barrancas de Belgrano por Hospital Militar y Univ.Cat.Arg.", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "64", Detalles = "D - Barrancas de Belgrano por Hipódromo y Univ.Cat.Arg.", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "64", Detalles = "E - Barrancas de Belgrano por Hospital Militar", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "152", Detalles = "A - Olivos", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "152", Detalles = "B - Olivos por UCA", });
            ViewModel.AddLinea(new ColectivoItemViewModel { Nombre = "152", Detalles = "C - Est. Mitre", });


            //var model = new Collection<ColectivoItemViewModel>();

            //var i = 1;
            //foreach (var puntoModel in l)
            //{
            //    var punto = new GeoCoordinate(puntoModel.Latitud, puntoModel.Longitud);
            //    var itemViewModel = new ItemViewModel
            //        {
            //            Titulo = puntoModel.Nombre, 
            //            Punto = punto,
            //            Index = i++,
            //        };
            //    model.Add(itemViewModel);
            //}
            
            //RecargaLoading.Visibility = Visibility.Collapsed;
            //RecargaError.Visibility = Visibility.Collapsed;
            FinishRequest();
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