using System;
using System.Linq;
using System.Windows;
using GuiaTBAWP.Commons.Data;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using GuiaTBAWP.Commons.Models;
using System.Device.Location;
using Microsoft.Phone.Maps.Toolkit;

namespace GuiaTBAWP.Views.Subtes
{
    public partial class Subtes
    {
        public Subtes()
        {
            InitializeComponent();

            StatusChecker.Check("Subte");

            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;
        }
        
        private void Button_Click_SubteLineas(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteLineas.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteHorarios(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteHorarios.xaml", UriKind.Relative));
        }

        private void Button_Click_SubtePrecio(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubtePrecio.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteMapa(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteMapa.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteEstado(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Subtes/SubteEstado.xaml", UriKind.Relative));
        }

        private void Button_Click_SubteMapaReal(object sender, RoutedEventArgs e)
        {
            App.MapViewModel.Reset();
            var mapReferenceLines = new MapReference { Id = 1, Nombre = "Trazado", Checked = true};
            var mapReferencePushpins = new MapReference { Id = 2, Nombre = "Estaciones", Checked = false};
            var lineas = DataSubte.GetData();

            foreach (var line in lineas)
            {
                //linea
                var routeLine = new MapPolyline
                {
                    Path = new GeoCoordinateCollection(),
                    StrokeColor = line.Color,
                    StrokeThickness = 5.0,
                };
                foreach (var location in line.Trazado)
                {
                    routeLine.Path.Add(new GeoCoordinate(location.X, location.Y));
                }
                App.MapViewModel.AddElement(mapReferenceLines, routeLine);

                //estaciones
                foreach (var nuevoLugar in line.Postas.Select(ml => new MapOverlay
                {
                    Content = ml.Name,
                    GeoCoordinate = new GeoCoordinate(ml.X, ml.Y),
                }))
                {
                    App.MapViewModel.AddElement(mapReferencePushpins, nuevoLugar);
                }
            }


            NavigationService.Navigate(new Uri("/Views/Mapa.xaml", UriKind.Relative));
        }
    }
}