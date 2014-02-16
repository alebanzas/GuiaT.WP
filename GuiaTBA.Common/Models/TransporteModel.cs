using System;
using System.Collections.Generic;

namespace GuiaTBAWP.Models
{
    public class PuntoModel
    {
        public double X { get; set; }

        public double Y { get; set; }
    }

    public class TransporteModel
    {
        public Guid ID { get; set; }

        public string TipoNickName { get; set; }

        public string Nombre { get; set; }

        public string Linea { get; set; }

        public string Ramal { get; set; }

        public List<PuntoModel> Puntos { get; set; }

        public string RecorridoText { get; set; }
    }
}
