﻿using System.Collections.Generic;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;

namespace GuiaTBAWP.Models
{
    /// <summary>
    /// Represents the route data model.
    /// </summary>
    public class RouteModel
    {
        private readonly LocationCollection _locations;

        /// <summary>
        /// Gets the location collection of this route.
        /// </summary>
        public ICollection<GeoCoordinate> Locations
        {
            get { return _locations; }
        }

        /// <summary>
        /// Initializes a new instance of this type.
        /// </summary>
        /// <param name="locations">A collection of locations.</param>
        public RouteModel(IEnumerable<Location> locations)
        {
            _locations = new LocationCollection();
            foreach (var location in locations)
            {
                _locations.Add(location);
            }
        }
    }
}
