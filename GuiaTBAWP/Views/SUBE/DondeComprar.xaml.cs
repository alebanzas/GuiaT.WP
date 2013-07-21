﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class DondeComprar
    {
        private static MainViewModel _viewModel;
        public static MainViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new MainViewModel()); }
        }

        private HttpWebRequest _httpReq;

        public DondeComprar()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;

                GetDondeComprar();
            };
            Unloaded += DondeCargar_Unloaded;
        }

        private void DondeCargar_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }

        void GetDondeComprar()
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
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar donde comprar SUBE, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }

            ProgressBar.Show("Buscando más cercanos...");
            ViewModel.CurrentPosition = currentLocation.Location;
            ViewModel.PuntosVenta.Clear();
            Loading.Visibility = Visibility.Visible;
            SetApplicationBarEnabled(false);
            CancelarRequest();

            var param = new Dictionary<string, object>
                {
                    {"lat", currentLocation.Location.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    {"lon", currentLocation.Location.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    {"cant", 10},
                };

            _httpReq = (HttpWebRequest)WebRequest.Create("/sube/ventanear".ToApiCallUri(param));
            _httpReq.Method = "GET";
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            var httpRequest = (HttpWebRequest)result.AsyncState;
            var response = httpRequest.EndGetResponse(result);
            var stream = response.GetResponseStream();

            var serializer = new DataContractJsonSerializer(typeof(List<SUBEPuntoModel>));
            var o = (List<SUBEPuntoModel>)serializer.ReadObject(stream);

            Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
        }

        delegate void DelegateUpdateWebBrowser(List<SUBEPuntoModel> local);
        private void UpdateWebBrowser(List<SUBEPuntoModel> l)
        {
            ViewModel.LoadPuntosVenta(l.Select(x => new ItemViewModel
            {
                Punto = new GeoCoordinate(x.Latitud, x.Longitud),
                Titulo = x.Nombre,
            }));

            var pp = from ll in ViewModel.Pushpins
                     select ll.Location;
            Mapa.SetView(LocationRect.CreateLocationRect(pp));

            ResetUI();
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

        private void ResetUI()
        {
            Loading.Visibility = Visibility.Collapsed;
            ConnectionError.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
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
            GetDondeComprar();
        }
    }
}