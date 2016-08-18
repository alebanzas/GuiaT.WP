using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.Views.Colectivos;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace GuiaTBAWP.Views.Ruta
{
    public partial class Home
    {
        private GetColectivoMapService _getColectivoMapService;
        //private GeocodeQuery _geoQo;
        //private GeocodeQuery _geoQd;
        private GeoCoordinate _origen;
        private GeoCoordinate _destino;
        private WebRequest _httpReq;
        private bool _results;
        private Stack<int> _navHistory;
        private bool Initialized { get; set; }

        private static RuteBusquedaViewModel _viewModel = new RuteBusquedaViewModel();
        public static RuteBusquedaViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new RuteBusquedaViewModel()); }
        }
        
        public Home()
        {
            InitializeComponent();
            
            DataContext = ViewModel;
            Loaded += (sender, args) =>
            {
                //_geoQo = new GeocodeQuery();
                //_geoQo.QueryCompleted += geoQ_OrigenQueryCompleted;
                //_geoQd = new GeocodeQuery();
                //_geoQd.QueryCompleted += geoQ_DestinoQueryCompleted;

                if (Initialized) return;

                Initialized = true;
                _getColectivoMapService = new GetColectivoMapService();
                _navHistory = new Stack<int>();
                _navHistory.Push(0);
                BtnGpsOrigen.IsEnabled = App.Configuration.IsLocationEnabled;
                BtnGpsDestino.IsEnabled = App.Configuration.IsLocationEnabled;
                ViewModel.BusquedaOrigen.Clear();
                ViewModel.BusquedaDestino.Clear();
                TxtOrigen.Text = "Seleccione origen";
                TxtOrigen.Tap += (o, eventArgs) =>
                {
                    PivotControl.SelectedIndex = 0;
                };
                TxtDestino.Text = "Seleccione destino";
                TxtDestino.Tap += (o, eventArgs) =>
                {
                    PivotControl.SelectedIndex = 1;
                };
            };
            //Unloaded += (sender, args) =>
            //    {
            //        if (_httpReq != null)
            //            _httpReq.Abort();
            //        if (_geoQo != null)
            //        {
            //            _geoQo.CancelAsync();
            //            _geoQo.Dispose();
            //        }
            //        if (_geoQd != null)
            //        {
            //            _geoQd.CancelAsync();
            //            _geoQd.Dispose();
            //        }
            //    };
            
            StatusChecker.Check("HomeRuta");
        }

        private void ButtonBuscarOrigen_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBuscarOrigen.Text)) return;


            var client = new HttpClient();
            var param = new Dictionary<string, object> 
            {
                {"id", TxtBuscarOrigen.Text},
            };
            _httpReq = client.Get(("/api/geocoder").ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestBuscarOrigenCallBack, _httpReq);


            //if (_geoQo.IsBusy)
            //{
            //    _geoQo.CancelAsync();
            //}
            //
            //_geoQo.GeoCoordinate = new GeoCoordinate(-34.603577, -58.381802, 1000);
            //_geoQo.SearchTerm = TxtBuscarOrigen.Text;
            //_geoQo.MaxResultCount = 200;
            //_geoQo.QueryAsync();
            BtnBuscarOrigen.IsEnabled = false;
            ViewModel.BusquedaOrigen.Clear(); 
            MiMapaOrigen.Layers.Clear();
            NoResultsOrigen.Visibility = Visibility.Collapsed;
            LoadingOrigen.Visibility = Visibility.Visible;
        }
        private void ButtonBuscarDestino_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBuscarDestino.Text)) return;

            var client = new HttpClient();
            var param = new Dictionary<string, object>
            {
                {"id", TxtBuscarDestino.Text},
            };
            _httpReq = client.Get(("/api/geocoder").ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestBuscarDestinoCallBack, _httpReq);

            //if (_geoQd.IsBusy)
            //{
            //    _geoQd.CancelAsync();
            //}

            //_geoQd.GeoCoordinate = new GeoCoordinate(-34.603577, -58.381802, 1000);
            //_geoQd.SearchTerm = TxtBuscarDestino.Text;
            //_geoQd.MaxResultCount = 200;
            //_geoQd.QueryAsync();
            BtnBuscarDestino.IsEnabled = false;
            MiMapaDestino.Layers.Clear();
            ViewModel.BusquedaDestino.Clear();
            NoResultsDestino.Visibility = Visibility.Collapsed;
            LoadingDestino.Visibility = Visibility.Visible;
        }

        private void HTTPWebRequestBuscarOrigenCallBack(IAsyncResult result)
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    LoadingOrigen.Visibility = Visibility.Collapsed;
                    BtnBuscarOrigen.IsEnabled = true;

                    var httpRequest = (HttpWebRequest)result.AsyncState;
                    var response = httpRequest.EndGetResponse(result);
                    var stream = response.GetResponseStream();
                    var serializer = new DataContractJsonSerializer(typeof(List<GeocoderResult>));
                    var list = (List<GeocoderResult>)serializer.ReadObject(stream);

                    if (!list.Any())
                    {
                        NoResultsOrigen.Visibility = Visibility.Visible;
                        return;
                    }

                    var mapLayer = new MapLayer();
                    for (var index = 0; index < list.Count; index++)
                    {
                        var geocoderResult = list[index];
                        ViewModel.BusquedaOrigen.Add(geocoderResult);
                        mapLayer.Add(GetMapOverlay((index+1).ToString(CultureInfo.InvariantCulture),
                            new GeoCoordinate(geocoderResult.X, geocoderResult.Y)));
                    }

                    MiMapaOrigen.Layers.Add(mapLayer);
                    SetMapView(MiMapaOrigen, mapLayer);

                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            });
        }

        private void HTTPWebRequestBuscarDestinoCallBack(IAsyncResult result)
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    LoadingDestino.Visibility = Visibility.Collapsed;
                    BtnBuscarDestino.IsEnabled = true;

                    var httpRequest = (HttpWebRequest)result.AsyncState;
                    var response = httpRequest.EndGetResponse(result);
                    var stream = response.GetResponseStream();
                    var serializer = new DataContractJsonSerializer(typeof(List<GeocoderResult>));
                    var list = (List<GeocoderResult>)serializer.ReadObject(stream);

                    if (!list.Any())
                    {
                        NoResultsDestino.Visibility = Visibility.Visible;
                        return;
                    }

                    var mapLayer = new MapLayer();
                    for (var index = 0; index < list.Count; index++)
                    {
                        var geocoderResult = list[index];
                        ViewModel.BusquedaDestino.Add(geocoderResult);
                        mapLayer.Add(GetMapOverlay((index + 1).ToString(CultureInfo.InvariantCulture),
                            new GeoCoordinate(geocoderResult.X, geocoderResult.Y)));
                    }

                    MiMapaDestino.Layers.Add(mapLayer);
                    SetMapView(MiMapaDestino, mapLayer);

                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            });
        }

        private void ButtonGpsOrigen_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPosition = PositionService.GetCurrentLocation();
            if (currentPosition != null)
            {
                _origen = currentPosition.Location;
                MiMapaOrigen.Layers.Clear();
                MiMapaOrigen.Layers.Add(new MapLayer{ new MapOverlay
                {
                    GeoCoordinate = currentPosition.Location,
                    ContentTemplate = Application.Current.Resources["locationPushpinTemplate"] as DataTemplate,
                } });
                MiMapaOrigen.Center = currentPosition.Location;
                MiMapaOrigen.ZoomLevel = 15;
                TxtBuscarOrigen.Text = "Mi Ubicación";
                TxtOrigen.Text = "Mi Ubicación";
                BtnBuscar.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("No se pudo obtener la posición actual.");
            }
        }

        private void ButtonGpsDestino_OnClick(object sender, RoutedEventArgs e)
        {
            var currentPosition = PositionService.GetCurrentLocation();
            if (currentPosition != null)
            {
                _destino = currentPosition.Location;
                MiMapaDestino.Layers.Clear();
                MiMapaDestino.Layers.Add(new MapLayer{ new MapOverlay
                {
                    GeoCoordinate = currentPosition.Location,
                    ContentTemplate = Application.Current.Resources["locationPushpinTemplate"] as DataTemplate,
                } });
                MiMapaDestino.Center = currentPosition.Location;
                MiMapaDestino.ZoomLevel = 15;
                TxtBuscarDestino.Text = "Mi Ubicación";
                TxtDestino.Text = "Mi Ubicación";
                BtnBuscar.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("No se pudo obtener la posición actual.");
            }
        }

        private void geoQ_OrigenQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            //var list = e.Result.Where(x => x.Information.Address.Neighborhood.Equals("Gran Buenos Aires")).ToList();
            var list = e.Result.ToList();
            LoadingOrigen.Visibility = Visibility.Collapsed;
            BtnBuscarOrigen.IsEnabled = true;

            if (list.Any())
            {
                var mapLayer = new MapLayer();

                for (int index = 1; index <= list.Count; index++)
                {
                    var mapLocation = list[index-1];
                    var nombre = string.Format("{0}. {1} {2}", index, mapLocation.Information.Address.Street, mapLocation.Information.Address.HouseNumber);
                    var detalles = mapLocation.Information.Address.District;
                    if (!string.IsNullOrWhiteSpace(detalles)) { detalles += ", "; }
                    detalles += mapLocation.Information.Address.City;
                    
                    ViewModel.BusquedaOrigen.Add(new GeocoderResult
                    {
                        Nombre = nombre,
                        Detalles = detalles,
                        X = mapLocation.GeoCoordinate.Latitude,
                        Y = mapLocation.GeoCoordinate.Longitude,
                    });
                    mapLayer.Add(GetMapOverlay(index.ToString(CultureInfo.InvariantCulture), mapLocation.GeoCoordinate));
                }

                MiMapaOrigen.Layers.Add(mapLayer);
                SetMapView(MiMapaOrigen, mapLayer);
            }
            else
            {
                NoResultsOrigen.Visibility = Visibility.Visible;
            }
        }

        private void geoQ_DestinoQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            var list = e.Result.Where(x => x.Information.Address.Neighborhood.Equals("Gran Buenos Aires")).ToList();
            LoadingDestino.Visibility = Visibility.Collapsed;
            BtnBuscarDestino.IsEnabled = true;

            if (list.Any())
            {
                var mapLayer = new MapLayer();

                for (int index = 1; index <= list.Count; index++)
                {
                    var mapLocation = list[index - 1];
                    var nombre = string.Format("{0}. {1} {2}", index, mapLocation.Information.Address.Street, mapLocation.Information.Address.HouseNumber);
                    var detalles = mapLocation.Information.Address.District;
                    if (!string.IsNullOrWhiteSpace(detalles)) { detalles += ", "; }
                    detalles += mapLocation.Information.Address.City;

                    ViewModel.BusquedaDestino.Add(new GeocoderResult
                    {
                        Nombre = nombre,
                        Detalles = detalles,
                        X = mapLocation.GeoCoordinate.Latitude,
                        Y = mapLocation.GeoCoordinate.Longitude,
                    });
                    mapLayer.Add(GetMapOverlay(index.ToString(CultureInfo.InvariantCulture), mapLocation.GeoCoordinate));
                }

                MiMapaDestino.Layers.Add(mapLayer);
                SetMapView(MiMapaDestino, mapLayer);
            }
            else
            {
                NoResultsDestino.Visibility = Visibility.Visible;
            }
        }
        
        private void SelectorOrigen_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultadosListOrigen.SelectedIndex == -1) return;

            var r = e.AddedItems[0] as GeocoderResult;
            if (r == null) return;

            BtnBuscar.IsEnabled = true;
            //TxtOrigen.Text = string.Format("{0}, {1}", string.Join(".", r.Nombre.Split('.').Skip(1)), r.Detalles);
            TxtOrigen.Text = r.Nombre;

            var point = new GeoCoordinate(r.X, r.Y);
            MiMapaOrigen.Center = point;
            MiMapaOrigen.ZoomLevel = 15;
            _origen = point;
        }

        private void SelectorDestino_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultadosListDestino.SelectedIndex == -1) return;

            var r = e.AddedItems[0] as GeocoderResult;
            if (r == null) return;

            BtnBuscar.IsEnabled = true;
            //TxtDestino.Text = string.Format("{0}, {1}", string.Join(".", r.Nombre.Split('.').Skip(1)), r.Detalles);
            TxtDestino.Text = r.Nombre;

            var point = new GeoCoordinate(r.X, r.Y);
            MiMapaDestino.Center = point;
            MiMapaDestino.ZoomLevel = 15;
            _destino = point;
        }

        private MapOverlay GetMapOverlay(String text, GeoCoordinate location)
        {

            var oneMarker = new MapOverlay { GeoCoordinate = location };

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
            var textt = new TextBlock { Text = text, HorizontalAlignment = HorizontalAlignment.Center };
            Canvas.SetLeft(textt, 10);
            Canvas.SetTop(textt, 5);
            Canvas.SetZIndex(textt, 5);

            canCan.Children.Add(textt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);

            return oneMarker;
        }

        private void SetMapView(Map map, MapLayer mapLayer)
        {
            if (mapLayer.Count() == 1)
            {
                map.Center = mapLayer[0].GeoCoordinate;
            }
            else
            {

                bool gotRect = false;
                double north = 0;
                double west = 0;
                double south = 0;
                double east = 0;

                for (var p = 0; p < mapLayer.Count(); p++)
                {
                    if (!gotRect)
                    {
                        gotRect = true;
                        north = south = mapLayer[p].GeoCoordinate.Latitude;
                        west = east = mapLayer[p].GeoCoordinate.Longitude;
                    }
                    else
                    {
                        if (north < mapLayer[p].GeoCoordinate.Latitude) north = mapLayer[p].GeoCoordinate.Latitude;
                        if (west > mapLayer[p].GeoCoordinate.Longitude) west = mapLayer[p].GeoCoordinate.Longitude;
                        if (south > mapLayer[p].GeoCoordinate.Latitude) south = mapLayer[p].GeoCoordinate.Latitude;
                        if (east < mapLayer[p].GeoCoordinate.Longitude) east = mapLayer[p].GeoCoordinate.Longitude;
                    }
                }

                if (gotRect)
                {
                    map.SetView(new LocationRectangle(north, west, south, east));
                }
            }
        }

        private void ButtonBusqueda_OnClick(object sender, RoutedEventArgs e)
        {
            if (_origen == null || _destino == null) return;

            var button = (Button) sender;
            button.IsEnabled = false;

            LoadingBuscar.Visibility = Visibility.Visible;
            ConnectionErrorBuscar.Visibility = Visibility.Collapsed;
            NoResultsBuscar.Visibility = Visibility.Collapsed;

            var param = new Dictionary<string, object>
                {
                    {"lat", _origen.Latitude.ToString(CultureInfo.InvariantCulture).Replace(',','.')},
                    {"lon", _origen.Longitude.ToString(CultureInfo.InvariantCulture).Replace(',','.')},
                    {"lat2", _destino.Latitude.ToString(CultureInfo.InvariantCulture).Replace(',','.')},
                    {"lon2", _destino.Longitude.ToString(CultureInfo.InvariantCulture).Replace(',','.')},
                };
            var client = new HttpClient();
            _httpReq = client.Get("/api/transporte".ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            var o = new List<TransporteModel>();

            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();
                var serializer = new DataContractJsonSerializer(typeof(List<TransporteModel>));
                o = (List<TransporteModel>)serializer.ReadObject(stream);
            }
            catch (Exception ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    BtnBuscar.IsEnabled = true;
                    LoadingBuscar.Visibility = Visibility.Collapsed;
                    NoResultsBuscar.Visibility = Visibility.Collapsed;
                    ConnectionErrorBuscar.Visibility = Visibility.Visible;
                });
                ex.Log();
                return;
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ViewModel.BusquedaResultados.Clear();
                if (o.Any())
                {
                    foreach (var transporteModel in o.GroupBy(x => x.Linea))
                    {
                        ViewModel.BusquedaResultados.Add(new ColectivoItemViewModel
                        {
                            Id = transporteModel.Key,
                            Nombre = "Línea " + transporteModel.Key,
                            Detalles = SetDetalleByLinea(transporteModel.Key, o),
                        });
                    }
                    _results = true;
                    _navHistory.Clear();
                    _navHistory.Push(0);
                    _navHistory.Push(1);
                    _navHistory.Push(2);
                    PivotControl.SelectedIndex = 3;
                }
                else
                {
                    NoResultsBuscar.Visibility = Visibility.Visible;
                }
                LoadingBuscar.Visibility = Visibility.Collapsed;
            });
        }

        private string SetDetalleByLinea(string key, IEnumerable<TransporteModel> transporteModels)
        {
            return string.Join("\n", transporteModels.Where(x => x.Linea.Equals(key)).OrderBy(x => x.Ramal).Select(x => x.Ramal));
        }

        private void PivotControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;
            if (pivot == null) return;

            if (!_results && pivot.SelectedIndex == 3)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    pivot.SelectedIndex = 2; 
                });
                return;
            }

            if (_navHistory != null && _navHistory.Peek() != pivot.SelectedIndex) _navHistory.Push(pivot.SelectedIndex);
        }

        private void UIElement_OnDoubleTap(object sender, GestureEventArgs e)
        {
            PivotControl.SelectedIndex++;
        }
        
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (PivotControl.SelectedIndex != 4 && PivotControl.SelectedIndex != _navHistory.Peek())
            {
                _navHistory.Clear();
                _navHistory.Push(0);
                _navHistory.Push(1);
                _navHistory.Push(2);
            }

            if (!_navHistory.Any()) return;
            _navHistory.Pop();
            if (!_navHistory.Any()) return;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                PivotControl.SelectedIndex = _navHistory.Peek();
            });
            e.Cancel = true;
        }

        private void Resultados_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListBox;
            if (list == null) return;
            var item = list.SelectedItem as ColectivoItemViewModel;
            if (item == null) return;

            App.MapViewModel.Reset();
            _getColectivoMapService.SuccessFunc = () =>
            {
                App.MapViewModel.AddElement(new MapReference
                {
                    Checked = true,
                    Nombre = "Origen"
                }, new MapOverlay
                {
                    GeoCoordinate = _origen,
                    ContentTemplate = Application.Current.Resources["locationPushpinTemplate"] as DataTemplate,
                });
                App.MapViewModel.AddElement(new MapReference
                {
                    Checked = true,
                    Nombre = "Destino"
                }, new MapOverlay
                {
                    GeoCoordinate = _destino,
                    ContentTemplate = Application.Current.Resources["locationPushpinTemplate"] as DataTemplate,
                });
                NavigationService.Navigate(new Uri("/Views/Mapa.xaml", UriKind.Relative));
                return 0;
            };
            _getColectivoMapService.GetColectivo(item.Id);
            
            //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
            list.SelectedIndex = -1;
        }

        private void ButtonComentarios_Click(object sender, EventArgs eventArgs)
        {
            NavigationService.Navigate(new Uri("/Views/Comments.xaml", UriKind.Relative));
        }

        private void ButtonEmpezar_OnClick(object sender, RoutedEventArgs e)
        {
            StartPanel.Visibility = Visibility.Collapsed;
            PivotControl.SelectedIndex = 0;
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void RateReview_Click(object sender, EventArgs e)
        {
            ShowReviewTask();
        }

        private static void ShowReviewTask()
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void ButtonSwap_OnClick(object sender, RoutedEventArgs e)
        {
            if (_origen == null && _destino == null) return;

            if (_origen == null || _destino == null)
            {
                if (_origen == null)
                {
                    _origen = new GeoCoordinate(_destino.Latitude, _destino.Longitude);
                    _destino = null;
                    TxtOrigen.Text = TxtDestino.Text;
                    TxtDestino.Text = "Seleccione destino";
                    return;
                }
                if (_destino == null)
                {
                    _destino = new GeoCoordinate(_origen.Latitude, _origen.Longitude);
                    _origen = null;
                    TxtDestino.Text = TxtOrigen.Text;
                    TxtOrigen.Text = "Seleccione origen";
                    return;
                }
            }

            var aux = new GeoCoordinate(_origen.Latitude, _origen.Longitude);
            _origen = new GeoCoordinate(_destino.Latitude, _destino.Longitude);
            _destino = aux;

            var auxN = TxtOrigen.Text;
            TxtOrigen.Text = TxtDestino.Text;
            TxtDestino.Text = auxN;

            BtnBuscar.IsEnabled = true;
        }
    }

    public class RuteBusquedaViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GeocoderResult> _busquedaOrigen;
        private ObservableCollection<GeocoderResult> _busquedaDestino;
        private ObservableCollection<ColectivoItemViewModel> _busquedaResults;

        public RuteBusquedaViewModel()
        {
            BusquedaOrigen = new ObservableCollection<GeocoderResult>();
            BusquedaDestino = new ObservableCollection<GeocoderResult>();
            BusquedaResultados = new ObservableCollection<ColectivoItemViewModel>();
        }

        public ObservableCollection<GeocoderResult> BusquedaOrigen
        {
            get { return _busquedaOrigen; }
            set
            {
                _busquedaOrigen = value;
                NotifyPropertyChanged("BusquedaOrigen");
            }
        }

        public ObservableCollection<GeocoderResult> BusquedaDestino
        {
            get { return _busquedaDestino; }
            set
            {
                _busquedaDestino = value;
                NotifyPropertyChanged("BusquedaDestino");
            }
        }

        public ObservableCollection<ColectivoItemViewModel> BusquedaResultados
        {
            get { return _busquedaResults; }
            set
            {
                _busquedaResults = value;
                NotifyPropertyChanged("BusquedaResultados");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class GeocoderResult
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Nombre { get; set; }
        public string Detalles { get; set; }
    }
}