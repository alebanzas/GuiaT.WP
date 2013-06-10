namespace GuiaTBAWP.BusData
{
    public class Bus
    {
        private string _description;

        public Bus(string category)
        {
            Category = category;
        }

        public string Title { get; set; }
        public string Description
        {
            get { return _description.ToLowerInvariant(); }
            set { _description = value; }
        }

        public string Category { get; private set; }

        public string Tipo { get; set; }
    }
}
