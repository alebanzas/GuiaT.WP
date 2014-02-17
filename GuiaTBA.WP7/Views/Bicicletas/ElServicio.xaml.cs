using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.Bicicletas
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