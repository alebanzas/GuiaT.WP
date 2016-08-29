using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiaTBA.Common.Models
{
    public static class BicicletasStatusModelExtensions
    {
        public static IEnumerable<BicicletaEstacionTable> ConvertToBicicletaEstacionTable(this IList<BicicletaEstacion> estaciones)
        {
            return estaciones.Select(x => new BicicletaEstacionTable
                {
                    Id = Guid.NewGuid(),
                    Horario = x.Horario,
                    Latitud = x.Latitud,
                    Longitud = x.Longitud,
                    Nombre = x.Nombre,
                    Estado = x.Estado, 
                    Cantidad = x.Cantidad,
                    CantidadEspacios = x.CantidadEspacios,
                    ExternalId = x.ExternalId,
                });
        }
    }


    public class BicicletasStatusModel
    {
        public BicicletasStatusModel()
        {
            Estaciones = new List<BicicletaEstacion>();
        }

        public IList<BicicletaEstacion> Estaciones { get; set; }

        public DateTime Actualizacion { get; set; }

        public string ActualizacionStr => $"{Actualizacion.ToLongDateString()} {Actualizacion.ToLongTimeString()}";
    }

    public class BicicletaEstacion
    {
        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Nombre { get; set; }

        public string Estado { get; set; }

        public string Horario { get; set; }

        public int Cantidad { get; set; }

        public int CantidadEspacios { get; set; }

        public int ExternalId { get; set; }
    }
}