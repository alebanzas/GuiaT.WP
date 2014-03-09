using System.Device.Location;
using System.Linq;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;

namespace GuiaTBAWP.Extensions
{
    public static class MapExtensions
    {
        public static LocationRectangle CreateBoundingRectangle(this Map map)
        {
            var geoCoordinates = (from layer in map.Layers from mapo in layer where mapo.GeoCoordinate != null select mapo.GeoCoordinate).ToList();

            foreach (var mapElement in map.MapElements)
            {
                if (mapElement as MapPolyline != null)
                {
                    var element = mapElement as MapPolyline;
                    geoCoordinates.AddRange(element.Path);
                }
                if (mapElement as MapPolygon != null)
                {
                    var element = mapElement as MapPolygon;
                    geoCoordinates.AddRange(element.Path);
                }
            }

            geoCoordinates.AddRange(from dependencyObject in Microsoft.Phone.Maps.Toolkit.MapExtensions.GetChildren(map) where dependencyObject as Pushpin != null select (dependencyObject as Pushpin).GeoCoordinate);

            if (!geoCoordinates.Any())
            {
                geoCoordinates.Add(new GeoCoordinate(-34.77828, -58.152802));
                geoCoordinates.Add(new GeoCoordinate(-34.498069, -58.726151));
            }

            return LocationRectangle.CreateBoundingRectangle(geoCoordinates);
        }
    }
}
