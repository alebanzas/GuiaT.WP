using System;
using System.Data.Linq.Mapping;
using System.Device.Location;

namespace GuiaTBAWP.Commons.Models
{
    [Table]
    public class BicicletaEstacionTable : IEquatable<BicicletaEstacionTable>
    {
        private Guid id;
        [Column]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;
        [Column(IsPrimaryKey = true)]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private double longitud;
        [Column]
        public double Longitud
        {
            get { return longitud; }
            set { longitud = value; }
        }

        private double latitud;
        [Column]
        public double Latitud
        {
            get { return latitud; }
            set { latitud = value; }
        }

        private string _estado;
        [Column]
        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        private int _cantidad;
        [Column]
        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        private string _horario;
        [Column]
        public string Horario
        {
            get { return _horario; }
            set
            {
                if(!string.IsNullOrEmpty(value))
                    _horario = value;
            }
        }

        private string url;
        [Column]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string distance;
        [Column]
        public string Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public string Descripcion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Distance))
                {
                    return string.Format("Cantidad disponible: {0}", Cantidad);
                }
                return string.Concat(string.Format("Cantidad disponible: {0}", Cantidad), " | (a ", Distance, ")");
            }
        }

        public BicicletaEstacionTable()
        {
        }

        public BicicletaEstacionTable(Guid id, string nombre, double longitud, double latitud, string estado, int cantidad, string horario, string url, string distance)
        {
            this.Id = id; 
            this.Nombre = nombre;
            this.Longitud = longitud;
            this.Latitud = latitud;
            this.Estado = estado;
            this.Cantidad = cantidad;
            this.Horario = horario;
            this.Url = url;
            this.Distance = distance;
        }

        public bool Equals(BicicletaEstacionTable other)
        {
            return this.Nombre.Equals(other.Nombre);
        }

        public double GetDistanceTo(GeoPosition<GeoCoordinate> getCurrentLocation)
        {
            var point = getCurrentLocation.Location;
            return ((Latitud - point.Latitude)*(Latitud - point.Latitude)) +
                   ((Longitud - point.Longitude)*(Longitud - point.Longitude));
        }
    }
    
}
