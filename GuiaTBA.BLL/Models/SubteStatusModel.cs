using System;
using System.Collections.Generic;

namespace GuiaTBA.BLL.Models
{
    public class SubteStatusModel
    {
        public SubteStatusModel()
        {
            Lineas = new List<SubteStatusItem>();
        }

        public List<SubteStatusItem> Lineas { get; set; }

        public DateTime Actualizacion { get; set; }

        public string ActualizacionStr { get; set; }
    }
}