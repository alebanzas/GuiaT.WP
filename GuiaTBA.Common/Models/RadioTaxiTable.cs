using System;
using System.Data.Linq.Mapping;

namespace GuiaTBAWP.Commons.Models
{
    [Table]
    public class RadioTaxiTable : IEquatable<RadioTaxiTable>
    {
        private Guid _id;
        [Column]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _nombre;
        [Column(IsPrimaryKey = true)]
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        private string _telefono;
        [Column]
        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        private string _detalles;
        [Column]
        public string Detalles
        {
            get { return _detalles; }
            set
            {
                if(!string.IsNullOrEmpty(value))
                    _detalles = value;
            }
        }

        private string _url;
        [Column]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public RadioTaxiTable()
        {
        }

        public RadioTaxiTable(Guid id, string nombre, string telefono, string detalles, string url)
        {
            Id = id; 
            Nombre = nombre;
            Telefono = telefono;
            Detalles = detalles;
            Url = url;
        }

        public bool Equals(RadioTaxiTable other)
        {
            return Nombre.Equals(other.Nombre);
        }
    }
    
}
