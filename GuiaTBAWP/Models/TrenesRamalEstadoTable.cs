using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using GuiaTBAWP.ViewModels;

namespace GuiaTBAWP.Models
{
    [Table]
    public class TrenesRamalEstadoTable : IEquatable<TrenesRamalEstadoTable>
    {
        [Column]
        public Guid Id { get; set; }

        [Column(IsPrimaryKey = true)]
        public string Nombre { get; set; }

        [Column]
        public string LineaNickName { get; set; }
        
        [Column]
        public string Estado { get; set; }

        [Column]
        public string MasInfo { get; set; }
        
        public TrenesRamalEstadoTable()
        {
        }

        public TrenesRamalEstadoTable(Guid id, string nombre, string estado, string masInfo, string lineaNickName)
        {
            Id = id; 
            Nombre = nombre;
            Estado = estado;
            MasInfo = masInfo;
            LineaNickName = lineaNickName;
        }

        public bool Equals(TrenesRamalEstadoTable other)
        {
            return Nombre.Equals(other.Nombre);
        }
    }

    public static class TrenesRamalEstadoTableExtensions
    {
        public static IEnumerable<TrenRamalItemViewModel> ConvertToTrenesRamalEstadoTable(this IList<TrenesRamalEstadoTable> ramales)
        {
            return ramales.Select(ConvertToTrenRamalItemViewModel);
        }

        public static TrenRamalItemViewModel ConvertToTrenRamalItemViewModel(this TrenesRamalEstadoTable ramal)
        {
            return new TrenRamalItemViewModel
            {
                Nombre = ramal.Nombre,
                Estado = ramal.Estado,
                MasInfo = ramal.MasInfo,
            };
        }
    }

}
