namespace GuiaTBAWP.Models
{
    public class LocalServiceModel
    {
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string Barrio { get; set; }
        public virtual string Ciudad { get; set; }
        public virtual string Provincia { get; set; }
        public virtual string Sitio { get; set; }
        public virtual string Telefono { get; set; }

        public virtual string Id1 { get; set; }
        public virtual string Id2 { get; set; }
        public virtual string Id3 { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}