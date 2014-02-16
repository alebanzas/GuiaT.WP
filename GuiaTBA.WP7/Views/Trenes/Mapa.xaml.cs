using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using GuiaTBA.WP7.BusData;
using GuiaTBAWP.Commons.Data;
using GuiaTBAWP.Commons.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace GuiaTBA.WP7.Views.Trenes
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin _posicionActual;
        private string _lineaTren;

        public Mapa()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _lineaTren = Uri.EscapeUriString(NavigationContext.QueryString["linea"]);
            
            base.OnNavigatedTo(e);
        }
        

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MiMapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);
            MostrarLugares(_lineaTren);
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

        void MostrarLugares(string linea)
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

            //ReferencesListBox.ItemsSource = DataTren.GetTrenesList(linea);
            ReferencesListBox.ItemsSource = new List<MapReference>
            {
                new MapReference { Id = 1, Nombre = "Trazado", Checked = true},
                new MapReference { Id = 2, Nombre = "Estaciones", Checked = false},
            };

            var lineas = DataTren.GetData(_lineaTren);

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
                        Visibility = Visibility.Collapsed,
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