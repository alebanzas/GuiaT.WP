using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiaTBAWP.BusData
{
    public class Bus
    {
        private Bus() { }

        public Bus(string category)
        {
            this.Category = category;
        }

        public static readonly string[] Categories = { "Action", "Romance", "Thrillers", "Comedy", "Documentaries", "Drama" };
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; private set; }
    }
}
