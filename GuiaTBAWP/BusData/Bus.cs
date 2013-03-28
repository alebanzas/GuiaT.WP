namespace GuiaTBAWP.BusData
{
    public class Bus
    {
        public Bus(string category)
        {
            Category = category;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; private set; }
    }
}
