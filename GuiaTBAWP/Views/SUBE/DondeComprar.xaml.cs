﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using GuiaTBAWP.Bing.Geocode;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class DondeComprar : PhoneApplicationPage
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
        
        #region TODO: mover


        public static void RefreshAllPushpins()
        {
            foreach (var venta in ViewModel.PuntosVenta)
            {
                CreateNewVentaPushpin(venta);
            }
        }

        public static void CreateNewPushpin(GeoCoordinate location)
        {
            var pushpin = ViewModel.DefaultPushPin.Clone(location);
            CreateNewPushpin(pushpin);
        }

        public static void CreateNewVentaPushpin(ItemViewModel itemViewModel)
        {
            var pushpin = ViewModel.VentaPushPin.Clone(itemViewModel.Punto);
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

        private HttpWebRequest _httpReq;

        public DondeComprar()
        {
            InitializeComponent();

            ResetUI();

            Unloaded += DondeComprar_Unloaded;

            Loaded += (s, e) =>
                {
                    DataContext = ViewModel;

                    _progress.IsVisible = true;
                    _progress.IsIndeterminate = true;

                    if (ViewModel.IsPuntosVentaLoaded) return;

                    VentaLoading.Visibility = Visibility.Visible;
                    
                    //SetProgressBar("Buscando posición...");
                    var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (applicationBarIconButton != null)
                        applicationBarIconButton.IsEnabled = false;
                    GetDondeComprar();
                };
        }

        private void DondeComprar_Unloaded(object sender, RoutedEventArgs e)
        {
            if(_httpReq != null)
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

        void GetDondeComprar()
        {
            var e = App.Ubicacion;
            var location = new GeocodeLocation
            {
                Latitude = e.Position.Location.Latitude,
                Longitude = e.Position.Location.Longitude
            };

            if (Math.Abs(location.Latitude - ViewModel.CurrentLocation.Latitude) < App.Configuration.MinDiffGeography && Math.Abs(location.Longitude - ViewModel.CurrentLocation.Longitude) < App.Configuration.MinDiffGeography)
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

            var param = new Dictionary<string, object>
                {
                    {"lat", ViewModel.CurrentLocation.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    {"lon", ViewModel.CurrentLocation.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    {"cant", 10},
                };

            _httpReq = (HttpWebRequest)WebRequest.Create("/sube/ventanear".ToApiCallUri(param));
            _httpReq.Method = "GET";
            _httpReq.BeginGetResponse(HTTPWebRequestCallBackVenta, _httpReq);
            _pendingRequests++;
        }
        
        private void HTTPWebRequestCallBackVenta(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<SUBEPuntoModel>));
                var o = (List<SUBEPuntoModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowserVenta), o);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        VentaLoading.Visibility = Visibility.Collapsed;
                        VentaError.Visibility = Visibility.Visible;
                        FinishRequest();
                    });
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }
        delegate void DelegateUpdateWebBrowser(List<SUBEPuntoModel> local);

        private void FinishRequest()
        {
            _pendingRequests--;
            if (_pendingRequests != 0) return;
            
            ResetUI();
        }

        private void ResetUI()
        {
            SetProgressBar(null);
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = true;
        }

        private void UpdateWebBrowserVenta(List<SUBEPuntoModel> l)
        {
            var model = new Collection<ItemViewModel>();
            var i = 1;
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
                CreateNewVentaPushpin(itemViewModel);
            }

            if (!ViewModel.IsPuntosVentaLoaded)
            {
                ViewModel.LoadPuntosVenta(model);
            }

            VentaLoading.Visibility = Visibility.Collapsed;
            VentaError.Visibility = Visibility.Collapsed;
            FinishRequest();
        }
        
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
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void MiMapa_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/SUBE/Mapa.xaml", UriKind.Relative));
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            GetDondeComprar();
        }
    }
}