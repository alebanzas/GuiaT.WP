using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using Microsoft.Phone.Maps.Controls;

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

            return LocationRectangle.CreateBoundingRectangle(geoCoordinates);
        }
    }
}
