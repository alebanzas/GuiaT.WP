using System.Collections.Generic;
using System.Linq;
using System.Windows.Shapes;
using GuiaTBAWP.Commons.Models;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;

namespace GuiaTBAWP
{
    public class MapViewModel
    {
        public MapViewModel()
        {
            Pushpins = new Dictionary<MapReference, List<MapOverlay>>();
            Lines = new Dictionary<MapReference, List<MapPolyline>>();
        }

        public List<MapReference> ReferencesList
        {
            get
            {
                return Pushpins.Select(x=> x.Key).Union(Lines.Select(x => x.Key)).Distinct().ToList();
            }
        }

        public Dictionary<MapReference, List<MapOverlay>> Pushpins { get; set; }
        public Dictionary<MapReference, List<MapPolyline>> Lines { get; set; }

        public void AddElement(MapReference key, MapPolyline element)
        {
            if (Lines.ContainsKey(key))
            {
                Lines[key].Add(element);
            }
            else
            {
                Lines.Add(key, new List<MapPolyline>{element});
            }
        }

        public void AddElement(MapReference key, MapOverlay element)
        {
            if (Pushpins.ContainsKey(key))
            {
                Pushpins[key].Add(element);
            }
            else
            {
                Pushpins.Add(key, new List<MapOverlay> { element });
            }
        }

        public void Reset()
        {
            Pushpins.Clear();
            Lines.Clear();
        }
    }
}