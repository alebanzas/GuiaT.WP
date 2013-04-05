using System;
using System.Collections.Generic;

namespace GuiaTBAWP.Models
{
    public class TrenesStatusModel
    {
        public TrenesStatusModel()
        {
            Lineas = new List<LineaTrenModel>();
        }

        public IList<LineaTrenModel> Lineas { get; set; }

        public DateTime Actualizacion { get; set; }

        public string ActualizacionStr
        {
            get { return string.Format("{0} {1}", Actualizacion.ToLongDateString(), Actualizacion.ToLongTimeString()); }
        }
    }
}