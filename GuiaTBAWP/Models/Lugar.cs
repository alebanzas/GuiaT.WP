using System.Data.Linq.Mapping;


namespace WPLugares
{
    [Table]
    public class Lugar
    {
        private int id;
        [Column(IsPrimaryKey = true)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;
        [Column]
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

        private string imagen1;
        [Column]
        public string Imagen1
        {
            get { return imagen1; }
            set { imagen1 = value; }
        }

        private string imagen2;
        [Column]
        public string Imagen2
        {
            get { return imagen2; }
            set { imagen2 = value; }
        }

        private string descripcion;
        [Column]
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private string url;
        [Column]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public Lugar()
        {
        }

        public Lugar(int id, string nombre, double longitud, double latitud, string imagen1, string imagen2, string descripcion, string url)
        {
            this.Id = id; 
            this.Nombre = nombre;
            this.Longitud = longitud;
            this.Latitud = latitud;
            this.Imagen1 = imagen1;
            this.Imagen2 = imagen2;
            this.Descripcion = descripcion;
            this.Url = url;
        }

    }
    
}
