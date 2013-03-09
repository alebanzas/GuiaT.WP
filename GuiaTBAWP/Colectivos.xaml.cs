using Microsoft.Phone.Controls;

namespace GuiaTBAWP
{
    public partial class Colectivos : PhoneApplicationPage
    {
        public Colectivos()
        {
            InitializeComponent();

            DataColectivos.SetData(busesIUrb, DataColectivos.LoadBusesIUrb());
            DataColectivos.SetData(busesProv, DataColectivos.LoadBusesProv());
            DataColectivos.SetData(busesMuni, DataColectivos.LoadBusesMuni());
        }
    }
}