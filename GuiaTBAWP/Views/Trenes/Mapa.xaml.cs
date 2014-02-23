using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using GuiaTBAWP.BusData;
using GuiaTBAWP.Commons.Data;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Mapa : PhoneApplicationPage
    {
        MapLayer _posicionActual = new MapLayer();
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
            MostrarLugares(_lineaTren);
            MiMapa.Layers.Add(_posicionActual);
        }

        private void ActualizarUbicacion(GeoPosition<GeoCoordinate> location)
        {
            _posicionActual.Clear();
            if (location == null || location.Location.IsUnknown) return;

            _posicionActual.Add(new MapOverlay
            {
                GeoCoordinate = location.Location,
                //TODO: template
                //ContentTemplate = (ControlTemplate) (Application.Current.Resources["locationPushpinTemplate"]),
            });
        }

        void MostrarLugares(string linea)
        {
            //TODO: Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            //MiMapa.Children.Clear();

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
                    //TODO: name
                    //Name = line.Nombre,
                    Path = new GeoCoordinateCollection(),
                    StrokeColor = line.Color,
                    //TODO: opacity
                    //Opacity = 0.8,
                    StrokeThickness = 5.0,
                };

                foreach (var location in line.Trazado)
                {
                    routeLine.Path.Add(new GeoCoordinate(location.X, location.Y));
                }

                MiMapa.MapElements.Add(routeLine);

                //estaciones
                var estacionesLayer = new MapLayer();
                foreach (var ml in line.Postas)
                {
                    var nuevoLugar = new MapOverlay()
                    {
                        Content = ml.Name,
                        GeoCoordinate = new GeoCoordinate(ml.X, ml.Y),
                        //TODO: visibility
                        //Visibility = Visibility.Collapsed,
                    };
                    estacionesLayer.Add(nuevoLugar);
                }
                MiMapa.Layers.Add(estacionesLayer);
            }
            
            MiMapa.SetView(MiMapa.CreateBoundingRectangle());

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
            //var item = (CheckBox)sender;
            //if (item.Content.Equals("Estaciones"))
            //{
            //    foreach (var child in MiMapa.Children.OfType<Pushpin>().Where(x => x.Content != null))
            //    {
            //        child.Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            //    }
            //}
            //else
            //{
            //    foreach (var child in MiMapa.Children.OfType<MapPolyline>())
            //    {
            //        child.Visibility = item.IsChecked != null && item.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            //    }
            //}
        }
    }
}