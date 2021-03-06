using System.Collections.Generic;
using System.Windows.Media;
using GuiaTBAWP.Commons.ViewModels;

namespace GuiaTBAWP.Commons.Models
{
    public class TrackLine
    {
        public TrackLine()
        {
            Trazado = new List<PuntoViewModel>();
            Postas = new List<PuntoViewModel>();
        }

        public string Nombre { get; set; }
        public IEnumerable<PuntoViewModel> Trazado { get; set; }
        public IEnumerable<PuntoViewModel> Postas { get; set; }
        public Color Color { get; set; }
    }
}