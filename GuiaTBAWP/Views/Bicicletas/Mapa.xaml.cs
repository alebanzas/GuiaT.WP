using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GuiaTBAWP.BusData;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin _posicionActual;
        public ObservableCollection<BicicletaEstacionTable> EnLugar;

        public Mapa()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MiMapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);
            MostrarLugares();
        }
        
        private void ActualizarUbicacion(GeoPosition<GeoCoordinate> location)
        {
            MiMapa.Children.Remove(_posicionActual);
            if (location == null || location.Location.IsUnknown) return;

            _posicionActual = new Pushpin
            {
                Location = location.Location,
                Template = App.Current.Resources["locationPushpinTemplate"] as ControlTemplate,
            };
            MiMapa.Children.Add(_posicionActual);
        }

        void MostrarLugares()
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

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

            EnLugar = new ObservableCollection<BicicletaEstacionTable>(query.ToList());

            foreach (var ml in EnLugar)
            {

                var nuevoLugar = new Pushpin
                {
                    Content = ml.Nombre,
                    Location = new GeoCoordinate(ml.Latitud, ml.Longitud),
                    Visibility = Visibility.Collapsed,
                };

                nuevoLugar.MouseLeftButtonUp += NuevoLugar_MouseLeftPuttonUp;
                MiMapa.Children.Add(nuevoLugar);
            }

            //Ajusto el mapa para mostrar los items
            var x = from l in MiMapa.Children let pushpin = l as Pushpin where pushpin != null && pushpin.Location != null select pushpin.Location;

            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? App.Configuration.Ubicacion : null);
        }

        private void RenderBikeRoads()
        {
            var lineas = DataBicicletas.GetData();

            var routeBrush = new SolidColorBrush(Colors.Blue);

            foreach (var line in lineas)
            {
                var routeLine = new MapPolyline
                {
                    Name = line.Nombre,
                    Locations = new LocationCollection(),
                    Stroke = routeBrush,
                    Opacity = 0.8,
                    StrokeThickness = 5.0,
                };

                foreach (var location in line.Trazado)
                {
                    routeLine.Locations.Add(new GeoCoordinate(location.X, location.Y));
                }

                MiMapa.Children.Add(routeLine);
            }
        }

        private void NuevoLugar_MouseLeftPuttonUp(object sender, MouseButtonEventArgs e)
        {
            var pin = sender as Pushpin;

            if (pin == null) return;

            var query = from l in BicicletaEstacionDC.Current.Estaciones
                where l.Latitud == pin.Location.Latitude
                      && l.Longitud == pin.Location.Longitude
                      && l.Nombre == pin.Content.ToString()
                select l;

            var listaLugares = query.ToList();
            var bicicletaEstacion = listaLugares.FirstOrDefault();
            var uri = new Uri(String.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
            NavigationService.Navigate(uri);
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

        private void BtnReferencias_Click(object sender, EventArgs e)
        {
            Results.Visibility = Results.Visibility.Equals(Visibility.Collapsed)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void References_OnChecked(object sender, RoutedEventArgs e)
        {
            var item = (CheckBox)sender;
            if (item.Content.Equals("Estaciones"))
            {
                foreach (var child in MiMapa.Children.OfType<Pushpin>().Where(x => x.Content != null))
                {
                    child.Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else
            {
                foreach (var child in MiMapa.Children.OfType<MapPolyline>())
                {
                    child.Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}