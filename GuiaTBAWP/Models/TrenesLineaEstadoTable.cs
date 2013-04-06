using System;
using System.Data.Linq.Mapping;

namespace GuiaTBAWP.Models
{
    [Table]
    public class TrenesLineaEstadoTable : IEquatable<TrenesLineaEstadoTable>
    {
        [Column]
        public Guid Id { get; set; }

        [Column(IsPrimaryKey = true)]
        public string Nombre { get; set; }

        [Column]
        public string NickName { get; set; }

        [Column]
        public string Estado { get; set; }
        
        public TrenesLineaEstadoTable()
        {
        }

        public TrenesLineaEstadoTable(Guid id, string nombre, string nickName, string estado)
        {
            Id = id;
            Nombre = nombre;
            NickName = nickName;
            Estado = estado;
        }

        public bool Equals(TrenesLineaEstadoTable other)
        {
            return Nombre.Equals(other.Nombre);
        }
    }
    
}
