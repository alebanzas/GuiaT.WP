using System;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GuiaTBAWP.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;

namespace GuiaTBAWP.Views
{
    public partial class Mapa : PhoneApplicationPage
    {
        MapLayer _posicionActualLayer = new MapLayer();
        MapLayer _puntosLayer = new MapLayer();
        
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
                ContentTemplate = Application.Current.Resources["locationPushpinTemplate"] as DataTemplate,
            };
            _posicionActualLayer.Add(posicionActual);
        }

        void MostrarLugares()
        {
            MiMapa.MapElements.Clear();
            _puntosLayer.Clear();
            _posicionActualLayer.Clear();
            ReferencesListBox.ItemsSource = App.MapViewModel.ReferencesList;

            foreach (var mapPolyline in App.MapViewModel.Lines.Where(x => x.Key.Checked).Select(x => x.Value).SelectMany(line => line))
            {
                MiMapa.MapElements.Add(mapPolyline);
            }
            foreach (var mapOverlay in App.MapViewModel.Pushpins.Where(x => x.Key.Checked).Select(x => x.Value).SelectMany(pushpin => pushpin))
            {
                mapOverlay.ContentTemplate = Application.Current.Resources["Pushpin"] as DataTemplate;
                mapOverlay.PositionOrigin = new Point(0,1);
                _puntosLayer.Add(mapOverlay);
            }

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? App.Configuration.Ubicacion : null);

            MiMapa.SetView(MiMapa.CreateBoundingRectangle());
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
            var item = (CheckBox)sender;
            var reference = App.MapViewModel.ReferencesList.FirstOrDefault(x => x.Nombre.Equals(item.Content));
            if (item.IsChecked != null) if (reference != null) reference.Checked = item.IsChecked.Value;
            MostrarLugares();
        }
    }
}