using System;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views.Aviones.Aeroparque
{
    public partial class ComoLlegar : PhoneApplicationPage
    {
        public ComoLlegar()
        {
            InitializeComponent();

            var posicion = new GeoCoordinate(-34.5584560886206, -58.4167098999023);
            var pushpin = new Pushpin
            {
                Location = posicion,
                Template = (ControlTemplate)(App.Current.Resources["locationPushpinTemplate"])
            };

            Mapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);
            Mapa.Center = posicion;
            Mapa.ZoomLevel = 14;
            Mapa.Children.Add(pushpin);

            if (App.Configuration.IsLocationEnabled && PositionService.GetCurrentLocation().Location != null)
            {
                pushpin = new Pushpin
                {
                    Location = PositionService.GetCurrentLocation().Location,
                    Template = (ControlTemplate)(App.Current.Resources["locationPushpinTemplate"])
                };
                Mapa.Children.Add(pushpin);

                var x = from l in Mapa.Children
                        select (l as Pushpin).Location;
                Mapa.SetView(LocationRect.CreateLocationRect(x));
            }
        }

        private void SwitchView(object sender, EventArgs e)
        {
            Mapa.Mode = (Mapa.Mode is RoadMode) ? (MapMode)new AerialMode() : new RoadMode();
        }

        private void Phone_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask { PhoneNumber = "+54 11 5480 6111" };
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
                    new LabeledMapLocation("Aeroparque Internacional \"Jorge Newbery\"",
                        new GeoCoordinate(-34.5584560886206, -58.4167098999023))
            };
            bingMapsDirectionsTask.Show();
        }
    }
}