using System;
using System.Device.Location;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class AtencionAlUsuario : PhoneApplicationPage
    {
        public AtencionAlUsuario()
        {
            InitializeComponent();
        }

        private void Hyperlink_Belgrano_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Belgrano",
                        new GeoCoordinate(-34.564089, -58.455318))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Caballito_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Caballito",
                        new GeoCoordinate(-34.610857, -58.435512))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Congreso_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Congreso",
                        new GeoCoordinate(-34.605462, -58.3922))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Constitucion_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Contitución",
                        new GeoCoordinate(-34.630921, -58.382673))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Liniers_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Liniers",
                        new GeoCoordinate(-34.637145, -58.527989))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_SIGEN_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - SIGEN",
                        new GeoCoordinate(-34.637145, -58.527989))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Tigre_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Tigre",
                        new GeoCoordinate(-34.455204, -58.634865))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_SanMartin_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - San Martín",
                        new GeoCoordinate(-34.536806, -58.577809))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_SanMiguel_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - San Miguel",
                        new GeoCoordinate(-34.543469, -58.712348))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_MalvinasArgentinas_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Malvinas Argentinas",
                        new GeoCoordinate(-34.497879, -58.695904))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Ciudadela_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Ciudadela",
                        new GeoCoordinate(-34.640274, -58.541481))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Caseros_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Caseros",
                        new GeoCoordinate(-34.603285, -58.564414))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Ituzaingo_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Ituzaingó",
                        new GeoCoordinate(-34.659171, -58.66681))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Moreno_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Moreno",
                        new GeoCoordinate(-34.649511, -58.788502))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Moron_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Morón",
                        new GeoCoordinate(-34.650403, -58.621867))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_GonzalezCatan_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Gonzalez Catán",
                        new GeoCoordinate(-34.79333, -58.632054))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_LaPlata_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - La Plata",
                        new GeoCoordinate(-34.911458, -57.95567))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Ensenada_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Ensenada",
                        new GeoCoordinate(-34.856984, -57.907568))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Temperley_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Temperley",
                        new GeoCoordinate(-34.774918, -58.397951))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Ezeiza_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Ezeiza",
                        new GeoCoordinate(-34.891486, -58.569890))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Avellaneda_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Avellaneda",
                        new GeoCoordinate(-34.677776, -58.36695))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_RafaelCalzada_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Rafael Calzada",
                        new GeoCoordinate(-34.677776, -58.36695))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_Lanus_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Lanús",
                        new GeoCoordinate(-34.699623, -58.392168))
            };
            bingMapsDirectionsTask.Show();
        }

        private void Hyperlink_AlmiranteBrown_OnClick(object sender, RoutedEventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
            {
                End =
                    new LabeledMapLocation(
                        "SUBE - Almirante Brown",
                        new GeoCoordinate(-34.802598, -58.401883))
            };
            bingMapsDirectionsTask.Show();
        }
    }
}