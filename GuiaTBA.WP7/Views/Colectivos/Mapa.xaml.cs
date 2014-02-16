using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GuiaTBA.WP7.Commons.Extensions;
using GuiaTBA.WP7.Commons.Helpers;
using GuiaTBA.WP7.Commons.Services;
using GuiaTBA.WP7.Commons.ViewModels;
using GuiaTBA.WP7.Extensions;
using GuiaTBA.WP7.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GuiaTBA.WP7.Views.Colectivos
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin _posicionActual;
        WebRequest _httpReq;

        public string Linea { get; set; }
        
        public Mapa()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                MiMapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);
                GetColectivo();
            };
            Unloaded += (sender, args) =>
            {
                if (_httpReq != null)
                    _httpReq.Abort();
            };
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string linea;
            if (NavigationContext.QueryString.TryGetValue("linea", out linea))
            {
                Linea = linea;
            }
        }

        void SetApplicationBarEnabled(bool isEnabled)
        {
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;
        }

        void GetColectivo()
        {
            ResetUI();

            //Esta en file system
            if (Config.Get<List<TransporteViewModel>>("linea-" + Linea) != null)
            {
                UpdateList(Config.Get<List<TransporteViewModel>>("linea-" + Linea));
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
            
            Refreshing.Visibility = Visibility.Visible;
            ProgressBar.Show(string.Format("Obteniendo recorrido línea {0}...", Linea));
            SetApplicationBarEnabled(false);

            var param = new Dictionary<string, object>
                {
                    {"linea", Linea},
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
                var httpRequest = (HttpWebRequest) result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof (List<TransporteViewModel>));
                var o = (List<TransporteViewModel>) serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateList(UpdateList), o);
            }
            catch (Exception ex)
            {
                ex.Log(ResetUI, () => { NoConnection.Visibility = Visibility.Visible; return 0; });
            }
        }

        delegate void DelegateUpdateList(List<TransporteViewModel> local);
        private void UpdateList(List<TransporteViewModel> ls)
        {
            if (Config.Get<List<TransporteViewModel>>("linea-" + Linea) == null && ls.Any())
                Config.Set("linea-" + Linea, ls);

            ls = ls.OrderBy(y => y.Nombre).ToList();

            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();
            
            try
            {

                for (int index = 0; index < ls.Count; index++)
                {
                    TransporteViewModel transporteViewModel = ls[index];
                    var routeColor = GetRandomColor(index);
                    var routeBrush = new SolidColorBrush(routeColor);

                    var routeLine = new MapPolyline
                    {
                        Name = transporteViewModel.Nombre,
                        Locations = new LocationCollection(),
                        Stroke = routeBrush,
                        Opacity = 0.8,
                        Visibility = Visibility.Collapsed,
                        StrokeThickness = 5.0,
                    };

                    foreach (var location in transporteViewModel.Puntos)
                    {
                        routeLine.Locations.Add(new GeoCoordinate(location.Y, location.X));
                    }

                    ReferencesListBox.ItemsSource = new ObservableCollection<TransporteViewModel>(ls);

                    MiMapa.Children.Add(routeLine);
                }
            }
            catch
            {
                ResetUI();
                NoResults.Visibility = Visibility.Visible;
                Config.Remove("linea-" + Linea);
                return;
            }

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? App.Configuration.Ubicacion : null);

            var x = new List<GeoCoordinate>();
            //Ajusto el mapa para mostrar los items
            foreach (var child in MiMapa.Children)
            {
                var pushpin = child as Pushpin;
                if (pushpin != null)
                {
                    x.Add(pushpin.Location);
                }

                var line = child as MapPolyline;
                if (line != null)
                {
                    x.AddRange(line.Locations);
                }
            }

            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            ResetUI();
            NoResults.Visibility = ls.Any() ? Visibility.Collapsed : Visibility.Visible;
            Results.Visibility = ls.Any() ? Visibility.Visible : Visibility.Collapsed;
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
        
        private int ResetUI()
        {
            Refreshing.Visibility = Visibility.Collapsed;
            NoResults.Visibility = Visibility.Collapsed;
            Results.Visibility = Visibility.Collapsed;
            NoConnection.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
            return 0;
        }

        
        private void ActualizarUbicacion(GeoPosition<GeoCoordinate> location)
        {
            MiMapa.Children.Remove(_posicionActual);
            if (location == null || location.Location.IsUnknown) return;

            _posicionActual = new Pushpin
                {
                    Location = location.Location,
                    Template = (ControlTemplate) (Application.Current.Resources["locationPushpinTemplate"])
                };
            MiMapa.Children.Add(_posicionActual);
        }
        
        private void BtnAcercar_Click(object sender, EventArgs e)
        {
            MiMapa.ZoomLevel++;
        }

        private void BtnAlejar_Click(object sender, EventArgs e)
        {
            MiMapa.ZoomLevel--;
        }

        private void BtnVista_Click(object sender, EventArgs e)
        {
            if (MiMapa.Mode is RoadMode)
                MiMapa.Mode = new AerialMode();
            else
                MiMapa.Mode = new RoadMode();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void References_OnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            var item = (CheckBox) sender;
            foreach (var child in MiMapa.Children.OfType<MapPolyline>().Where(x => x.Name.Equals(item.Content)))
            {
                child.Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnReferencias_Click(object sender, EventArgs e)
        {
            Results.Visibility = Results.Visibility.Equals(Visibility.Collapsed)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}