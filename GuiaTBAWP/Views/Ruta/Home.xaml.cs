using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GuiaTBAWP.Commons.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;

namespace GuiaTBAWP.Views.Ruta
{
    public partial class Home : PhoneApplicationPage
    {
        GeocodeQuery _geoQo;
        GeocodeQuery _geoQd;
        private GeoCoordinate _origen;
        private GeoCoordinate _destino;

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
                    _geoQo = new GeocodeQuery();
                    _geoQd = new GeocodeQuery();
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
            Unloaded += (sender, args) =>
                {
                    if (_geoQo != null)
                    {
                        _geoQo.CancelAsync();
                        _geoQo.Dispose();
                    }
                    if (_geoQd != null)
                    {
                        _geoQd.CancelAsync();
                        _geoQd.Dispose();
                    }
                };
            
            StatusChecker.Check("HomeRuta");
        }

        private void ButtonBuscarOrigen_OnClick(object sender, RoutedEventArgs e)
        {
            if (_geoQo.IsBusy)
            {
                _geoQo.CancelAsync();
            }

            _geoQo.QueryCompleted += geoQ_OrigenQueryCompleted;
            _geoQo.GeoCoordinate = new GeoCoordinate(-34.603577, -58.381802, 1000);
            _geoQo.SearchTerm = TxtBuscarOrigen.Text;
            _geoQo.MaxResultCount = 200;
            _geoQo.QueryAsync();
            NoResultsOrigen.Visibility = Visibility.Collapsed;
            LoadingOrigen.Visibility = Visibility.Visible;
        }

        private void ButtonBuscarDestino_OnClick(object sender, RoutedEventArgs e)
        {
            if (_geoQd.IsBusy)
            {
                _geoQd.CancelAsync();
            }

            _geoQd.GeoCoordinate = new GeoCoordinate(-34.603577, -58.381802, 1000);
            _geoQd.QueryCompleted += geoQ_DestinoQueryCompleted;
            _geoQd.SearchTerm = TxtBuscarDestino.Text;
            _geoQd.MaxResultCount = 200;
            _geoQd.QueryAsync();
            NoResultsDestino.Visibility = Visibility.Collapsed;
            LoadingDestino.Visibility = Visibility.Visible;
        }

        private void geoQ_OrigenQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            var list = e.Result.Where(x => x.Information.Address.Neighborhood.Equals("Gran Buenos Aires")).ToList();
            ViewModel.BusquedaOrigen.Clear();
            MiMapaOrigen.Layers.Clear();
            LoadingOrigen.Visibility = Visibility.Collapsed;

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
            ViewModel.BusquedaDestino.Clear();
            MiMapaDestino.Layers.Clear();
            LoadingDestino.Visibility = Visibility.Collapsed;

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

            TxtOrigen.Text = string.Format("{0}, {1}", r.Nombre.Split('.')[1], r.Detalles);

            var point = new GeoCoordinate(r.X, r.Y);
            MiMapaOrigen.Center = point;
            MiMapaOrigen.ZoomLevel = 15;
            _origen = point;

            ResultadosListOrigen.SelectedIndex = -1;
        }

        private void SelectorDestino_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultadosListDestino.SelectedIndex == -1) return;

            var r = e.AddedItems[0] as GeocoderResult;
            if (r == null) return;

            TxtDestino.Text = string.Format("{0}, {1}", r.Nombre.Split('.')[1], r.Detalles);

            var point = new GeoCoordinate(r.X, r.Y);
            MiMapaDestino.Center = point;
            MiMapaDestino.ZoomLevel = 15;
            _destino = point;
            
            ResultadosListDestino.SelectedIndex = -1;
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
            
        }
    }

    public class RuteBusquedaViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GeocoderResult> _busquedaOrigen;
        private ObservableCollection<GeocoderResult> _busquedaDestino;

        public RuteBusquedaViewModel()
        {
            BusquedaOrigen = new ObservableCollection<GeocoderResult>();
            BusquedaDestino = new ObservableCollection<GeocoderResult>();
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