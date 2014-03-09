using System.Device.Location;
using GuiaTBAWP.Commons.Data;
using GuiaTBAWP.Commons.Models;
using Microsoft.Phone.Maps.Controls;

namespace GuiaTBAWP.Views.Trenes
{
    public static class TrenMapModel
    {
        public static void SetMapModel(string linea)
        {
            App.MapViewModel.Reset();

            var lineas = DataTren.GetData(linea);

            var trazadoMapReference = new MapReference {Id = 1, Nombre = "Trazado", Checked = true};
            var estacionesMapReference = new MapReference {Id = 2, Nombre = "Estaciones", Checked = false};

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
                App.MapViewModel.AddElement(trazadoMapReference, routeLine);

                //estaciones
                foreach (var ml in line.Postas)
                {
                    var nuevoLugar = new MapOverlay
                    {
                        Content = ml.Name,
                        GeoCoordinate = new GeoCoordinate(ml.X, ml.Y),
                    };
                    App.MapViewModel.AddElement(estacionesMapReference, nuevoLugar);
                }
            }
        }
    }
}