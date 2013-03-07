using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using GuiaTBAWP.Bing.Route;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using GeocodeLocation = GuiaTBAWP.Bing.Geocode.GeocodeLocation;

namespace GuiaTBAWP
{
    public partial class PuntosDeVentaYCarga : PhoneApplicationPage
    {
        ProgressIndicator progress = new ProgressIndicator();
        private int requests;

        public PuntosDeVentaYCarga()
        {
            InitializeComponent();

            SetLocationService(GeoPositionAccuracy.High);


            if (!NetworkInterface.GetIsNetworkAvailable() ||
                (App.ViewModel.watcher.Permission.Equals(GeoPositionPermission.Denied) ||
                 App.ViewModel.watcher.Permission.Equals(GeoPositionPermission.Unknown)))
            {
                this.Loaded += (s, e) =>
                {
                    var ns = NavigationService;
                    ns.Navigate(new Uri("/Error.xaml", UriKind.Relative));
                };
                return;
            }

            DataContext = App.ViewModel;

            progress.IsVisible = true;
            progress.IsIndeterminate = true;

            StartLocationService();
        }
        
        private void SetProgressBar(string msj, bool showProgress = true)
        {
            if (string.IsNullOrEmpty(msj))
            {
                SystemTray.SetProgressIndicator(this, null);
            }
            else
            {
                progress.Text = msj;
                progress.IsIndeterminate = showProgress;
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetProgressIndicator(this, progress);
            }
        }


        //TODO: Extract to class
        #region Location Service

        /// <summary>
        /// Helper method to start up the location data acquisition
        /// </summary>
        /// <param name="accuracy">The accuracy level </param>
        private void SetLocationService(GeoPositionAccuracy accuracy)
        {
            // Reinitialize the GeoCoordinateWatcher
            //StatusTextBlock.Text = "starting, " + accuracyText;
            App.ViewModel.watcher = new GeoCoordinateWatcher(accuracy) { MovementThreshold = 20 };

            // Add event handlers for StatusChanged and PositionChanged events
            App.ViewModel.watcher.StatusChanged += watcher_StatusChanged;
            App.ViewModel.watcher.PositionChanged += watcher_PositionChanged;
        }

        /// <summary>
        /// Helper method to start up the location data acquisition
        /// </summary>
        private void StartLocationService()
        {
            SetProgressBar("Buscando posición...");
            // Start data acquisition
            App.ViewModel.watcher.Start();
        }

        /// <summary>
        /// Helper method to stop up the location data acquisition
        /// </summary>
        private void StopLocationService()
        {
            // Start data acquisition
            App.ViewModel.watcher.Stop();
        }

        /// <summary>
        /// Handler for the StatusChanged event. This invokes MyStatusChanged on the UI thread and
        /// passes the GeoPositionStatusChangedEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(() => MyStatusChanged(e));

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

            if (location.Latitude == App.ViewModel.CurrentLocation.Latitude && location.Longitude == App.ViewModel.CurrentLocation.Longitude)
            {
                return;
            }

            App.ViewModel.CurrentLocation = e.Position.Location;

            SetLocation(location);
            CreateNewPushpin(location);
            StopLocationService();
            SetProgressBar(null);

            GetMasCercanos();
        }

        private void GetMasCercanos()
        {
            SetProgressBar("Buscando mas cercano...");

            requests = 2;

            HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(string.Format("http://servicio.abhosting.com.ar/SUBE/recarganear/?lat={0}&lon={1}&cant=10", App.ViewModel.CurrentLocation.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."), App.ViewModel.CurrentLocation.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."))));
            httpReq.Method = "POST";
            httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);

            HttpWebRequest httpReq2 = (HttpWebRequest)HttpWebRequest.Create(new Uri(string.Format("http://servicio.abhosting.com.ar/SUBE/ventanear/?lat={0}&lon={1}&cant=10", App.ViewModel.CurrentLocation.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."), App.ViewModel.CurrentLocation.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."))));
            httpReq2.Method = "POST";
            httpReq2.BeginGetResponse(HTTPWebRequestCallBackVenta, httpReq2);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse response = httpRequest.EndGetResponse(result);
                Stream stream = response.GetResponseStream();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<SUBEPuntoModel>));
                var o = (List<SUBEPuntoModel>)serializer.ReadObject(stream);

                this.Dispatcher.BeginInvoke(new delegateUpdateWebBrowser(updateWebBrowser), o);
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
            }
        }

        private void HTTPWebRequestCallBackVenta(IAsyncResult result)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse response = httpRequest.EndGetResponse(result);
                Stream stream = response.GetResponseStream();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<SUBEPuntoModel>));
                var o = (List<SUBEPuntoModel>)serializer.ReadObject(stream);

                this.Dispatcher.BeginInvoke(new delegateUpdateWebBrowser(updateWebBrowserVenta), o);
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
            }
        }

        delegate void delegateUpdateWebBrowser(List<SUBEPuntoModel> local);
        private void updateWebBrowser(List<SUBEPuntoModel> l)
        {
            var model = new Collection<ItemViewModel>();
            foreach (var puntoModel in l)
            {
                model.Add(new ItemViewModel
                            {
                                Titulo = puntoModel.Nombre,
                            });
                CreateNewRecargaPushpin(new GeoCoordinate(puntoModel.Latitud, puntoModel.Longitud));
            }

            if (!App.ViewModel.IsPuntosRecargaLoaded)
            {
                App.ViewModel.LoadPuntosRecarga(model);
            }

            requests--;
            if(requests == 0)
                SetProgressBar(null);
        }
        private void updateWebBrowserVenta(List<SUBEPuntoModel> l)
        {
            var model = new Collection<ItemViewModel>();
            foreach (var puntoModel in l)
            {
                model.Add(new ItemViewModel
                {
                    Titulo = puntoModel.Nombre,
                });
                CreateNewVentaPushpin(new GeoCoordinate(puntoModel.Latitud, puntoModel.Longitud));
            }

            if (!App.ViewModel.IsPuntosVentaLoaded)
            {
                App.ViewModel.LoadPuntosVenta(model);
            }

            requests--;
            if (requests == 0)
                SetProgressBar(null);
        }

        #endregion


        private void SetLocation(GeoCoordinate location)
        {
            // Center map to default location.
            App.ViewModel.Center = location;

            // Reset zoom default level.
            App.ViewModel.Zoom = App.ViewModel.DefaultZoomLevel;
        }

        private void CreateNewPushpin(Point point)
        {
            // Translate the map viewport touch point to a geo coordinate.
            GeoCoordinate location;
            Map.TryViewportPointToLocation(point, out location);
            CreateNewPushpin(location);
        }

        private void CreateNewPushpin(GeoCoordinate location)
        {
            var pushpin = App.ViewModel.DefaultPushPin.Clone(location);
            CreateNewPushpin(pushpin);
        }

        private void CreateNewVentaPushpin(GeoCoordinate location)
        {
            var pushpin = App.ViewModel.VentaPushPin.Clone(location);
            CreateNewPushpin(pushpin);
        }

        private void CreateNewRecargaPushpin(GeoCoordinate location)
        {
            var pushpin = App.ViewModel.RecargaPushPin.Clone(location);
            CreateNewPushpin(pushpin);
        }

        private void CreateNewPushpin(PushpinModel pushpin)
        {
            App.ViewModel.Pushpins.Add(pushpin);
        }

        private void CreateNewPushpin(object selectedItem, Point point)
        {
            // Use the geo coordinate calculated to add a new pushpin,
            // based on the selected pushpin prototype,
            var pushpinPrototype = selectedItem as PushpinModel;
            if (pushpinPrototype == null) return;

            CreateNewPushpin(point);
        }

        private void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pushpin = sender as Pushpin;

            // Center the map on a pushpin when touched.
            if (pushpin != null) App.ViewModel.Center = pushpin.Location;
        }

        private void ButtonZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.ViewModel.Zoom += 1;
        }

        private void ButtonZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.ViewModel.Zoom -= 1;
        }

    }

    public class SUBEPuntoModel
    {
        public virtual string Nombre { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}