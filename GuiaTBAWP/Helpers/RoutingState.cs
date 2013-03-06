using System.Linq;
using GuiaTBAWP.Bing.Geocode;

namespace GuiaTBAWP.Helpers
{
    /// <summary>
    /// Internally used for passing state between route asynchronous calls.
    /// </summary>
    internal class RoutingState
    {
        internal RoutingState(GeocodeResult[] resultArray, int index, string tb)
        {
            Results = resultArray;
            LocationNumber = index;
            Output = tb;
        }

        internal bool GeocodesComplete
        {
            get
            {
                return Results.All(t => null != t);
            }
        }

        internal bool GeocodesSuccessful
        {
            get
            {
                return Results.All(t => null != t && null != t.Locations && 0 != t.Locations.Count);
            }
        }

        internal GeocodeResult[] Results { get; set; }
        internal int LocationNumber { get; set; }
        internal string Output { get; set; }
    }
}
