using System;
using System.Data.Linq.Mapping;

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
    
}
