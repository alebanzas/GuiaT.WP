using System;
using System.Device.Location;
using System.Windows;
using GuiaTBAWP.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views.Aviones.Ezeiza
{
    public partial class ComoLlegar : PhoneApplicationPage
    {
        public ComoLlegar()
        {
            InitializeComponent();

            var markerLayer = new MapLayer();
            var posicion = new GeoCoordinate(-34.812393945083, -58.5363578796387);
            var pushpin = new MapOverlay
            {
                GeoCoordinate = posicion,
                //TODO: template
                //ContentTemplate = (ControlTemplate)(App.Current.Resources["locationPushpinTemplate"])
            };
            markerLayer.Add(pushpin);
            
            Mapa.Layers.Add(markerLayer);
            Mapa.Center = posicion;
            Mapa.ZoomLevel = 14;

            if (App.Configuration.IsLocationEnabled && PositionService.GetCurrentLocation().Location != null)
            {
                pushpin = new MapOverlay
                {
                    GeoCoordinate = PositionService.GetCurrentLocation().Location,
                    //TODO: template
                    //ContentTemplate = (ControlTemplate)(App.Current.Resources["locationPushpinTemplate"])
                };
                markerLayer.Add(pushpin);

                Mapa.SetView(Mapa.CreateBoundingRectangle());
            }
        }

        private void SwitchView(object sender, EventArgs e)
        {
            Mapa.CartographicMode = (Mapa.CartographicMode.Equals(MapCartographicMode.Road)) ? MapCartographicMode.Hybrid : MapCartographicMode.Road;
        }

        private void Phone_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask { PhoneNumber = "+54 11 5480 2500" };
            task.Show();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Directions(object sender, EventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation("Aeropuerto Internacional de Ezeiza \"Ministro Pistarini\"",
                        new GeoCoordinate(-34.812393945083, -58.5363578796387))
            };
            bingMapsDirectionsTask.Show();
        }
    }
}