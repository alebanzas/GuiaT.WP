using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiaTBAWP.Models
{
    public static class LineaTrenModelExtensions
    {
        public static IEnumerable<TrenesRamalEstadoTable> ConvertToTrenesRamalEstadoTable(this IList<RamalTrenModel> ramales, string linea)
        {
            return ramales.Select(x => ConvertToTrenesRamalEstadoTable(x, linea));
        }

        public static TrenesRamalEstadoTable ConvertToTrenesRamalEstadoTable(this RamalTrenModel ramal, string linea)
        {
            return new TrenesRamalEstadoTable
                            {
                                Id = Guid.NewGuid(),
                                Nombre = ramal.Nombre,
                                Estado = ramal.Estado,
                                LineaNickName = linea,
                                MasInfo = ramal.MasInfo,
                            };
        }
    }

    public class LineaTrenModel
    {
        public string Nombre { get; set; }

        /// <summary>
        /// TODO: el peor de los estados de sus ramales
        /// </summary>
        public string Estado { get; set; }

        public List<RamalTrenModel> Ramales { get; set; }
    }
}