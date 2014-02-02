using System;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace GuiaTBAWP.Views.Subtes
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin _posicionActual;

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
                Template = Application.Current.Resources["locationPushpinTemplate"] as ControlTemplate,
            };
            MiMapa.Children.Add(_posicionActual);
        }

        void MostrarLugares()
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

            var lineas = DataSubte.GetData();

            foreach (var line in lineas)
            {
                //linea
                var routeLine = new MapPolyline
                {
                    Name = line.Nombre,
                    Locations = new LocationCollection(),
                    Stroke = new SolidColorBrush(line.Color),
                    Opacity = 0.8,
                    StrokeThickness = 5.0,
                };

                foreach (var location in line.Trazado)
                {
                    routeLine.Locations.Add(new GeoCoordinate(location.X, location.Y));
                }

                MiMapa.Children.Add(routeLine);

                //estaciones
                foreach (var ml in line.Postas)
                {

                    var nuevoLugar = new Pushpin
                    {
                        Content = ml.Name,
                        Location = new GeoCoordinate(ml.X, ml.Y),
                    };

                    MiMapa.Children.Add(nuevoLugar);
                }
            }

            //Ajusto el mapa para mostrar los items
            var x = from l in MiMapa.Children let pushpin = l as Pushpin where pushpin != null && pushpin.Location != null select pushpin.Location;

            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? App.Configuration.Ubicacion : null);
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
    }
}