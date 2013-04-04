using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using GuiaTBAWP.Bing.Geocode;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class DondeCargar : PhoneApplicationPage
    {
        private static MainViewModel _viewModel;
        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new MainViewModel());
            }
        }

        public GeoCoordinateWatcher Ubicacion { get; set; }

        #region TODO: mover


        public static void RefreshAllPushpins()
        {
            foreach (var recarga in ViewModel.PuntosRecarga)
            {
                CreateNewRecargaPushpin(recarga);
            }
        }

        private void CreateNewPushpin(Point point)
        {
            // Translate the map viewport touch point to a geo coordinate.
            GeoCoordinate location;
            Mapa.TryViewportPointToLocation(point, out location);
            CreateNewPushpin(location);
        }

        public static void CreateNewPushpin(GeoCoordinate location)
        {
            var pushpin = ViewModel.DefaultPushPin.Clone(location);
            CreateNewPushpin(pushpin);
        }
        
        public static void CreateNewRecargaPushpin(ItemViewModel itemViewModel)
        {
            var pushpin = ViewModel.RecargaPushPin.Clone(itemViewModel.Punto);
            pushpin.Title = itemViewModel.Titulo;
            pushpin.Index = itemViewModel.Index;
            CreateNewPushpin(pushpin);
        }

        private static void CreateNewPushpin(PushpinModel pushpin)
        {
            ViewModel.Pushpins.Add(pushpin);
        }

/*
        private void CreateNewPushpin(object selectedItem, Point point)
        {
            // Use the geo coordinate calculated to add a new pushpin,
            // based on the selected pushpin prototype,
            var pushpinPrototype = selectedItem as PushpinModel;
            if (pushpinPrototype == null) return;

            CreateNewPushpin(point);
        }
*/

        #endregion

        readonly ProgressIndicator _progress = new ProgressIndicator();

        public DondeCargar()
        {
            InitializeComponent();

            ResetUI();

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("localizacion"))
                IsolatedStorageSettings.ApplicationSettings["localizacion"] = true;
            InitializeGPS();
            SetLocationService();

            if (!NetworkInterface.GetIsNetworkAvailable() ||
                (Ubicacion.Permission.Equals(GeoPositionPermission.Denied) ||
                 Ubicacion.Permission.Equals(GeoPositionPermission.Unknown)))
            {
                Loaded += (s, e) =>
                {
                    var ns = NavigationService;
                    ns.Navigate(new Uri("/Error.xaml", UriKind.Relative));
                };
                return;
            }
            Loaded += (s, e) =>
                {
                    DataContext = ViewModel;

                    _progress.IsVisible = true;
                    _progress.IsIndeterminate = true;

                    if (ViewModel.IsPuntosRecargaLoaded) return;

                    RecargaLoading.Visibility = Visibility.Visible;
                    StartLocationService();
                };
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

            if (location.Latitude == ViewModel.CurrentLocation.Latitude && location.Longitude == ViewModel.CurrentLocation.Longitude)
            {
                return;
            }

            ViewModel.CurrentLocation = e.Position.Location;

            SetLocation(location);
            CreateNewPushpin(location);
            //StopLocationService();
            SetProgressBar(null);

            GetMasCercanos();
        }

        private int _pendingRequests;
        private void GetMasCercanos()
        {
            SetProgressBar("Buscando más cercano...");

            var httpReq = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("http://servicio.abhosting.com.ar/sube/recarganear/?lat={0}&lon={1}&cant=10", ViewModel.CurrentLocation.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."), ViewModel.CurrentLocation.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."))));
            httpReq.Method = "POST";
            httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);
            _pendingRequests++;
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
                RecargaLoading.Visibility = Visibility.Collapsed;
                RecargaError.Visibility = Visibility.Visible;
                FinishRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }

        delegate void DelegateUpdateWebBrowser(List<SUBEPuntoModel> local);
        private void UpdateWebBrowser(List<SUBEPuntoModel> l)
        {
            var model = new Collection<ItemViewModel>();
            var i = 0;
            foreach (var puntoModel in l)
            {
                var punto = new GeoCoordinate(puntoModel.Latitud, puntoModel.Longitud);
                var itemViewModel = new ItemViewModel
                    {
                        Titulo = puntoModel.Nombre, 
                        Punto = punto,
                        Index = i++,
                    };
                model.Add(itemViewModel);
                CreateNewRecargaPushpin(itemViewModel);
            }

            if (!ViewModel.IsPuntosRecargaLoaded)
            {
                ViewModel.LoadPuntosRecarga(model);
            }

            var x = from ll in ViewModel.Pushpins
                    select ll.Location;
            Mapa.SetView(LocationRect.CreateLocationRect(x));


            RecargaLoading.Visibility = Visibility.Collapsed;
            RecargaError.Visibility = Visibility.Collapsed;
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


        private static void SetLocation(GeoCoordinate location)
        {
            // Center map to default location.
            ViewModel.Center = location;

            // Reset zoom default level.
            ViewModel.Zoom = ViewModel.DefaultZoomLevel;
        }

        private void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pushpin = sender as Pushpin;

            // Center the map on a pushpin when touched.
            if (pushpin != null) ViewModel.Center = pushpin.Location;
        }
        
        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SUBE/Opciones.xaml", UriKind.Relative));
        }

        private void MiMapa_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/SUBE/Mapa.xaml", UriKind.Relative));
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            ResetLocationService();
        }
    }
}