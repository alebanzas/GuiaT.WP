using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GuiaTBA.Common.ViewModels;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using ProgressBar = GuiaTBA.Common.ProgressBar;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class PuntosSUBE
    {
        MapLayer _pointsLayer = new MapLayer();
        private static MainViewModel _viewModel;
        public static MainViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new MainViewModel(App.Configuration.Ubicacion.Location)); }
        }

        private HttpWebRequest _httpReq;
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var titulo = NavigationContext.QueryString["titulo"];
            var tipo = NavigationContext.QueryString["tipo"];

            ViewModel.Titulo = titulo;
            ViewModel.Tipo = tipo;

            base.OnNavigatedTo(e);
        }

        public PuntosSUBE()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                Panorama.Title = ViewModel.Titulo;
                PanoramaItem.Header = ViewModel.Tipo;
                Mapa.Layers.Add(_pointsLayer);
                _pointsLayer.Clear();
                ViewModel.Puntos.Clear();

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
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }
            if (currentLocation == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar, por favor, active la función de localización."));
                return;
            }

            ProgressBar.Show("Buscando más cercanos...");

            if (App.Configuration.IsLocationEnabled && PositionService.GetCurrentLocation().Location != null)
            {
                var pushpin = new MapOverlay
                {
                    GeoCoordinate = PositionService.GetCurrentLocation().Location,
                    ContentTemplate = Application.Current.Resources["locationPushpinTemplate"] as DataTemplate,
                };
                _pointsLayer.Add(pushpin);

                Mapa.SetView(Mapa.CreateBoundingRectangle());
            }
            if (ViewModel.Puntos.Count == 0) Refreshing.Visibility = Visibility.Visible;
            SetApplicationBarEnabled(false);
            CancelarRequest();

            var param = new Dictionary<string, object>
                {
                    {"cant", 10},
                };

            var client = new HttpClient();
            _httpReq = client.Get(string.Format("/api/{0}sube", ViewModel.Tipo).ToApiCallUri(param));
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
            ViewModel.Puntos.Clear();
            var i = 1;
            foreach (var it in l)
            {
                var item = new ItemViewModel
                {
                    Index = i,
                    Punto = new GeoCoordinate(it.Latitud, it.Longitud),
                    Titulo = it.Nombre,
                };
                _pointsLayer.Add(GetMapOverlay(i.ToString(CultureInfo.InvariantCulture), item.Punto));
                ViewModel.Puntos.Add(item);
                i++;
            }

            Mapa.SetView(Mapa.CreateBoundingRectangle());

            ResetUI();
            if (ViewModel.Puntos.Count == 0)
            {
                NoResults.Visibility = Visibility.Visible;
            }
        }

        private MapOverlay GetMapOverlay(String text, GeoCoordinate location)
        {

            var oneMarker = new MapOverlay {GeoCoordinate = location};

            var canCan = new Canvas();

            var circhegraphic = new Ellipse
            {
                Fill = new SolidColorBrush(ColorTranslator.FromHtml("#6495ED")),
                Stroke = new SolidColorBrush(ColorTranslator.FromHtml("#FFFFFF")),
                StrokeThickness = 5,
                Opacity = 0.8,
                Height = 40,
                Width = 40
            };

            canCan.Children.Add(circhegraphic);
            var textt = new TextBlock {Text = text, HorizontalAlignment = HorizontalAlignment.Center};
            Canvas.SetLeft(textt, 10);
            Canvas.SetTop(textt, 5);
            Canvas.SetZIndex(textt, 5);

            canCan.Children.Add(textt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);

            return oneMarker;
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
            if (pushpin != null) ViewModel.Center = pushpin.GeoCoordinate;
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