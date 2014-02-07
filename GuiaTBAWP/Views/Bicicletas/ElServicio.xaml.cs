using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class ElServicio : PhoneApplicationPage
    {
        public ElServicio()
        {
            InitializeComponent();
            StatusChecker.Check("Bicicletas.ElServicio");
        }
    }
}