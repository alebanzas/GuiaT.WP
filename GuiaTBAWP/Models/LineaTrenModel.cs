using System.Collections.Generic;

namespace GuiaTBAWP.Models
{
    public class LineaTrenModel
    {
        public string Nombre { get; set; }

        /// <summary>
        /// TODO: el peor de los estados de sus ramales
        /// </summary>
        public string Estado { get; set; }

        public List<RamalTrenModel> Ramales { get; set; }
    }
}