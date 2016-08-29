using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GuiaTBA.Common.Models;
using GuiaTBAWP.BusData;
using GuiaTBAWP.Commons.Data;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using MapExtensions = Microsoft.Phone.Maps.Toolkit.MapExtensions;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class Mapa : PhoneApplicationPage
    {
        MapLayer _posicionActualLayer = new MapLayer();
        MapLayer _puntosLayer = new MapLayer();
        public ObservableCollection<BicicletaEstacionTable> Estaciones;

        public Mapa()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MiMapa.Layers.Add(_posicionActualLayer);
            MiMapa.Layers.Add(_puntosLayer);
            MostrarLugares();
        }
        
        private void ActualizarUbicacion(GeoPosition<GeoCoordinate> location)
        {
            _posicionActualLayer.Clear();
            if (location == null || location.Location.IsUnknown) return;

            var posicionActual = new MapOverlay
            {
                GeoCoordinate = location.Location,
                ContentTemplate = App.Current.Resources["locationPushpinTemplate"] as DataTemplate,
            };
            _posicionActualLayer.Add(posicionActual);
        }

        void MostrarLugares()
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            _puntosLayer.Clear();

            ReferencesListBox.ItemsSource = new List<MapReference>
            {
                new MapReference { Id = 1, Nombre = "Ciclovias", Checked = true},
                new MapReference { Id = 2, Nombre = "Estaciones", Checked = false},
                //new MapReference { Id = 3, Nombre = "Estacionamientos", Checked = false},
            };

            RenderBikeRoads();

            var query = from miLugar in BicicletaEstacionDC.Current.Estaciones
                        orderby miLugar.Id
                        select miLugar;

            Estaciones = new ObservableCollection<BicicletaEstacionTable>(query.ToList());

            foreach (var ml in Estaciones)
            {
                Pushpin nuevoLugar = new Pushpin
                {
                    Content = ml.Nombre,
                    GeoCoordinate = new GeoCoordinate(ml.Latitud, ml.Longitud),
                    Visibility = Visibility.Collapsed,
                };
                nuevoLugar.MouseLeftButtonUp += NuevoLugar_MouseLeftPuttonUp;
                MapExtensions.GetChildren(MiMapa).Add(nuevoLugar);
            }
            MiMapa.SetView(MiMapa.CreateBoundingRectangle());

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? App.Configuration.Ubicacion : null);
        }

        private void RenderBikeRoads()
        {
            var lineas = DataBicicletas.GetData();

            var strokeColor = Colors.Blue;

            foreach (var line in lineas)
            {
                var routeLine = new MapPolyline()
                {
                    Path = new GeoCoordinateCollection(),
                    StrokeColor = strokeColor,
                    StrokeThickness = 5.0,
                };

                foreach (var location in line.Trazado)
                {
                    routeLine.Path.Add(new GeoCoordinate(location.X, location.Y));
                }

                MiMapa.MapElements.Add(routeLine);
            }
        }

        private void NuevoLugar_MouseLeftPuttonUp(object sender, MouseButtonEventArgs e)
        {
            //TODO
            //var pin = sender as Pushpin;
            //
            //if (pin == null) return;
            //
            //var query = from l in BicicletaEstacionDC.Current.Estaciones
            //    where l.Latitud == pin.Location.Latitude
            //          && l.Longitud == pin.Location.Longitude
            //          && l.Nombre == pin.Content.ToString()
            //    select l;
            //
            //var listaLugares = query.ToList();
            //var bicicletaEstacion = listaLugares.FirstOrDefault();
            //var uri = new Uri(String.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
            //NavigationService.Navigate(uri);
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
            MiMapa.CartographicMode = (MiMapa.CartographicMode.Equals(MapCartographicMode.Road)) ? MapCartographicMode.Hybrid : MapCartographicMode.Road;
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void BtnReferencias_Click(object sender, EventArgs e)
        {
            Results.Visibility = Results.Visibility.Equals(Visibility.Collapsed)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void References_OnChecked(object sender, RoutedEventArgs e)
        {
            //TODO:
            var item = (CheckBox)sender;
            if (item.Content.Equals("Estaciones"))
            {
                foreach (var child in MapExtensions.GetChildren(MiMapa).OfType<Pushpin>().Where(x => x.Content != null))
                {
                    child.Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else
            {
                //foreach (var child in MapExtensions.GetChildren(MiMapa).OfType<MapPolyline>())
                //{
                //    child..Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
                //}
            }
        }
    }
}