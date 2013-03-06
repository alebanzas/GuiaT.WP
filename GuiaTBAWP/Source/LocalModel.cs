using System.Windows;

namespace GuiaTBAWP.Source
{
    public class LocalModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Direccion1 { get; set; }

        public string Direccion2 { get; set; }

        public Point Ubicacion { get; set; }

        public string Telefonos { get; set; }

        public string CodigoPostal { get; set; }

        public string Horarios { get; set; }

        public double Latitud
        {
            get { return Ubicacion.X; }
        }

        public double Longitud
        {
            get { return Ubicacion.Y; }
        }
    }
}