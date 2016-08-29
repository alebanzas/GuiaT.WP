using System;
using System.Data.Linq.Mapping;
using System.Device.Location;

namespace GuiaTBA.Common.Models
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

        private int _cantidadEspacios;
        [Column]
        public int CantidadEspacios
        {
            get { return _cantidadEspacios; }
            set { _cantidadEspacios = value; }
        }

        private int _externalId;
        [Column]
        public int ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
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

        public BicicletaEstacionTable(Guid id, string nombre, double longitud, double latitud, string estado, int cantidad, int cantidadespacios, string horario, string url, string distance, int externalId)
        {
            Id = id; 
            Nombre = nombre;
            Longitud = longitud;
            Latitud = latitud;
            Estado = estado;
            Cantidad = cantidad;
            CantidadEspacios = cantidadespacios;
            Horario = horario;
            Url = url;
            Distance = distance;
            ExternalId = externalId;
        }

        public bool Equals(BicicletaEstacionTable other)
        {
            return Nombre.Equals(other.Nombre) || ExternalId.Equals(other.ExternalId);
        }

        public double GetDistanceTo(GeoPosition<GeoCoordinate> getCurrentLocation)
        {
            var point = getCurrentLocation.Location;
            return ((Latitud - point.Latitude)*(Latitud - point.Latitude)) +
                   ((Longitud - point.Longitude)*(Longitud - point.Longitude));
        }
    }
    
}
