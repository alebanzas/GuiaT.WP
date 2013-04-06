using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiaTBAWP.Models
{
    public static class TrenesStatusModelExtensions
    {
        public static IEnumerable<TrenesLineaEstadoTable> ConvertToTrenesLineaEstadoTable(this IList<LineaTrenModel> lineas)
        {
            return lineas.Select(ConvertToTrenesLineaEstadoTable);
        }

        public static TrenesLineaEstadoTable ConvertToTrenesLineaEstadoTable(this LineaTrenModel linea)
        {
            return new TrenesLineaEstadoTable
                        {
                            Id = Guid.NewGuid(),
                            Nombre = linea.Nombre,
                            Estado = linea.Estado,
                            NickName = linea.Nombre.ToLowerInvariant(),
                        };
        }
    }

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