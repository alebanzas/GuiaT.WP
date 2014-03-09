using System;
using System.Collections.Generic;

namespace GuiaTBA.Common.Models
{
    public class LiveTrenModel
    {
        public LiveTrenModel()
        {
            Estaciones = new List<LiveTrenItemModel>();
        }

        public IList<LiveTrenItemModel> Estaciones { get; set; }

        public DateTime Actualizacion { get; set; }

        public string ActualizacionStr
        {
            get { return string.Format("{0} {1}", Actualizacion.ToLongDateString(), Actualizacion.ToLongTimeString()); }
        }
    }

    public class LiveTrenItemModel
    {
        public string Nombre { get; set; }

        public int? Ida1 { get; set; }
        public int? Ida2 { get; set; }
        public int? Vuelta1 { get; set; }
        public int? Vuelta2 { get; set; }
    }
}